using Domain.DTOs.Courier;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICourierService
{
    Task<Response<List<GetCourierDto>>> GetAllAsync(CourierFilter filter);
    Task<Response<GetCourierDto>> GetAsync(int id);
    Task<Response<GetCourierDto>> CreateAsync(CreateCourierDto request);
    Task<Response<GetCourierDto>> UpdateAsync(int id, UpdateCourierDto request);
    Task<Response<string>> DeleteAsync(int id);
}