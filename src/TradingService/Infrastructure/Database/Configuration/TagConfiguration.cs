using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configuration;
public class TagConfiguration : IEntityTypeConfiguration<Tag> {
    public void Configure(EntityTypeBuilder<Tag> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(64).IsRequired();

        // Many-to-many configured from Trade side (no need to repeat here)
    }
}
