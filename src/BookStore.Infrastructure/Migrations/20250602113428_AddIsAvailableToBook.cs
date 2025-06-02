using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAvailableToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("66d3cc1f-e815-4f44-988b-aee7e6e4ed27"), "50d7c974-dc3d-4684-a5ba-ef8e2b41d92d", "Customer", "CUSTOMER" },
                    { new Guid("6be5768f-b8da-4bed-b24c-b416a735faed"), "5e274512-e7c1-4d42-9a84-c26584840557", "Admin", "ADMIN" },
                    { new Guid("d81a5431-f6da-4700-936b-4af1dcc26849"), "72613521-f6ac-4379-a703-5e99b554aa76", "Staff", "STAFF" }
                });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7f80a16-067d-4bef-8942-5d94968ceb8c", "AQAAAAIAAYagAAAAEFvcar/rHPgvu5Za5kc9nUNu8F+VnvasxhJo7cAh4Gslr0oLMH8UJn6hxnOs3xIgyA==", "6a0e0d71-391e-4874-b098-8de24536327f" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { new Guid("6be5768f-b8da-4bed-b24c-b416a735faed"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e"), "UserRole" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("66d3cc1f-e815-4f44-988b-aee7e6e4ed27"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d81a5431-f6da-4700-936b-4af1dcc26849"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6be5768f-b8da-4bed-b24c-b416a735faed"), new Guid("9c9155c6-582f-431e-9c42-1e24e9c6219e") });

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6be5768f-b8da-4bed-b24c-b416a735faed"));

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Books");

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
    }
}
