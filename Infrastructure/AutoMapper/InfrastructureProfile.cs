using AutoMapper;
using Domain.DTOs.Courier;
using Domain.DTOs.Menu;
using Domain.DTOs.Order;
using Domain.DTOs.OrderDetail;
using Domain.DTOs.Restaurants;
using Domain.DTOs.User;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<Courier, GetCourierDto>();
        CreateMap<Menu, GetMenuDto>();
        CreateMap<Order, GetOrderDto>();
        CreateMap<OrderDetail, GetOrderDetailDto>();
        CreateMap<Restaurant, GetRestaurantDto>();
        CreateMap<User, GetUserDto>();
    }
}