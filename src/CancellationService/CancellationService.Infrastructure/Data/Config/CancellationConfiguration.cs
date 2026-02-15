using CancellationService.Domain.Cancellations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CancellationService.Infrastructure.Configurations
{
   public class CancellationConfiguration : IEntityTypeConfiguration<Cancellation>
   {
       public void Configure(EntityTypeBuilder<Cancellation> builder)
       {
           builder.ToTable("Cancellations");

           builder.HasKey(c => c.Id);

           builder.Property(c => c.OrderId).IsRequired();

           builder.OwnsOne(c => c.Reason, b =>
           {
               b.Property(r => r.Value)
                   .HasColumnName("Reason")
                   .IsRequired()
                   .HasMaxLength(500);
           });

           builder.OwnsOne(c => c.Remarks, b =>
           {
               b.Property(r => r.Value)
                   .HasColumnName("Remarks")
                   .HasMaxLength(500);
           });

           builder.Property(c => c.Status).IsRequired();

           builder.Property(c => c.RequestedAt).IsRequired();

           builder.Property(c => c.ProcessedAt);

           builder.Property(c => c.ProcessedBy);

           builder.OwnsOne(c => c.OrderAmount, b =>
           {
               b.Property(m => m.Value)
                   .HasColumnName("OrderAmount")
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
           });

           builder.OwnsOne(c => c.Charges, b =>
           {
               b.Property(m => m.Value)
                   .HasColumnName("CancellationCharges")
                   .HasColumnType("decimal(18,2)");
           });
       }
   }
}
