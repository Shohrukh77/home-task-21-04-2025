using System.Net;
using AutoMapper;
using Domain.DTOs.Courier;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CourierService(DataContext context, IMapper mapper) : ICourierService
{
    public async Task<Response<List<GetCourierDto>>> GetAllAsync(CourierFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var couriers = context.Couriers.AsQueryable();

            if (filter.UserId != null)
            {
                var courier = couriers.Where(c => c.UserId == filter.UserId);
            }

            if (filter.From != null)
            {
                var courier = couriers.Where(c => c.Rating >= filter.Rating);
            }

            if (filter.To != null)
            {
                var courier = couriers.Where(c => c.Rating <= filter.Rating);
            }

            var maped = mapper.Map<List<GetCourierDto>>(couriers);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetCourierDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Response<GetCourierDto>> GetAsync(int Id)
    {
        var courier = await context.Couriers.FindAsync(Id);

        if (courier == null)
        {
            return new Response<GetCourierDto>(HttpStatusCode.BadRequest, "Courier not found");
        }

        var data = mapper.Map<GetCourierDto>(courier);

        return new Response<GetCourierDto>(data);
    }
    
    public async Task<Response<GetCourierDto>> CreateAsync(CreateCourierDto request)
    {
        var courier = mapper.Map<Courier>(request);
        await context.Couriers.AddAsync(courier);
        var result = await context.SaveChangesAsync();
        var get = mapper.Map<GetCourierDto>(courier);
        return result == 0
            ? new Response<GetCourierDto>(HttpStatusCode.BadRequest, "Courier not added!")
            : new Response<GetCourierDto>(get);
    }

    public async Task<Response<GetCourierDto>> UpdateAsync(int Id, UpdateCourierDto request)
    {
        var exist = await context.Couriers.FindAsync(Id);
        if (exist == null)
        {
            return new Response<GetCourierDto>(HttpStatusCode.BadRequest, "Courier not found");
        }

        exist.UserId = request.UserId;
        exist.Status = request.Status;
        exist.CurrentLocation = request.CurrentLocation;
        exist.TransportType = request.TransportType;

        var result = await context.SaveChangesAsync();
        var courier = mapper.Map<GetCourierDto>(exist);
        return result == 0 ?
            new Response<GetCourierDto>(HttpStatusCode.BadRequest, "Courier not updated!")
            : new Response<GetCourierDto>(courier);
    }
    
    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var courier = await context.Couriers.FindAsync(Id);
        if (courier == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Courier not found");
        }

        context.Remove(courier);
        var res = await context.SaveChangesAsync();
        return res == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Courier not deleted!")
            : new Response<string>("Courier  deleted!");
    }

    //task8
    public async Task<Response<List<GetCourierDto>>> GetTopCouriersAsync()
    {
        var query = context.Couriers
            .OrderByDescending(c => c.Rating)
            .Take(5);
        
        var data = mapper.Map<List<GetCourierDto>>(query);

        return new Response<List<GetCourierDto>>(data);
    }
}