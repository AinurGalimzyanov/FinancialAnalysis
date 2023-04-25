using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Operation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDalId",
                table: "Operation",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operation_UserDalId",
                table: "Operation",
                column: "UserDalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_Users_UserDalId",
                table: "Operation",
                column: "UserDalId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Users_UserDalId",
                table: "Operation");

            migrationBuilder.DropIndex(
                name: "IX_Operation_UserDalId",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "UserDalId",
                table: "Operation");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Categories",
                type: "text",
                nullable: true);
        }
    }
}
