using Domain.DTOs.OrderDetail;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderDetailService
{
    Task<Response<List<GetOrderDetailDto>>> GetAllAsync(OrderDetailFilter filter);
    Task<Response<GetOrderDetailDto>> GetAsync(int id);
    Task<Response<GetOrderDetailDto>> CreateAsync(CreateOrderDetailDto request);
    Task<Response<GetOrderDetailDto>> UpdateAsync(int id, UpdateOrderDetailDto request);
    Task<Response<string>> DeleteAsync(int id);
}