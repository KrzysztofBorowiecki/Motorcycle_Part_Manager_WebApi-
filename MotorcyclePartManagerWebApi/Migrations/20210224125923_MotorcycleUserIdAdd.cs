using Microsoft.EntityFrameworkCore.Migrations;

namespace MotorcyclePartManagerWebApi.Migrations
{
    public partial class MotorcycleUserIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Motorcycles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_CreatedById",
                table: "Motorcycles",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Motorcycles_Users_CreatedById",
                table: "Motorcycles",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motorcycles_Users_CreatedById",
                table: "Motorcycles");

            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_CreatedById",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Motorcycles");
        }
    }
}
