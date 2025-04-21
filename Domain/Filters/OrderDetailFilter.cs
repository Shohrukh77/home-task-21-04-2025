namespace Domain.Filters;

public class OrderDetailFilter
{
    public int? OrderId { get; set; }
    public int? MenuItemId { get; set; }
    public decimal? Price { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}