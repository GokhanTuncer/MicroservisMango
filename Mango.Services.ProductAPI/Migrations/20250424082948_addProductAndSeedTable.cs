using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class addProductAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Burger", "Klasik dana eti burger, cheddar peyniri ile servis edilir.", "https://placehold.co/603x403", "Cheeseburger", 45.0 },
                    { 2, "Pizza", "İnce hamur üzerine bol mozzarella ve pepperoni dilimleri.", "https://placehold.co/603x403", "Pepperoni Pizza", 60.0 },
                    { 3, "Wrap", "Izgara tavuk, yeşillik ve özel sos ile lavaşa sarılmış lezzet.", "https://placehold.co/603x403", "Tavuklu Wrap", 35.0 },
                    { 4, "Appetizer", "Dışı çıtır, içi yumuşak klasik patates kızartması.", "https://placehold.co/603x403", "Patates Kızartması", 20.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
