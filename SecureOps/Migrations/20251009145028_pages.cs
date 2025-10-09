using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureOps.Migrations
{
    /// <inheritdoc />
    public partial class pages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Page_PageId",
                table: "PageRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Page",
                table: "Page");

            migrationBuilder.RenameTable(
                name: "Page",
                newName: "Pages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pages",
                table: "Pages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_Pages_PageId",
                table: "PageRoles",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Pages_PageId",
                table: "PageRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pages",
                table: "Pages");

            migrationBuilder.RenameTable(
                name: "Pages",
                newName: "Page");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Page",
                table: "Page",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_Page_PageId",
                table: "PageRoles",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
