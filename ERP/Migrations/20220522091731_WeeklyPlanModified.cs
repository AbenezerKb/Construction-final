using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class WeeklyPlanModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "WeeklyPlans");

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekStartDate",
                table: "WeeklyPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekStartDate",
                table: "WeeklyPlans");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "WeeklyPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
