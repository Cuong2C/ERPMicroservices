using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemCatalog.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_Code",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Code",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Items_Code",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Units",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Tags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ItemUnits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ItemCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ItemSellPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSellPrices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_TenantId_Code",
                table: "Units",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TenantId_Code",
                table: "Tags",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_TenantId_Code",
                table: "Items",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TenantId_Code",
                table: "Categories",
                columns: new[] { "TenantId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSellPrices");

            migrationBuilder.DropIndex(
                name: "IX_Units_TenantId_Code",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TenantId_Code",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Items_TenantId_Code",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Categories_TenantId_Code",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Code",
                table: "Units",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Code",
                table: "Tags",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Code",
                table: "Items",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);
        }
    }
}
