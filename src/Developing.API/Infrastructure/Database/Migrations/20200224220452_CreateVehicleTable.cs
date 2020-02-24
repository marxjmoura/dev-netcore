using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Developing.API.Infrastructure.Database.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class CreateVehicleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicle",
                schema: "dev_netcore",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model_id = table.Column<int>(nullable: false),
                    model_year = table.Column<int>(maxLength: 4, nullable: false),
                    fuel = table.Column<string>(maxLength: 20, nullable: false),
                    value = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle__model",
                        column: x => x.model_id,
                        principalSchema: "dev_netcore",
                        principalTable: "model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_model_id",
                schema: "dev_netcore",
                table: "vehicle",
                column: "model_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle",
                schema: "dev_netcore");
        }
    }
}
