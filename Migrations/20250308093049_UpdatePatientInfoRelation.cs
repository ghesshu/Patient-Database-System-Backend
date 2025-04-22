using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxonPDS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePatientInfoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientInfoId",
                table: "Patients",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients",
                column: "PatientInfoId",
                principalTable: "PatientInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientInfoId",
                table: "Patients",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientInfos_PatientInfoId",
                table: "Patients",
                column: "PatientInfoId",
                principalTable: "PatientInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
