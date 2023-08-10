using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionDetails = table.Column<string>(name: "Action_Details", type: "nvarchar(max)", nullable: false),
                    MeetingDate = table.Column<DateTime>(name: "Meeting_Date", type: "datetime2", nullable: true),
                    TargetedCloseDate = table.Column<DateTime>(name: "Targeted_Close_Date", type: "datetime2", nullable: true),
                    ActualCloseDate = table.Column<DateTime>(name: "Actual_Close_Date", type: "datetime2", nullable: true),
                    Study = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionsDate = table.Column<DateTime>(name: "Decisions_Date", type: "datetime2", nullable: true),
                    Study = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskImpact = table.Column<string>(name: "Risk_Impact", type: "nvarchar(max)", nullable: false),
                    RiskDescription = table.Column<string>(name: "Risk_Description", type: "nvarchar(max)", nullable: false),
                    StudyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.Id);
                });

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Decisions");
        }
    }
}
