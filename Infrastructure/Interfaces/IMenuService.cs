using Domain.DTOs.Menu;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IMenuService
{
    Task<Response<List<GetMenuDto>>> GetAllAsync(MenuFilter filter);
    Task<Response<GetMenuDto>> GetAsync(int id);
    Task<Response<GetMenuDto>> CreateAsync(CreateMenuDto request);
    Task<Response<GetMenuDto>> UpdateAsync(int id, UpdateMenuDto request);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<GetMenuDto>>> GetMenuAvailable(int pageNumber = 1, int pageSize = 10);
}