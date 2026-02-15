using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInboxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxMessages",
                table: "InboxMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InboxMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "InboxMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxMessages",
                table: "InboxMessages",
                columns: new[] { "Id", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_InboxMessages_Id_Name",
                table: "InboxMessages",
                columns: new[] { "Id", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxMessages",
                table: "InboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_InboxMessages_Id_Name",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "Error",
                table: "InboxMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InboxMessages",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxMessages",
                table: "InboxMessages",
                column: "Id");
        }
    }
}
