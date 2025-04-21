namespace Domain.Filters;

public class RestaurantsFilter
{
    public string? Name { get; set; }
    public decimal? Rating { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}