using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.OrderAPI.Migrations
{
    public partial class pushOrderTableToDbnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_OrderHeaders_OrderHeaderId",
                table: "CartDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails");

            migrationBuilder.RenameTable(
                name: "CartDetails",
                newName: "OrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_OrderHeaderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "OrderDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "CartDetails");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_OrderHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails",
                column: "OrderDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_OrderHeaders_OrderHeaderId",
                table: "CartDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
