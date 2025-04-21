using Domain.DTOs.Order;
using Domain.DTOs.User;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderService
{
    Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter);
    Task<Response<GetOrderDto>> GetAsync(int id);
    Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request);
    Task<Response<GetOrderDto>> UpdateAsync(int id, UpdateOrderDto request);
    Task<Response<string>> DeleteAsync(int id);
}