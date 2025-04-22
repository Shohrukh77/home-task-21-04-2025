using System.Net;
using AutoMapper;
using Domain.DTOs.Menu;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class MenuService(DataContext context, IMapper mapper ) : IMenuService
{
    public async Task<Response<List<GetMenuDto>>> GetAllAsync(MenuFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var menus = context.Menus.AsQueryable();

            if (filter.RestaurantId != null)
            {
                var menu = menus.Where(m => m.RestaurantId == filter.RestaurantId);
            }

            if (filter.Name != null)
            {
                var menu = menus.Where(m => m.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.Category != null)
            {
                var menu = menus.Where(m => m.Category == filter.Category);
            }

            if (filter.From != null)
            {
                var menu = menus.Where(m => m.Price >=filter.Price);
            }

            if (filter.To != null)
            {
                var menu = menus.Where(m => m.Price <= filter.Price);
            }
            var maped = mapper.Map<List<GetMenuDto>>(menus);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetMenuDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Response<GetMenuDto>> GetAsync(int Id)
    {
        var menu = await context.Menus.FindAsync(Id);

        if (menu == null)
        {
            return new Response<GetMenuDto>(HttpStatusCode.BadRequest, "Menu not found");
        }

        var data = mapper.Map<GetMenuDto>(menu);

        return new Response<GetMenuDto>(data);
    }
    
    public async Task<Response<GetMenuDto>> CreateAsync(CreateMenuDto request)
    {
        var menu = mapper.Map<Menu>(request);
        await context.Menus.AddAsync(menu);
        var result = await context.SaveChangesAsync();
        var get = mapper.Map<GetMenuDto>(menu);
        return result == 0
            ? new Response<GetMenuDto>(HttpStatusCode.BadRequest, "Menu not added!")
            : new Response<GetMenuDto>(get);
    }

    public async Task<Response<GetMenuDto>> UpdateAsync(int Id, UpdateMenuDto request)
    {
        var exist = await context.Menus.FindAsync(Id);
        if (exist == null)
        {
            return new Response<GetMenuDto>(HttpStatusCode.BadRequest, "Menu not found");
        }

        exist.RestaurantId = request.RestaurantId;
        exist.Category = request.Category;
        exist.Price = request.Price;
        exist.Name = request.Name;
        exist.Description = request.Description;
        exist.IsAvailable = request.IsAvailable;
        exist.PreparationTime = request.PreparationTime;
        exist.Weight = request.Weight;
        exist.PhotoUrl = request.PhotoUrl;

        var result = await context.SaveChangesAsync();
        var menu = mapper.Map<GetMenuDto>(exist);
        return result == 0 ?
            new Response<GetMenuDto>(HttpStatusCode.BadRequest, "Menu not updated!")
            : new Response<GetMenuDto>(menu);
    }
    
    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var menu = await context.Couriers.FindAsync(Id);
        if (menu == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Menu not found");
        }

        context.Remove(menu);
        var res = await context.SaveChangesAsync();
        return res == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Menu not deleted!")
            : new Response<string>("Menu  deleted!");
    }
    //task2
    public async Task<Response<List<GetMenuDto>>> GetMenuAvailable(int pageNumber = 1, int pageSize = 10)
    {
        var query = context.Menus
            .Where(m => m.Price <= 1000);
        var menus = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalRecords = await query.CountAsync();
        
        var data = mapper.Map<List<GetMenuDto>>(menus);

        return new PagedResponse<List<GetMenuDto>>(data, pageNumber, pageSize, totalRecords);
    }

    //task4
    public async Task<Response<List<GetMenuByCategoryDto>>> GetMenuByCategory(int pageNumber = 1, int pageSize = 10)
    {
        var query = context.Menus
            .GroupBy(m => m.Category)
            .Select(g => new { Category = g.Key, AveragePrice = g.Average(m => m.Price) });
        var res = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalRecords = await query.CountAsync();
        
        var data = mapper.Map<List<GetMenuByCategoryDto>>(res);
        return new PagedResponse<List<GetMenuByCategoryDto>>(data, pageNumber, pageSize, totalRecords);
    }

    //task10
    public async Task<Response<List<GetMenuPopularCategoryDto>>> GetMenuPopularCategoty(int pageNumber = 1, int pageSize = 10)
    {
        var query = context.Menus
            .GroupBy(m => m.Category)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Category = g.Key, Count = g.Count() });

        var res = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var totalRecords = await query.CountAsync();
        var data = mapper.Map<List<GetMenuPopularCategoryDto>>(res);
        return new PagedResponse<List<GetMenuPopularCategoryDto>>(data, pageNumber, pageSize, totalRecords);
    }
}