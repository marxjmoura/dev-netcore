using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Developing.API.Infrastructure.Database.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class CreateModelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "model",
                schema: "dev_netcore",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    brand_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_model", x => x.id);
                    table.ForeignKey(
                        name: "fk_model__brand",
                        column: x => x.brand_id,
                        principalSchema: "dev_netcore",
                        principalTable: "brand",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_model_brand_id",
                schema: "dev_netcore",
                table: "model",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "idx_model_name",
                schema: "dev_netcore",
                table: "model",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "model",
                schema: "dev_netcore");
        }
    }
}
