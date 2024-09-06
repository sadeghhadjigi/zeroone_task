using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroOneTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    route_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    origin_city_id = table.Column<long>(type: "bigint", nullable: false),
                    destination_city_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.route_id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    OriginCityId = table.Column<long>(type: "bigint", nullable: false),
                    DestinationCityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => new { x.AgencyId, x.OriginCityId, x.DestinationCityId });
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    flight_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    arrival_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    route_id = table.Column<long>(type: "bigint", nullable: false),
                    airline_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.flight_id);
                    table.ForeignKey(
                        name: "FK_Flights_Routes_route_id",
                        column: x => x.route_id,
                        principalTable: "Routes",
                        principalColumn: "route_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_route_id",
                table: "Flights",
                column: "route_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
