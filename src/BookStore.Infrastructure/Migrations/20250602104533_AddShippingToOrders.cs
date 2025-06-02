using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_FullName",
                table: "Orders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Line1",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Line2",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PostalCode",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_State",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5765dd50-6d8f-4e49-a249-98de646669a3"), "d763e239-0860-4fc2-a4b2-e78dbf0f0e84", "Staff", "STAFF" },
                    { new Guid("bc5f7ac7-11e4-4899-9710-3e1ce9964d68"), "ca808efd-ea00-4315-811f-7e21d3e6e519", "Customer", "CUSTOMER" },
                    { new Guid("cecbfaa0-2629-464f-9950-d01610823867"), "5bfc5c3c-d2c4-47a9-8e73-e7a9082bc6fe", "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64bc1f9f-f460-41d3-9486-a056785f4eb9", "AQAAAAIAAYagAAAAEL855qRSLmxK1aLl1a95oe1C1GwUMwrueCwsHRdJD31bXyjeJRjPeW+9xfjqOXP5LQ==", "9d58a9da-e2e7-4d60-9001-6ab3ef06e1f9" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { new Guid("cecbfaa0-2629-464f-9950-d01610823867"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"), "UserRole" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5765dd50-6d8f-4e49-a249-98de646669a3"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bc5f7ac7-11e4-4899-9710-3e1ce9964d68"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cecbfaa0-2629-464f-9950-d01610823867"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cecbfaa0-2629-464f-9950-d01610823867"));

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_FullName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Line1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Line2",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PostalCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_State",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Orders");

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
        }
    }
}
