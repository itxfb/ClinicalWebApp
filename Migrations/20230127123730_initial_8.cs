using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "ProtocolDeviations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "InvestigatorSites",
                type: "int",
                nullable: true);
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ProtocolDeviations");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "InvestigatorSites");
        }
    }
}
