using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configuration;

public class TradeExecutionDetailConfiguration : IEntityTypeConfiguration<TradeExecutionDetail> {
    public void Configure(EntityTypeBuilder<TradeExecutionDetail> builder) {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Venue).HasMaxLength(64).IsRequired();

        // 1:1 with shared PK
        builder.HasOne(e => e.Trade)
               .WithOne(t => t.ExecutionDetail)
               .HasForeignKey<TradeExecutionDetail>(e => e.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

