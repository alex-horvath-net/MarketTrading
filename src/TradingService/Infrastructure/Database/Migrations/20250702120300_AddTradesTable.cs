using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TradingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTradesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TraderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instrument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    TimeInForce = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StrategyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionRequestedForUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trades",
                columns: new[] { "Id", "ExecutionRequestedForUtc", "Instrument", "OrderType", "PortfolioCode", "Price", "Quantity", "Side", "Status", "StrategyCode", "SubmittedAt", "TimeInForce", "TraderId", "UserComment" },
                values: new object[,]
                {
                    { new Guid("e170ba5f-98d8-4893-8333-e21c5c79dc01"), null, "USD", 0, "P1", 100m, 1m, 0, 0, "S1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "me", "" },
                    { new Guid("e170ba5f-98d8-4893-8333-e21c5c79dc02"), null, "EUR", 0, "P1", 100m, 1m, 0, 0, "S1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "me", "" },
                    { new Guid("e170ba5f-98d8-4893-8333-e21c5c79dc03"), null, "GBD", 0, "P1", 100m, 1m, 0, 0, "S1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "me", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
