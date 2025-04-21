namespace Domain.Filters;

public class OrderFilter
{
    public int? UserId { get; set; }
    public int? RestaurantId { get; set; }
    public int? CourierId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}