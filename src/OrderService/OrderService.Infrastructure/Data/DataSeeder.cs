using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(OrderDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Orders.AnyAsync())
        {
            return; 
        }

        var ordersToSeed = new List<Order>();

        // --- ORDER 1: Pending with Multiple Items ---
        var order1Result = Order.Create(
            Guid.NewGuid(),
            customerId: Guid.NewGuid(),
            orderNumber: "ORD-2026-001",
            billingAddressId: Guid.NewGuid(),
            shippingAddressId: Guid.NewGuid(),
            shippingCost: 15.00m
        );

        if (order1Result.IsSuccess)
        {
            var order = order1Result.Value;
            
            // Adding Items using our Domain Logic
            order.AddItems(new List<OrderItem>
            {
                OrderItem.Create(order.Id, Guid.NewGuid(), "Mechanical Keyboard", 1, 120.00m, 10).Value,
                OrderItem.Create(order.Id, Guid.NewGuid(), "Gaming Mouse", 2, 55.00m, 0).Value
            });

            ordersToSeed.Add(order);
        }

        // --- ORDER 2: Shipped with One Item ---
        var order2Result = Order.Create(
Guid.NewGuid(),
            customerId: Guid.NewGuid(),
            orderNumber: "ORD-2026-002",
            billingAddressId: Guid.NewGuid(),
            shippingAddressId: Guid.NewGuid(),
            shippingCost: 5.00m
        );

        if (order2Result.IsSuccess)
        {
            var order = order2Result.Value;
            
            order.AddOrderItem(OrderItem.Create(
                order.Id, 
                Guid.NewGuid(), 
                "USB-C Cable", 
                5, 
                10.00m, 
                0).Value);

            // Manual Status Transition for Shipped state
            order.TransitionTo(OrderStatus.Shipped);

            ordersToSeed.Add(order);
        }

        // 4. Save to Database
        await dbContext.Orders.AddRangeAsync(ordersToSeed);
        await dbContext.SaveChangesAsync();
    }
}