using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38ed2da3-4da5-4e6b-b695-79a3879550b9"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("64ecd975-ad9f-4085-b383-09fc0de88858"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("56eb3f1a-c1a6-455a-a39c-fd69e11aac64"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("56eb3f1a-c1a6-455a-a39c-fd69e11aac64"));

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("16ad3fba-e893-47c3-9f97-7be024a50a32"), "7bd952f5-6c71-4b6b-88e8-a0706eea6236", "Admin", "ADMIN" },
                    { new Guid("7b361b56-0593-419a-be6c-2983894b07ba"), "2b77a180-c9b2-4e3b-bba6-516a2bdbc84b", "Customer", "CUSTOMER" },
                    { new Guid("f4c6ce39-32e3-4688-aaab-b114407b52e0"), "62fd9318-968f-49b9-a1b9-887c539dea2c", "Staff", "STAFF" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d41ec3ce-ac91-4b6d-9207-f351aa692958", "AQAAAAIAAYagAAAAEDZfWnw+1AIKmn7ffadgiux8Pfdc+pAykTGrNi1Mrs5F3JqsSRWNnWhd7YXeRoeA0Q==", "c393739f-1f15-4821-bb1e-b761d02b9c2c" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { new Guid("16ad3fba-e893-47c3-9f97-7be024a50a32"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"), "UserRole" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BookId",
                table: "CartItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7b361b56-0593-419a-be6c-2983894b07ba"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f4c6ce39-32e3-4688-aaab-b114407b52e0"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("16ad3fba-e893-47c3-9f97-7be024a50a32"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("16ad3fba-e893-47c3-9f97-7be024a50a32"));

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("38ed2da3-4da5-4e6b-b695-79a3879550b9"), "ef68225e-45b6-4bdd-9eaa-87e13bd8ece3", "Staff", "STAFF" },
                    { new Guid("56eb3f1a-c1a6-455a-a39c-fd69e11aac64"), "70bd7fbe-fcb0-4a25-bdd3-b523d2bff9df", "Admin", "ADMIN" },
                    { new Guid("64ecd975-ad9f-4085-b383-09fc0de88858"), "83c401a6-35f5-42f3-8e0c-a020f7947896", "Customer", "CUSTOMER" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "97cb39ae-1816-45df-8a2e-f46f47189a9e", "AQAAAAIAAYagAAAAELUZn/K9hBuq8n1GPFpQmP4bTiyFkq4LPbUBjneZcqaQ+a+PyWGjR8UewYKf7JNL1w==", "225aaba3-520b-419e-99df-165deac339e4" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { new Guid("56eb3f1a-c1a6-455a-a39c-fd69e11aac64"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"), "UserRole" });
        }
    }
}
