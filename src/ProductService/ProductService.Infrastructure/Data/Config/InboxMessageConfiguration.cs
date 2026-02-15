using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Infrastructure.InboxMessages;

namespace ProductService.Infrastructure.Data.Config;
internal sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("InboxMessages");

        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });

        builder.HasIndex(e => new { e.Id, e.Name });
    }
}
