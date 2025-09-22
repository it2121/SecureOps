using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureOps.Migrations
{
    /// <inheritdoc />
    public partial class ManagerUserRoleManyToManySetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "SafetyTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Incidents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "EmployeeDocumentAcknowledgements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RolesRoleId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesRoleId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyTasks_ManagerId",
                table: "SafetyTasks",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ManagerId",
                table: "Incidents",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocumentAcknowledgements_ManagerId",
                table: "EmployeeDocumentAcknowledgements",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_Managers_ManagerId",
                table: "EmployeeDocumentAcknowledgements",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Managers_ManagerId",
                table: "Incidents",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyTasks_Managers_ManagerId",
                table: "SafetyTasks",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_Managers_ManagerId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Managers_ManagerId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyTasks_Managers_ManagerId",
                table: "SafetyTasks");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SafetyTasks_ManagerId",
                table: "SafetyTasks");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_ManagerId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocumentAcknowledgements_ManagerId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "SafetyTasks");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "EmployeeDocumentAcknowledgements");
        }
    }
}
