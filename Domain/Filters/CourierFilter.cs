namespace Domain.Filters;

public class CourierFilter
{
    public int? UserId { get; set; }
    public int? Rating { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}