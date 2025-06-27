using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonTrack.Migrations
{
    /// <inheritdoc />
    public partial class Initial23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_AspNetUsers_UserId",
                table: "ServiceTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_Services_ServiceId",
                table: "ServiceTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ServiceTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "ServiceTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_AspNetUsers_UserId",
                table: "ServiceTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_Services_ServiceId",
                table: "ServiceTasks",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_AspNetUsers_UserId",
                table: "ServiceTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_Services_ServiceId",
                table: "ServiceTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ServiceTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "ServiceTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_AspNetUsers_UserId",
                table: "ServiceTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_Services_ServiceId",
                table: "ServiceTasks",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
