using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WymianaWaluty.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCurrencyBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "CurrencyBalances",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "CurrencyBalances",
                newName: "CurrencyCode");
        }
    }
}
