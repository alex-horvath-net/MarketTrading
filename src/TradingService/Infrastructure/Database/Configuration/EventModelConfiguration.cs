using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database.Configurations;

public class EventModelConfiguration : IEntityTypeConfiguration<EventModel> {
    public void Configure(EntityTypeBuilder<EventModel> builder) {
        builder.ToTable("Events");

        builder.HasKey(e => new { e.Id, e.SequenceNumber }).IsClustered(true);

        builder.Property(e => e.RaisedAt).HasColumnType("datetime2(7)");
        builder.Property(e => e.EventTypeName).HasMaxLength(256).IsRequired();
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasData(SeedData());
    }

    private EventModel[] SeedData() => [];