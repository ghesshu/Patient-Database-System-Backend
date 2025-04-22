using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxonPDS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecord_Records_RecordId",
                table: "TreatmentRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecord_Treatments_TreatmentId",
                table: "TreatmentRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreatmentRecord",
                table: "TreatmentRecord");

            migrationBuilder.RenameTable(
                name: "TreatmentRecord",
                newName: "TreatmentRecords");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentRecord_TreatmentId",
                table: "TreatmentRecords",
                newName: "IX_TreatmentRecords_TreatmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TreatmentRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreatmentRecords",
                table: "TreatmentRecords",
                columns: new[] { "RecordId", "TreatmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecords_Records_RecordId",
                table: "TreatmentRecords",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecords_Treatments_TreatmentId",
                table: "TreatmentRecords",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecords_Records_RecordId",
                table: "TreatmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecords_Treatments_TreatmentId",
                table: "TreatmentRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreatmentRecords",
                table: "TreatmentRecords");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TreatmentRecords");

            migrationBuilder.RenameTable(
                name: "TreatmentRecords",
                newName: "TreatmentRecord");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentRecords_TreatmentId",
                table: "TreatmentRecord",
                newName: "IX_TreatmentRecord_TreatmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreatmentRecord",
                table: "TreatmentRecord",
                columns: new[] { "RecordId", "TreatmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecord_Records_RecordId",
                table: "TreatmentRecord",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecord_Treatments_TreatmentId",
                table: "TreatmentRecord",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
