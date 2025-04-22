namespace Domain.DTOs.User;

public class GetUsersWithOrdersDto : GetUserDto
{
    public int OrdersCount { get; set; }
}