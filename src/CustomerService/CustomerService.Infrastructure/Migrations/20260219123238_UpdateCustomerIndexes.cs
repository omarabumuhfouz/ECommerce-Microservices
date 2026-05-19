using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                    name: "FK_Addresses_Customers_CustomerId",
                    table: "Addresses");


            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UserId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedOnUtc",
                table: "Customers",
                column: "CreatedOnUtc")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                    name: "FK_Addresses_Customers_CustomerId",
                    table: "Addresses",
                    column: "CustomerId",
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                    name: "FK_Addresses_Customers_CustomerId",
                    table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedOnUtc",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

migrationBuilder.AddForeignKey(
        name: "FK_Addresses_Customers_CustomerId",
        table: "Addresses",
        column: "CustomerId",
        principalTable: "Customers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
        }
    }
}
