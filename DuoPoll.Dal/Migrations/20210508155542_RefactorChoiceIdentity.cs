using Microsoft.EntityFrameworkCore.Migrations;

namespace DuoPoll.Dal.Migrations
{
    public partial class RefactorChoiceIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_AspNetUsers_UserId",
                table: "Choices");

            migrationBuilder.DropIndex(
                name: "IX_Choices_Id_UserId_AnonIdentity",
                table: "Choices");

            migrationBuilder.DropIndex(
                name: "IX_Choices_UserId",
                table: "Choices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Choices");

            migrationBuilder.RenameColumn(
                name: "AnonIdentity",
                table: "Choices",
                newName: "UserIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_Id_UserIdentity",
                table: "Choices",
                columns: new[] { "Id", "UserIdentity" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Choices_Id_UserIdentity",
                table: "Choices");

            migrationBuilder.RenameColumn(
                name: "UserIdentity",
                table: "Choices",
                newName: "AnonIdentity");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Choices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Choices_Id_UserId_AnonIdentity",
                table: "Choices",
                columns: new[] { "Id", "UserId", "AnonIdentity" });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_UserId",
                table: "Choices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_AspNetUsers_UserId",
                table: "Choices",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
