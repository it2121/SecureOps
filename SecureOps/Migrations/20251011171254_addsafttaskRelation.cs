using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureOps.Migrations
{
    /// <inheritdoc />
    public partial class addsafttaskRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyTasks_Employees_EmployeeId",
                table: "SafetyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyTasks_Employees_EmployeeId",
                table: "SafetyTasks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyTasks_Employees_EmployeeId",
                table: "SafetyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyTasks_Employees_EmployeeId",
                table: "SafetyTasks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
