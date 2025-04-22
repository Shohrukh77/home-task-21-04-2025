using Domain.DTOs.Restaurants;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IRestaurantsService
{
    Task<Response<List<GetRestaurantDto>>> GetAllAsync(RestaurantsFilter filter);
    Task<Response<GetRestaurantDto>> GetAsync(int id);
    Task<Response<GetRestaurantDto>> CreateAsync(CreateRestaurantDto request);
    Task<Response<GetRestaurantDto>> UpdateAsync(int id, UpdateRestaurantsDto request);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<GetRestaurantDto>>> GetRestaurantsActive(int pageNumber = 1,int pageSize = 10);
}