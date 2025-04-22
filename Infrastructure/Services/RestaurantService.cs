using System.Net;
using AutoMapper;
using Domain.DTOs.Restaurants;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RestaurantService(DataContext context, IMapper mapper) : IRestaurantsService
{
    public async Task<Response<List<GetRestaurantDto>>> GetAllAsync(RestaurantsFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var restaurants = context.Restaurants.AsQueryable();

            if (filter.Name != null)
            {
                var res = restaurants.Where(r => r.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.From != null)
            {
                var res = restaurants.Where(r => r.Rating >= filter.Rating);
            }

            if (filter.To != null)
            {
                var res = restaurants.Where(r => r.Rating <= filter.Rating);
            }

            var maped = mapper.Map<List<GetRestaurantDto>>(restaurants);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetRestaurantDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Response<GetRestaurantDto>> GetAsync(int Id)
    {
        var restaurant = await context.Restaurants.FindAsync(Id);
        if (restaurant == null)
            return new Response<GetRestaurantDto>(HttpStatusCode.BadRequest, "Restaurant not found");

        var dto = mapper.Map<GetRestaurantDto>(restaurant);
        return new Response<GetRestaurantDto>(dto);
    }

    public async Task<Response<GetRestaurantDto>> CreateAsync(CreateRestaurantDto request)
    {
        var entity = mapper.Map<Restaurant>(request);
        await context.Restaurants.AddAsync(entity);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetRestaurantDto>(HttpStatusCode.BadRequest, "Restaurant not added!");

        var dto = mapper.Map<GetRestaurantDto>(entity);
        return new Response<GetRestaurantDto>(dto);
    }

    public async Task<Response<GetRestaurantDto>> UpdateAsync(int Id, UpdateRestaurantsDto request)
    {
        var exist = await context.Restaurants.FindAsync(Id);
        if (exist == null)
            return new Response<GetRestaurantDto>(HttpStatusCode.BadRequest, "Restaurant not found");

        mapper.Map(request, exist);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetRestaurantDto>(HttpStatusCode.BadRequest, "Restaurant not updated!");

        var dto = mapper.Map<GetRestaurantDto>(exist);
        return new Response<GetRestaurantDto>(dto);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var entity = await context.Restaurants.FindAsync(Id);
        if (entity == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Restaurant not found");

        context.Restaurants.Remove(entity);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Restaurant not deleted!")
            : new Response<string>("Restaurant deleted!");
    }

    //task1
    public async Task<Response<List<GetRestaurantDto>>> GetRestaurantsActive(int pageNumber = 1, int pageSize = 10)
    {
        var query = context.Restaurants
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.Rating);

        var restaurants = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalRecords = await query.CountAsync();

        var data = mapper.Map<List<GetRestaurantDto>>(restaurants);

        return new PagedResponse<List<GetRestaurantDto>>(data, pageSize, pageNumber, totalRecords);
    }

    //task11
    public async Task<Response<List<GetTopRestDto>>> GetTopRestaurants(int pageNumber = 1, int pageSize = 10)
    {
        var now = DateTime.Now.AddMonths(-1);

        var query = context.Orders
            .Where(o => o.CreatedAt >= now)
            .GroupBy(o => o.Restaurant)
            .Select(g => new GetTopRestDto
            {
                Name = g.Key.Name,
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalRecords = await query.CountAsync();
        return new PagedResponse<List<GetTopRestDto>>(data, pageNumber, pageSize, totalRecords);
    }
}