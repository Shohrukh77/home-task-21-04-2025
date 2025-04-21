using System.Net;
using AutoMapper;
using Domain.DTOs.OrderDetail;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class OrderDetailService(DataContext context, IMapper mapper) : IOrderDetailService
{
    public async Task<Response<List<GetOrderDetailDto>>> GetAllAsync(OrderDetailFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var orderDetails = context.OrderDetails.AsQueryable();

            if (filter.MenuItemId != null)
            {
                var res = orderDetails.Where(r => r.MenuItemId == filter.MenuItemId);
            }
            if (filter.OrderId != null)
            {
                var res = orderDetails.Where(r => r.OrderId == filter.OrderId);
            }

            if (filter.From != null)
            {
                var res = orderDetails.Where(r => r.Price >= filter.Price);
            }
            if (filter.To != null)
            {
                var res = orderDetails.Where(r => r.Price <= filter.Price);
            }
            var maped = mapper.Map<List<GetOrderDetailDto>>(orderDetails);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetOrderDetailDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Response<GetOrderDetailDto>> GetAsync(int Id)
    {
        var detail = await context.OrderDetails.FindAsync(Id);
        if (detail == null)
            return new Response<GetOrderDetailDto>(HttpStatusCode.BadRequest, "Order detail not found");

        var dto = mapper.Map<GetOrderDetailDto>(detail);
        return new Response<GetOrderDetailDto>(dto);
    }

    public async Task<Response<GetOrderDetailDto>> CreateAsync(CreateOrderDetailDto request)
    {
        var entity = mapper.Map<OrderDetail>(request);
        await context.OrderDetails.AddAsync(entity);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetOrderDetailDto>(HttpStatusCode.BadRequest, "Order detail not added!");

        var dto = mapper.Map<GetOrderDetailDto>(entity);
        return new Response<GetOrderDetailDto>(dto);
    }

    public async Task<Response<GetOrderDetailDto>> UpdateAsync(int Id, UpdateOrderDetailDto request)
    {
        var exist = await context.OrderDetails.FindAsync(Id);
        if (exist == null)
            return new Response<GetOrderDetailDto>(HttpStatusCode.BadRequest, "Order detail not found");

        mapper.Map(request, exist);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetOrderDetailDto>(HttpStatusCode.BadRequest, "Order detail not updated!");

        var dto = mapper.Map<GetOrderDetailDto>(exist);
        return new Response<GetOrderDetailDto>(dto);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var entity = await context.OrderDetails.FindAsync(Id);
        if (entity == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Order detail not found");

        context.OrderDetails.Remove(entity);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Order detail not deleted!")
            : new Response<string>("Order detail deleted!");
    }

}