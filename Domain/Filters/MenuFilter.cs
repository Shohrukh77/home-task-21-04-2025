﻿namespace Domain.Filters;

public class MenuFilter
{
    public int? RestaurantId { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
    public int? From { get; set; }
    public int? To { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}