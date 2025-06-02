using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8ed942f9-561d-4324-a494-cea5ef59c90f"), "7d5924d3-57fa-43e9-be35-5090df02c6fc", "Admin", "ADMIN" },
                    { new Guid("92c9ac0e-0999-431c-9e05-8a242c7b2634"), "efd2f8b4-c77e-46ca-837b-0bc1da2162b9", "Customer", "CUSTOMER" },
                    { new Guid("e670cf64-e2f8-48c4-b91e-f0f0f56747f1"), "395408e1-53f0-427e-baac-1469430f3812", "Staff", "STAFF" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2feb81d-2277-427f-b06d-e75addb7ab62", "AQAAAAIAAYagAAAAEF8DdtOlp+ic26wK8t/QvXz077vHsNh6IUs8LPKtz2r3rbCeZkPoHR6oB9HHz1ezLQ==", "fb94f937-3137-4eed-8c66-58098755bcd7" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { new Guid("8ed942f9-561d-4324-a494-cea5ef59c90f"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"), "UserRole" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("92c9ac0e-0999-431c-9e05-8a242c7b2634"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e670cf64-e2f8-48c4-b91e-f0f0f56747f1"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8ed942f9-561d-4324-a494-cea5ef59c90f"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8ed942f9-561d-4324-a494-cea5ef59c90f"));

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
        }
    }
}
