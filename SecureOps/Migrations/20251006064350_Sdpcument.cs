using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureOps.Migrations
{
    /// <inheritdoc />
    public partial class Sdpcument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_SDocuments_SDocumentId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocumentAcknowledgements_SDocumentId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropColumn(
                name: "SDocumentId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "SDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "SDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "SDocuments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfidential",
                table: "SDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "SDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UploadedById",
                table: "SDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SDocuments_UploadedById",
                table: "SDocuments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocumentAcknowledgements_DocumentId",
                table: "EmployeeDocumentAcknowledgements",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_SDocuments_DocumentId",
                table: "EmployeeDocumentAcknowledgements",
                column: "DocumentId",
                principalTable: "SDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SDocuments_Employees_UploadedById",
                table: "SDocuments",
                column: "UploadedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_SDocuments_DocumentId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropForeignKey(
                name: "FK_SDocuments_Employees_UploadedById",
                table: "SDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SDocuments_UploadedById",
                table: "SDocuments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocumentAcknowledgements_DocumentId",
                table: "EmployeeDocumentAcknowledgements");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "IsConfidential",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SDocuments");

            migrationBuilder.DropColumn(
                name: "UploadedById",
                table: "SDocuments");

            migrationBuilder.AddColumn<int>(
                name: "SDocumentId",
                table: "EmployeeDocumentAcknowledgements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocumentAcknowledgements_SDocumentId",
                table: "EmployeeDocumentAcknowledgements",
                column: "SDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocumentAcknowledgements_SDocuments_SDocumentId",
                table: "EmployeeDocumentAcknowledgements",
                column: "SDocumentId",
                principalTable: "SDocuments",
                principalColumn: "Id");
        }
    }
}
