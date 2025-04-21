using System.Net;
using AutoMapper;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class UserService(DataContext context, IMapper mapper) : IUserService
{
    public async Task<Response<List<GetUserDto>>> GetAllAsync(UserFilter filter)
    {
        try
        {
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var users = context.Users.AsQueryable();

            if (filter.Name != null)
            {
                var user = users.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
            }
            
            var maped = mapper.Map<List<GetUserDto>>(users);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetUserDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Response<GetUserDto>> GetAsync(int Id)
    {
        var user = await context.Users.FindAsync(Id);
        if (user == null)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not found");
        }

        var data = mapper.Map<GetUserDto>(user);
        return new Response<GetUserDto>(data);
    }

    public async Task<Response<GetUserDto>> CreateAsync(CreateUserDto request)
    {
        var user = mapper.Map<User>(request);
        await context.Users.AddAsync(user);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetUserDto>(user);
        return result == 0
            ? new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not added!")
            : new Response<GetUserDto>(data);
    }

    public async Task<Response<GetUserDto>> UpdateAsync(int Id, UpdateUserDto request)
    {
        var user = await context.Users.FindAsync(Id);
        if (user == null)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not found");
        }

        user.Name = request.Name;
        user.Email = request.Email;
        user.Phone = request.Phone;
        user.Password = request.Password;
        user.Address = request.Address;
        user.Role = request.Role;

        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetUserDto>(user);
        return result == 0
            ? new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not updated!")
            : new Response<GetUserDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var user = await context.Users.FindAsync(Id);
        if (user == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "User not found");
        }

        context.Users.Remove(user);
        var result = await context.SaveChangesAsync();
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "User not deleted!")
            : new Response<string>("User deleted!");
    }
}