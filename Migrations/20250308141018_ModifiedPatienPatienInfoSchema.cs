using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxonPDS.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedPatienPatienInfoSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientInfoId",
                table: "Patients");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "PatientInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PatientInfos_PatientId",
                table: "PatientInfos",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientInfos_Patients_PatientId",
                table: "PatientInfos",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientInfos_Patients_PatientId",
                table: "PatientInfos");

            migrationBuilder.DropIndex(
                name: "IX_PatientInfos_PatientId",
                table: "PatientInfos");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PatientInfos");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientInfoId",
                table: "Patients",
                column: "PatientInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients",
                column: "PatientInfoId",
                principalTable: "PatientInfos",
                principalColumn: "Id");
        }
    }
}
