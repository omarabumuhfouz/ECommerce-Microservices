using PaymentService.Domain.Payments;
using PaymentService.Domain.Payments.Enums;

namespace PaymentService.Infrastructure.Data;

public static class PaymentDataSeeder
{
    public static async Task SeedAsync(PaymentDbContext dbContext)
    {
        // 1. Ensure the database is migrated
        await dbContext.Database.MigrateAsync();

        // 2. Check if data already exists
        if (await dbContext.Set<Payment>().AnyAsync())
        {
            return; 
        }

        // 3. Create Seed Data using the Result Pattern
        var paymentsToSeed = new List<Payment>();

        // --- Payment 1: A Successful Credit Card Payment ---
        var payment1 = Payment.Create(
            orderId: Guid.NewGuid(),
            paymentMethod: PaymentMethod.CreditCard,
            amount: 150.00m,
            transactionId: "TXN-CC-998877"
        );

        if (payment1.IsSuccess)
        {
            // We manually complete it for the seed data state
            payment1.Value.MarkAsCompleted("TXN-CC-998877");
            paymentsToSeed.Add(payment1.Value);
        }

        // --- Payment 2: A Pending Cash On Delivery Payment ---
        var payment2 = Payment.Create(
            orderId: Guid.NewGuid(),
            paymentMethod: PaymentMethod.CashOnDelivery,
            amount: 45.50m
        );

        if (payment2.IsSuccess)
        {
            paymentsToSeed.Add(payment2.Value);
        }

        // --- Payment 3: A Failed Attempt ---
        var payment3 = Payment.Create(
            orderId: Guid.NewGuid(),
            paymentMethod: PaymentMethod.PayPal,
            amount: 200.00m
        );

        if (payment3.IsSuccess)
        {
            payment3.Value.MarkAsFailed();
            paymentsToSeed.Add(payment3.Value);
        }

        // 4. Persist to Database
        await dbContext.Set<Payment>().AddRangeAsync(paymentsToSeed);
        await dbContext.SaveChangesAsync();
    }
}