using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configuration;

public class TradeLegDetailConfiguration : IEntityTypeConfiguration<TradeLegDetail> {
    public void Configure(EntityTypeBuilder<TradeLegDetail> builder) {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.ClearingBroker).HasMaxLength(100);
        builder.Property(d => d.ExecutionVenue).HasMaxLength(100);
        builder.Property(d => d.SettlementCurrency).HasMaxLength(10);
    }
}