using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_Job_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedByID",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ClosedByID",
                table: "Jobs",
                column: "ClosedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Candidates_ClosedByID",
                table: "Jobs",
                column: "ClosedByID",
                principalTable: "Candidates",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Candidates_ClosedByID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ClosedByID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ClosedByID",
                table: "Jobs");
        }
    }
}
