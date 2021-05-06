using Microsoft.EntityFrameworkCore.Migrations;

namespace DuoPoll.Dal.Migrations
{
    public partial class AddAnon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnonIdentity",
                table: "Choices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Choices_Id_UserId_AnonIdentity",
                table: "Choices",
                columns: new[] { "Id", "UserId", "AnonIdentity" });

            migrationBuilder.DropForeignKey(
                "FK_Polls_AspNetUsers_UserId",
                "Polls");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_UserId",
                table: "Polls",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_UserId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Choices_Id_UserId_AnonIdentity",
                table: "Choices");

            migrationBuilder.DropColumn(
                name: "AnonIdentity",
                table: "Choices");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Polls",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_UserId",
                table: "Polls",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
