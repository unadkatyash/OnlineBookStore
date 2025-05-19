using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStore.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumns_AddCreatedOnAndRenameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PaymentSummaries",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ReturnedAt",
                table: "BorrowRecords",
                newName: "ReturnedOn");

            migrationBuilder.RenameColumn(
                name: "BorrowedAt",
                table: "BorrowRecords",
                newName: "BorrowedOn");

            migrationBuilder.AddColumn<int>(
                name: "CurrentBorrowedBooks",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatededOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BookCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "BookCategories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBorrowedBooks",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatededOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "PaymentSummaries",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ReturnedOn",
                table: "BorrowRecords",
                newName: "ReturnedAt");

            migrationBuilder.RenameColumn(
                name: "BorrowedOn",
                table: "BorrowRecords",
                newName: "BorrowedAt");
        }
    }
}
