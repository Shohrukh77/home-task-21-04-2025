using System.Net;
using AutoMapper;
using Domain.DTOs.Order;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService(DataContext context, IMapper  mapper) : IOrderService
{
    public async Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var orders = context.Orders.AsQueryable();

            if (filter.UserId != null)
            {
                var order = orders.Where(o =>  o.UserId == filter.UserId);
            }
            if (filter.CourierId != null)
            {
                var order = orders.Where(o =>  o.CourierId == filter.CourierId);
            }
            if (filter.RestaurantId != null)
            {
                var order = orders.Where(o =>  o.RestaurantId == filter.RestaurantId);
            }
            
            var maped = mapper.Map<List<GetOrderDto>>(orders);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetOrderDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Response<GetOrderDto>> GetAsync(int Id)
    {
        var order = await context.Orders.FindAsync(Id);
        if (order == null)
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "Order not found");

        var dto = mapper.Map<GetOrderDto>(order);
        return new Response<GetOrderDto>(dto);
    }

    public async Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request)
    {
        var entity = mapper.Map<Order>(request);
        await context.Orders.AddAsync(entity);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "Order not created!");

        var dto = mapper.Map<GetOrderDto>(entity);
        return new Response<GetOrderDto>(dto);
    }

    public async Task<Response<GetOrderDto>> UpdateAsync(int Id, UpdateOrderDto request)
    {
        var exist = await context.Orders.FindAsync(Id);
        if (exist == null)
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "Order not found");

        mapper.Map(request, exist);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "Order not updated!");

        var dto = mapper.Map<GetOrderDto>(exist);
        return new Response<GetOrderDto>(dto);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var order = await context.Orders.FindAsync(Id);
        if (order == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Order not found");

        context.Orders.Remove(order);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Order not deleted!")
            : new Response<string>("Order deleted!");
    }
    //task 3
    public async Task<Response<List<GetOrdersCountByStatusDto>>> GetOrdersByStatus(OrderStatus orderStatus, int pageNumber = 1, int pageSize = 10)
    {
        var result = context.Orders
            .GroupBy(o => o.OrderStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() });
        
        var orders = await result
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalRecords = await result.CountAsync();
        
        var  data = mapper.Map<List<GetOrdersCountByStatusDto>>(orders);

        return new PagedResponse<List<GetOrdersCountByStatusDto>>(data, pageNumber, pageSize, totalRecords);
    }
    
    //task6
    public async Task<Response<List<GetOrderDto>>> GetOrdersByCourier(int courierId, int pageNumber = 1, int pageSize = 10)
    {
        var query = context.Orders
            .Where(o => o.CourierId == courierId)
            .OrderByDescending(o => o.Id);

        var orders = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalRecords = await query.CountAsync();

        var data = mapper.Map<List<GetOrderDto>>(orders);

        return new PagedResponse<List<GetOrderDto>>(data, pageSize, pageNumber, totalRecords);
    }

    //task7
    public async Task<Response<decimal>> GetOrderTotalToday()
    {
        var today = DateTime.Today;
        var result = await context.Orders
            .Where(o => o.DeliveredAt == today)
            .SumAsync(o => o.TotalAmount);

        return new Response<decimal>(result);
    }
    
    //task9
    public async Task<Response<List<GetOrderDto>>> GetOrdersAboveAveragePagedAsync(int pageNumber = 1, int pageSize = 10)
    {
        var avg = await context.Orders.AverageAsync(o => o.TotalAmount);

        var query = context.Orders
            .Where(o => o.TotalAmount > avg)
            .OrderByDescending(o => o.TotalAmount);

        var orders = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalRecords = await query.CountAsync();

        var data = mapper.Map<List<GetOrderDto>>(orders);

        return new PagedResponse<List<GetOrderDto>>(data, pageSize, pageNumber, totalRecords);
    }
}