using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartWithTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Carts",
                newName: "PriceTotal");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartItem",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceTotal",
                table: "CartItem",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceTotalWithDiscount",
                table: "CartItem",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "PriceTotalWithDiscount",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "PriceTotal",
                table: "Carts",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "CartItem",
                newName: "Price");
        }
    }
}
