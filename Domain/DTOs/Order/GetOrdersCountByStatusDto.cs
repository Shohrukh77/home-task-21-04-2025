using Domain.Enums;

namespace Domain.DTOs.Order;

public class GetOrdersCountByStatusDto
{
    public int Count { get; set; }
    public OrderStatus Status { get; set; }
}