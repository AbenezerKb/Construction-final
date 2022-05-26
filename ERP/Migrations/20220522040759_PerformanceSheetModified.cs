using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class PerformanceSheetModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceSheets_Tasks_ProjectTaskId",
                table: "PerformanceSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceSheets_WeeklyResultValues_WeeklyResultValueId",
                table: "PerformanceSheets");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceSheets_ProjectTaskId",
                table: "PerformanceSheets");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceSheets_WeeklyResultValueId",
                table: "PerformanceSheets");

            migrationBuilder.DropColumn(
                name: "Staus",
                table: "WeeklyResults");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "PerformanceSheets");

            migrationBuilder.RenameColumn(
                name: "WeeklyResultValueId",
                table: "PerformanceSheets",
                newName: "ProjectId");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "CanApproveBorrow", "CanApproveBulkPurchase", "CanApproveBuy", "CanApproveIssue", "CanApproveMaintenance", "CanApprovePurchase", "CanApproveReceive", "CanApproveTransfer", "CanCheckBuy", "CanCheckPurchase", "CanConfirmBulkPurchase", "CanConfirmBuy", "CanConfirmPurchase", "CanEditUser", "CanFixMaintenance", "CanGetStockNotification", "CanHandBorrow", "CanHandIssue", "CanReceive", "CanReceiveTransfer", "CanRequestBorrow", "CanRequestBulkPurchase", "CanRequestBuy", "CanRequestIssue", "CanRequestMaintenance", "CanRequestPurchase", "CanRequestTransfer", "CanReturnBorrow", "CanSendTransfer", "CanViewBorrow", "CanViewBulkPurchase", "CanViewBuy", "CanViewIssue", "CanViewMaintenance", "CanViewPurchase", "CanViewReceive", "CanViewTransfer", "IsAdmin", "IsFinance", "Role" },
                values: new object[] { 1, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSheets_ProjectId",
                table: "PerformanceSheets",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceSheets_Projects_ProjectId",
                table: "PerformanceSheets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceSheets_Projects_ProjectId",
                table: "PerformanceSheets");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceSheets_ProjectId",
                table: "PerformanceSheets");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "PerformanceSheets",
                newName: "WeeklyResultValueId");

            migrationBuilder.AddColumn<string>(
                name: "Staus",
                table: "WeeklyResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "PerformanceSheets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSheets_ProjectTaskId",
                table: "PerformanceSheets",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSheets_WeeklyResultValueId",
                table: "PerformanceSheets",
                column: "WeeklyResultValueId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceSheets_Tasks_ProjectTaskId",
                table: "PerformanceSheets",
                column: "ProjectTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceSheets_WeeklyResultValues_WeeklyResultValueId",
                table: "PerformanceSheets",
                column: "WeeklyResultValueId",
                principalTable: "WeeklyResultValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
