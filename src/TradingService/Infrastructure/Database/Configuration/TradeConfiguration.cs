using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configurations;


       public class TradeConfiguration : IEntityTypeConfiguration<Trade> {
        public void Configure(EntityTypeBuilder<Trade> builder) {
            builder.HasKey(trade => trade.Id).IsClustered(true);

            builder.HasIndex(trade => trade.TraderId).IsClustered(false);
            builder.HasIndex(trade => trade.Instrument).IsClustered(false);
            builder.HasIndex(trade => trade.PortfolioCode).IsClustered(false);

            // one-to-many (Trade > TradeLegs)
            builder.HasMany(trade => trade.Legs)
                   .WithOne(lag => lag.Trade)
                   .HasForeignKey(l => l.TradeId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many (Trade <> Tag)
            builder.HasMany(trade => trade.Tags)
                   .WithMany(tag => tag.Trades)
                   .UsingEntity(tradeTags => tradeTags.ToTable("TradeTags"));

            // one-to-one (Trade <> TradeExecutionDetail)
            builder.HasOne(trade => trade.ExecutionDetail)
                   .WithOne(e => e.Trade)
                   .HasForeignKey<TradeExecutionDetail>(execution => execution.Id)
                   .OnDelete(DeleteBehavior.Cascade);

            // Global Query Filter for soft deletes
            builder.HasQueryFilter(trade => !trade.IsDeleted);

            // Shadow Property for last updated timestamp
            builder.Property<DateTime>("LastUpdatedAt");

            builder.HasData(SeedData());
        }

        private List<Trade> SeedData() => [
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC01"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "USD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" },
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC02"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "EUR", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" },
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC03"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "GBD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" }
        ];
    }
