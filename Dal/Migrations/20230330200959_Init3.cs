using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Categories_CategoriesListId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Operation_OperationListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CategoriesListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OperationListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoriesListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OperationListId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserDalId",
                table: "Operation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Operation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserDalId",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Operation_UserDalId",
                table: "Operation",
                column: "UserDalId");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_UserId",
                table: "Operation",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserDalId",
                table: "Categories",
                column: "UserDalId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserDalId",
                table: "Categories",
                column: "UserDalId",
                principalTable: "Users",
                principalColumn: "Id");

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
                name: "FK_Categories_Users_UserDalId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Users_UserDalId",
                table: "Operation");

            migrationBuilder.DropIndex(
                name: "IX_Operation_UserDalId",
                table: "Operation");

            migrationBuilder.DropIndex(
                name: "IX_Operation_UserId",
                table: "Operation");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserDalId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserDalId",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "UserDalId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoriesListId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OperationListId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CategoriesListId",
                table: "Users",
                column: "CategoriesListId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OperationListId",
                table: "Users",
                column: "OperationListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Categories_CategoriesListId",
                table: "Users",
                column: "CategoriesListId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Operation_OperationListId",
                table: "Users",
                column: "OperationListId",
                principalTable: "Operation",
                principalColumn: "Id");
        }
    }
}
