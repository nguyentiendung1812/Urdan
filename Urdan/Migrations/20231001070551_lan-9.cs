using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urdan.Migrations
{
    /// <inheritdoc />
    public partial class lan9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId1",
                table: "Orders",
                column: "AddressId1",
                unique: true,
                filter: "[AddressId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId1",
                table: "Orders",
                column: "AddressId1",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AddressId1",
                table: "Orders");
        }
    }
}
