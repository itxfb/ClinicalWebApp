using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FindingStatus",
                table: "GeneralFindings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewComments",
                table: "GeneralFindings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewDate",
                table: "GeneralFindings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FindingStatus",
                table: "GeneralFindings");

            migrationBuilder.DropColumn(
                name: "ReviewComments",
                table: "GeneralFindings");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "GeneralFindings");
        }
    }
}
