using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_Incomes_IncomeId",
                table: "ServiceTasks");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_Incomes_IncomeId",
                table: "ServiceTasks",
                column: "IncomeId",
                principalTable: "Incomes",
                principalColumn: "Id");
        }
    }
}
