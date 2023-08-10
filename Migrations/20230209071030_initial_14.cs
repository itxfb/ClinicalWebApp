using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvestiGatorSitefId",
                table: "GeneralFindings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "GeneralFindings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudyId",
                table: "GeneralFindings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestiGatorSitefId",
                table: "GeneralFindings");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "GeneralFindings");

            migrationBuilder.DropColumn(
                name: "StudyId",
                table: "GeneralFindings");
        }
    }
}
