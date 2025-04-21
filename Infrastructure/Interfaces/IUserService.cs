using Domain.DTOs.User;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<List<GetUserDto>>> GetAllAsync(UserFilter filter);
    Task<Response<GetUserDto>> GetAsync(int id);
    Task<Response<GetUserDto>> CreateAsync(CreateUserDto request);
    Task<Response<GetUserDto>> UpdateAsync(int id, UpdateUserDto request);
    Task<Response<string>> DeleteAsync(int id);
}