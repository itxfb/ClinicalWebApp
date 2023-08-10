using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Ethics_Notified_Date",
                table: "ProtocolDeviations",
                type: "datetime2",
                nullable: true);
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ethics_Notified_Date",
                table: "ProtocolDeviations");
        }
    }
}
