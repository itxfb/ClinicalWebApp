using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StudyIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Study",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolTitle = table.Column<string>(name: "Protocol_Title", type: "nvarchar(max)", nullable: false),
                    ConditionOrDisease = table.Column<string>(name: "Condition_Or_Disease", type: "nvarchar(max)", nullable: false),
                    Interventiontreatment = table.Column<string>(name: "Intervention_treatment", type: "nvarchar(max)", nullable: false),
                    Phase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyType = table.Column<string>(name: "Study_Type", type: "nvarchar(max)", nullable: false),
                    Enrollment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Allocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterventionModel = table.Column<string>(name: "Intervention_Model", type: "nvarchar(max)", nullable: false),
                    Masking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryPurpose = table.Column<string>(name: "Primary_Purpose", type: "nvarchar(max)", nullable: false),
                    ActualStudyStartDate = table.Column<DateTime>(name: "Actual_Study_Start_Date", type: "datetime2", nullable: false),
                    EstimatedPrimaryCompletionDate = table.Column<DateTime>(name: "Estimated_Primary_Completion_Date", type: "datetime2", nullable: false),
                    EstimatedStudyCompletionDate = table.Column<DateTime>(name: "Estimated_Study_Completion_Date", type: "datetime2", nullable: false),
                    VisitFrequency = table.Column<string>(name: "Visit_Frequency", type: "nvarchar(max)", nullable: false),
                    NCTNo = table.Column<string>(name: "NCT_No", type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Study", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Study_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestigatorSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityInstitutionName = table.Column<string>(name: "Facility_Institution_Name", type: "nvarchar(max)", nullable: true),
                    SiteNo = table.Column<double>(name: "Site_No", type: "float", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicalTrialAgreements = table.Column<string>(name: "Clinical_Trial_Agreements", type: "nvarchar(max)", nullable: true),
                    QualificationDate = table.Column<DateTime>(name: "Qualification_Date", type: "datetime2", nullable: true),
                    RecruitmentTarget = table.Column<double>(name: "Recruitment_Target", type: "float", nullable: true),
                    MonitoringFrequency = table.Column<int>(name: "Monitoring_Frequency", type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    StudyId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigatorSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigatorSites_Study_StudyId",
                        column: x => x.StudyId,
                        principalTable: "Study",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolDeviations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryCRA = table.Column<string>(name: "Primary_CRA", type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<double>(type: "float", nullable: true),
                    SubjectStatus = table.Column<string>(name: "Subject_Status", type: "nvarchar(max)", nullable: true),
                    protocoltype = table.Column<string>(name: "protocol_type", type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionResolution = table.Column<string>(name: "Action_Resolution", type: "nvarchar(max)", nullable: true),
                    SubjectVisit = table.Column<string>(name: "Subject_Visit", type: "nvarchar(max)", nullable: true),
                    Significance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    SponsorNotified = table.Column<DateTime>(name: "Sponsor_Notified", type: "datetime2", nullable: true),
                    ReportabletoEthics = table.Column<int>(name: "Reportable_to_Ethics", type: "int", nullable: true),
                    EthicsNotified = table.Column<int>(name: "Ethics_Notified", type: "int", nullable: true),
                    ReportedbyInvestigatorSite = table.Column<int>(name: "Reported_by_Investigator_Site", type: "int", nullable: true),
                    InvestigatorSiteId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolDeviations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtocolDeviations_InvestigatorSites_InvestigatorSiteId",
                        column: x => x.InvestigatorSiteId,
                        principalTable: "InvestigatorSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProtocolDeviations_Study_StudyId",
                        column: x => x.StudyId,
                        principalTable: "Study",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StaffInvestigatorSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(name: "Full_Name", type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    InvestigatorSiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffInvestigatorSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffInvestigatorSites_InvestigatorSites_InvestigatorSiteId",
                        column: x => x.InvestigatorSiteId,
                        principalTable: "InvestigatorSites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestigatorSites_StudyId",
                table: "InvestigatorSites",
                column: "StudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CreatedBy",
                table: "Organizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolDeviations_InvestigatorSiteId",
                table: "ProtocolDeviations",
                column: "InvestigatorSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolDeviations_StudyId",
                table: "ProtocolDeviations",
                column: "StudyId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffInvestigatorSites_InvestigatorSiteId",
                table: "StaffInvestigatorSites",
                column: "InvestigatorSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Study_OrganizationId",
                table: "Study",
                column: "OrganizationId");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtocolDeviations");

            migrationBuilder.DropTable(
                name: "StaffInvestigatorSites");

            migrationBuilder.DropTable(
                name: "InvestigatorSites");

            migrationBuilder.DropTable(
                name: "Study");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
