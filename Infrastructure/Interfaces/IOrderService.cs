using Domain.DTOs.Order;
using Domain.DTOs.User;
using Domain.Enums;
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

    Task<Response<List<GetOrdersCountByStatusDto>>> GetOrdersByStatus(OrderStatus orderStatus, int page = 1,
        int pageSize = 10);

    Task<Response<List<GetOrderDto>>> GetOrdersByCourier(int courierId, int page = 1, int pageSize = 10);
    Task<Response<decimal>> GetOrderTotalToday();
    Task<Response<List<GetOrderDto>>> GetOrdersAboveAveragePagedAsync(int pageNumber = 1, int pageSize = 10);
}