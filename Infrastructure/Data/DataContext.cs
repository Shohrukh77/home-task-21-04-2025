using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> option) : DbContext(option)
{
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>()
            .HasOne(m => m.Restaurant)
            .WithMany(r => r.Menus)
            .HasForeignKey(m => m.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Courier)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CourierId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Courier>()
            .HasOne(c => c.User)
            .WithOne(u => u.CourierProfile)
            .HasForeignKey<Courier>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.MenuItem)
            .WithMany(m => m.OrderDetails)
            .HasForeignKey(od => od.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}