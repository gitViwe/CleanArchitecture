using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class Authorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppIdentityRoleId",
                table: "RoleClaims",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_AppIdentityRoleId",
                table: "RoleClaims",
                column: "AppIdentityRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_AppIdentityRoleId",
                table: "RoleClaims",
                column: "AppIdentityRoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_AppIdentityRoleId",
                table: "RoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_AppIdentityRoleId",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "AppIdentityRoleId",
                table: "RoleClaims");
        }
    }
}
