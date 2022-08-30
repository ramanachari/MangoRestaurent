using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ProductAPI.Migrations
{
    public partial class AddProductToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Appetizer", "The samosas are a fried or baked pastry with a savory filling, such as spiced potatoes, onions, peas, lentils, and minced meat (lamb, beef or chicken).<br/>Sweet samosas are also sold in the cities of Pakistan including Peshawar; these sweet samosas contain no filling and are dipped in thick sugar syrup.", "https://medhasridotnetmastery.blob.core.windows.net/mango/14.jpg", "Samosa", 15.0 },
                    { 2, "Appetizer", "Paneer tikka is an Indian dish made from chunks of paneer marinated in spices and grilled in a tandoor.<br/>It is a vegetarian alternative to chicken tikka and other meat dishes. It is a popular dish that is widely available in India and countries with an Indian diaspora.", "https://medhasridotnetmastery.blob.core.windows.net/mango/12.jpg", "Paneer Tikka", 13.99 },
                    { 3, "Dessert", "A pie is a baked dish which is usually made of a pastry dough casing that contains a filling of various sweet or savoury ingredients.<br/>A filled pie (also single-crust or bottom-crust), has pastry lining the baking dish, and the filling is placed on top of the pastry but left open.", "https://medhasridotnetmastery.blob.core.windows.net/mango/11.jpg", "Sweet Pie", 10.99 },
                    { 4, "Entree", "Pav bhaji is a spiced mixture of mashed vegetables in a thick gravy served with bread.<br/>Vegetables in the curry may commonly include potatoes, onions, carrots, chillies, peas, bell peppers and tomatoes. Street sellers usually cook the curry on a flat griddle (tava) and serve the dish hot.", "https://medhasridotnetmastery.blob.core.windows.net/mango/13.jpg", "Pav Bhaji", 15.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
