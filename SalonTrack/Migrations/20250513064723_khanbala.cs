using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonTrack.Migrations
{
    /// <inheritdoc />
    public partial class khanbala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ServiceTasks");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceTasks");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "IncomeId",
                table: "ServiceTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTasks_IncomeId",
                table: "ServiceTasks",
                column: "IncomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_Incomes_IncomeId",
                table: "ServiceTasks",
                column: "IncomeId",
                principalTable: "Incomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_Incomes_IncomeId",
                table: "ServiceTasks");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTasks_IncomeId",
                table: "ServiceTasks");

            migrationBuilder.DropColumn(
                name: "IncomeId",
                table: "ServiceTasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ServiceTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceTasks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
