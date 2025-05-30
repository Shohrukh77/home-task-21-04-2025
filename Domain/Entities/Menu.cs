﻿namespace Domain.Entities;

public class Menu
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public bool IsAvailable { get; set; }
    public int PreparationTime { get; set; }
    public int Weight { get; set; }
    public string PhotoUrl { get; set; }
    
    public virtual Restaurant Restaurant { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}