using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configuration;
public class TradeLegConfiguration : IEntityTypeConfiguration<TradeLeg> {
    public void Configure(EntityTypeBuilder<TradeLeg> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Instrument).HasMaxLength(16).IsRequired();

        builder.HasOne(x => x.Trade)
               .WithMany(t => t.Legs)
               .HasForeignKey(x => x.TradeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Detail)
               .WithOne(d => d.TradeLeg)
               .HasForeignKey<TradeLegDetail>(d => d.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
