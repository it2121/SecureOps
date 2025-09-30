using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureOps.Migrations
{
    /// <inheritdoc />
    public partial class removemanager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
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
    }
}
