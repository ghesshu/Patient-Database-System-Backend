using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxonPDS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMedicineSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Des",
                table: "Medicines",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "Medicines",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Medicines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Medicines",
                newName: "Des");
        }
    }
}
