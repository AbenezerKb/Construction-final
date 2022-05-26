using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetDamages",
                columns: table => new
                {
                    AssetDamageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenalityPercentage = table.Column<decimal>(type: "decimal(2,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetDamages", x => x.AssetDamageId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCategories",
                columns: table => new
                {
                    EquipmentCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategories", x => x.EquipmentCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Miscellaneouses",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miscellaneouses", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PettyCashLimit = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsFinance = table.Column<bool>(type: "bit", nullable: false),
                    CanEditUser = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanApprovePurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanCheckPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanViewPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanConfirmPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanViewBulkPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestBulkPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveBulkPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanConfirmBulkPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestBuy = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveBuy = table.Column<bool>(type: "bit", nullable: false),
                    CanCheckBuy = table.Column<bool>(type: "bit", nullable: false),
                    CanViewBuy = table.Column<bool>(type: "bit", nullable: false),
                    CanConfirmBuy = table.Column<bool>(type: "bit", nullable: false),
                    CanReceive = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveReceive = table.Column<bool>(type: "bit", nullable: false),
                    CanViewReceive = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestIssue = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveIssue = table.Column<bool>(type: "bit", nullable: false),
                    CanHandIssue = table.Column<bool>(type: "bit", nullable: false),
                    CanViewIssue = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestBorrow = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveBorrow = table.Column<bool>(type: "bit", nullable: false),
                    CanHandBorrow = table.Column<bool>(type: "bit", nullable: false),
                    CanViewBorrow = table.Column<bool>(type: "bit", nullable: false),
                    CanReturnBorrow = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestTransfer = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveTransfer = table.Column<bool>(type: "bit", nullable: false),
                    CanSendTransfer = table.Column<bool>(type: "bit", nullable: false),
                    CanReceiveTransfer = table.Column<bool>(type: "bit", nullable: false),
                    CanViewTransfer = table.Column<bool>(type: "bit", nullable: false),
                    CanRequestMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    CanApproveMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    CanFixMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    CanViewMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    CanGetStockNotification = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "AssetNumberIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetNumberIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetNumberIds_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentCategories_EquipmentCategoryId",
                        column: x => x.EquipmentCategoryId,
                        principalTable: "EquipmentCategories",
                        principalColumn: "EquipmentCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Spec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTransferable = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Materials_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNo = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyPlans_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialSiteQties",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    MinimumQty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSiteQties", x => new { x.ItemId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_MaterialSiteQties_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSiteQties_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Stores_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeSiteId = table.Column<int>(type: "int", nullable: true),
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Sites_EmployeeSiteId",
                        column: x => x.EmployeeSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId");
                    table.ForeignKey(
                        name: "FK_Employees_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentModels",
                columns: table => new
                {
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentModels", x => x.EquipmentModelId);
                    table.ForeignKey(
                        name: "FK_EquipmentModels_Equipments_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Equipments",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    Progress = table.Column<double>(type: "float", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Staus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    WeeklyPlanId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyResults_WeeklyPlans_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "WeeklyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    BorrowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HandDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    HandedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.BorrowId);
                    table.ForeignKey(
                        name: "FK_Borrows_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrows_Employees_HandedById",
                        column: x => x.HandedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrows_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrows_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BulkPurchases",
                columns: table => new
                {
                    BulkPurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    TotalPurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkPurchases", x => x.BulkPurchaseId);
                    table.ForeignKey(
                        name: "FK_BulkPurchases_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BulkPurchases_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HandDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    HandedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_Issues_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Employees_HandedById",
                        column: x => x.HandedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    IsCleared = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Notifications_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    ReturnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedById = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.ReturnId);
                    table.ForeignKey(
                        name: "FK_Returns_Employees_ReturnedById",
                        column: x => x.ReturnedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Returns_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SendSiteId = table.Column<int>(type: "int", nullable: false),
                    ReceiveSiteId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    ReceivedById = table.Column<int>(type: "int", nullable: true),
                    DeliveredBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentById = table.Column<int>(type: "int", nullable: true),
                    VehiclePlateNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferId);
                    table.ForeignKey(
                        name: "FK_Transfers_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Employees_ReceivedById",
                        column: x => x.ReceivedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Employees_SentById",
                        column: x => x.SentById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Sites_ReceiveSiteId",
                        column: x => x.ReceiveSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Sites_SendSiteId",
                        column: x => x.SendSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Username);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentAssets",
                columns: table => new
                {
                    EquipmentAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    AssetNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentSiteId = table.Column<int>(type: "int", nullable: true),
                    AssetDamageId = table.Column<int>(type: "int", nullable: true),
                    CurrentEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentAssets", x => x.EquipmentAssetId);
                    table.ForeignKey(
                        name: "FK_EquipmentAssets_AssetDamages_AssetDamageId",
                        column: x => x.AssetDamageId,
                        principalTable: "AssetDamages",
                        principalColumn: "AssetDamageId");
                    table.ForeignKey(
                        name: "FK_EquipmentAssets_Employees_CurrentEmployeeId",
                        column: x => x.CurrentEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_EquipmentAssets_EquipmentModels_EquipmentModelId",
                        column: x => x.EquipmentModelId,
                        principalTable: "EquipmentModels",
                        principalColumn: "EquipmentModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentAssets_Sites_CurrentSiteId",
                        column: x => x.CurrentSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentSiteQties",
                columns: table => new
                {
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    MinimumQty = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSiteQties", x => new { x.EquipmentModelId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_EquipmentSiteQties_EquipmentModels_EquipmentModelId",
                        column: x => x.EquipmentModelId,
                        principalTable: "EquipmentModels",
                        principalColumn: "EquipmentModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentSiteQties_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_EquipmentSiteQties_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyPlanValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformedBy = table.Column<int>(type: "int", nullable: false),
                    SubTaskId = table.Column<int>(type: "int", nullable: true),
                    WeeklyPlanId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPlanValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyPlanValues_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeeklyPlanValues_WeeklyPlans_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "WeeklyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyResultValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    WeeklyResultId = table.Column<int>(type: "int", nullable: false),
                    SubTaskId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyResultValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyResultValues_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeeklyResultValues_WeeklyResults_WeeklyResultId",
                        column: x => x.WeeklyResultId,
                        principalTable: "WeeklyResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowItems",
                columns: table => new
                {
                    BorrowId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowItems", x => new { x.BorrowId, x.ItemId, x.EquipmentModelId });
                    table.ForeignKey(
                        name: "FK_BorrowItems_Borrows_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "Borrows",
                        principalColumn: "BorrowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BulkPurchaseItems",
                columns: table => new
                {
                    BulkPurchaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: false),
                    QtyPurchased = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkPurchaseItems", x => new { x.BulkPurchaseId, x.ItemId, x.EquipmentModelId });
                    table.ForeignKey(
                        name: "FK_BulkPurchaseItems_BulkPurchases_BulkPurchaseId",
                        column: x => x.BulkPurchaseId,
                        principalTable: "BulkPurchases",
                        principalColumn: "BulkPurchaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BulkPurchaseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedById = table.Column<int>(type: "int", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivingSiteId = table.Column<int>(type: "int", nullable: false),
                    TotalPurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BulkPurchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_BulkPurchases_BulkPurchaseId",
                        column: x => x.BulkPurchaseId,
                        principalTable: "BulkPurchases",
                        principalColumn: "BulkPurchaseId");
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_CheckedById",
                        column: x => x.CheckedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Sites_ReceivingSiteId",
                        column: x => x.ReceivingSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueItems",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueItems", x => new { x.IssueId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_IssueItems_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferItems",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferItems", x => new { x.TransferId, x.ItemId, x.EquipmentModelId });
                    table.ForeignKey(
                        name: "FK_TransferItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferItems_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "TransferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FixDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    EquipmentAssetId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    FixedById = table.Column<int>(type: "int", nullable: true),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FixRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_Maintenances_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Employees_FixedById",
                        column: x => x.FixedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_EquipmentAssets_EquipmentAssetId",
                        column: x => x.EquipmentAssetId,
                        principalTable: "EquipmentAssets",
                        principalColumn: "EquipmentAssetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_EquipmentModels_EquipmentModelId",
                        column: x => x.EquipmentModelId,
                        principalTable: "EquipmentModels",
                        principalColumn: "EquipmentModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Maintenances_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PerformancePoint = table.Column<float>(type: "real", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeeklyResultValueId = table.Column<int>(type: "int", nullable: false),
                    ProjectTaskId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceSheets_Tasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PerformanceSheets_WeeklyResultValues_WeeklyResultValueId",
                        column: x => x.WeeklyResultValueId,
                        principalTable: "WeeklyResultValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowItemEquipmentAssets",
                columns: table => new
                {
                    BorrowId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    EquipmentAssetId = table.Column<int>(type: "int", nullable: false),
                    AssetDamageId = table.Column<int>(type: "int", nullable: true),
                    ReturnRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowItemEquipmentAssets", x => new { x.BorrowId, x.ItemId, x.EquipmentModelId, x.EquipmentAssetId });
                    table.ForeignKey(
                        name: "FK_BorrowItemEquipmentAssets_AssetDamages_AssetDamageId",
                        column: x => x.AssetDamageId,
                        principalTable: "AssetDamages",
                        principalColumn: "AssetDamageId");
                    table.ForeignKey(
                        name: "FK_BorrowItemEquipmentAssets_BorrowItems_BorrowId_ItemId_EquipmentModelId",
                        columns: x => new { x.BorrowId, x.ItemId, x.EquipmentModelId },
                        principalTable: "BorrowItems",
                        principalColumns: new[] { "BorrowId", "ItemId", "EquipmentModelId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowItemEquipmentAssets_Borrows_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "Borrows",
                        principalColumn: "BorrowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowItemEquipmentAssets_EquipmentAssets_EquipmentAssetId",
                        column: x => x.EquipmentAssetId,
                        principalTable: "EquipmentAssets",
                        principalColumn: "EquipmentAssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowItemEquipmentAssets_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "Returns",
                        principalColumn: "ReturnId");
                });

            migrationBuilder.CreateTable(
                name: "Buys",
                columns: table => new
                {
                    BuyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    BuySiteId = table.Column<int>(type: "int", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedById = table.Column<int>(type: "int", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    BuyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    TotalBuyCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buys", x => x.BuyId);
                    table.ForeignKey(
                        name: "FK_Buys_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Employees_CheckedById",
                        column: x => x.CheckedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Sites_BuySiteId",
                        column: x => x.BuySiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItemEmployees",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItemEmployees", x => new { x.PurchaseId, x.ItemId, x.EquipmentModelId, x.RequestedById });
                    table.ForeignKey(
                        name: "FK_PurchaseItemEmployees_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseItemEmployees_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseItemEmployees_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: false),
                    QtyPurchased = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItems", x => new { x.PurchaseId, x.ItemId, x.EquipmentModelId });
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receives",
                columns: table => new
                {
                    ReceiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    DeliveredById = table.Column<int>(type: "int", nullable: true),
                    ReceivedById = table.Column<int>(type: "int", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivingSiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receives", x => x.ReceiveId);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_DeliveredById",
                        column: x => x.DeliveredById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_ReceivedById",
                        column: x => x.ReceivedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receives_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receives_Sites_ReceivingSiteId",
                        column: x => x.ReceivingSiteId,
                        principalTable: "Sites",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferItemEquipmentAssets",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    EquipmentAssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferItemEquipmentAssets", x => new { x.TransferId, x.ItemId, x.EquipmentModelId, x.EquipmentAssetId });
                    table.ForeignKey(
                        name: "FK_TransferItemEquipmentAssets_EquipmentAssets_EquipmentAssetId",
                        column: x => x.EquipmentAssetId,
                        principalTable: "EquipmentAssets",
                        principalColumn: "EquipmentAssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferItemEquipmentAssets_TransferItems_TransferId_ItemId_EquipmentModelId",
                        columns: x => new { x.TransferId, x.ItemId, x.EquipmentModelId },
                        principalTable: "TransferItems",
                        principalColumns: new[] { "TransferId", "ItemId", "EquipmentModelId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyItems",
                columns: table => new
                {
                    BuyId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: false),
                    QtyBought = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyItems", x => new { x.BuyId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BuyItems_Buys_BuyId",
                        column: x => x.BuyId,
                        principalTable: "Buys",
                        principalColumn: "BuyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveItems",
                columns: table => new
                {
                    ReceiveId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QtyReceived = table.Column<int>(type: "int", nullable: false),
                    ReceiveRemark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveItems", x => new { x.ReceiveId, x.ItemId, x.EquipmentModelId });
                    table.ForeignKey(
                        name: "FK_ReceiveItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveItems_Receives_ReceiveId",
                        column: x => x.ReceiveId,
                        principalTable: "Receives",
                        principalColumn: "ReceiveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[] { 1, "Before how many days should a deadline notification be sent", "DeadlineNotificationDay", "10" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetNumberIds_ItemId",
                table: "AssetNumberIds",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowItemEquipmentAssets_AssetDamageId",
                table: "BorrowItemEquipmentAssets",
                column: "AssetDamageId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowItemEquipmentAssets_EquipmentAssetId",
                table: "BorrowItemEquipmentAssets",
                column: "EquipmentAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowItemEquipmentAssets_ReturnId",
                table: "BorrowItemEquipmentAssets",
                column: "ReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowItems_ItemId",
                table: "BorrowItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_ApprovedById",
                table: "Borrows",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_HandedById",
                table: "Borrows",
                column: "HandedById");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_RequestedById",
                table: "Borrows",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_SiteId",
                table: "Borrows",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkPurchaseItems_ItemId",
                table: "BulkPurchaseItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkPurchases_ApprovedById",
                table: "BulkPurchases",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_BulkPurchases_RequestedById",
                table: "BulkPurchases",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuyItems_ItemId",
                table: "BuyItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_ApprovedById",
                table: "Buys",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_BuySiteId",
                table: "Buys",
                column: "BuySiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_CheckedById",
                table: "Buys",
                column: "CheckedById");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_PurchaseId",
                table: "Buys",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_RequestedById",
                table: "Buys",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeSiteId",
                table: "Employees",
                column: "EmployeeSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserRoleId",
                table: "Employees",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssets_AssetDamageId",
                table: "EquipmentAssets",
                column: "AssetDamageId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssets_CurrentEmployeeId",
                table: "EquipmentAssets",
                column: "CurrentEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssets_CurrentSiteId",
                table: "EquipmentAssets",
                column: "CurrentSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssets_EquipmentModelId",
                table: "EquipmentAssets",
                column: "EquipmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentModels_ItemId",
                table: "EquipmentModels",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentCategoryId",
                table: "Equipments",
                column: "EquipmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSiteQties_ItemId",
                table: "EquipmentSiteQties",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSiteQties_SiteId",
                table: "EquipmentSiteQties",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueItems_ItemId",
                table: "IssueItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ApprovedById",
                table: "Issues",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_HandedById",
                table: "Issues",
                column: "HandedById");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_RequestedById",
                table: "Issues",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_SiteId",
                table: "Issues",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_ApprovedById",
                table: "Maintenances",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_EquipmentAssetId",
                table: "Maintenances",
                column: "EquipmentAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_EquipmentModelId",
                table: "Maintenances",
                column: "EquipmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_FixedById",
                table: "Maintenances",
                column: "FixedById");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_ItemId",
                table: "Maintenances",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_RequestedById",
                table: "Maintenances",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_SiteId",
                table: "Maintenances",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSiteQties_SiteId",
                table: "MaterialSiteQties",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SiteId",
                table: "Notifications",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSheets_ProjectTaskId",
                table: "PerformanceSheets",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceSheets_WeeklyResultValueId",
                table: "PerformanceSheets",
                column: "WeeklyResultValueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItemEmployees_ItemId",
                table: "PurchaseItemEmployees",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItemEmployees_RequestedById",
                table: "PurchaseItemEmployees",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_ItemId",
                table: "PurchaseItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ApprovedById",
                table: "Purchases",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BulkPurchaseId",
                table: "Purchases",
                column: "BulkPurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CheckedById",
                table: "Purchases",
                column: "CheckedById");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ReceivingSiteId",
                table: "Purchases",
                column: "ReceivingSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_RequestedById",
                table: "Purchases",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveItems_ItemId",
                table: "ReceiveItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ApprovedById",
                table: "Receives",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_DeliveredById",
                table: "Receives",
                column: "DeliveredById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_PurchaseId",
                table: "Receives",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ReceivedById",
                table: "Receives",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ReceivingSiteId",
                table: "Receives",
                column: "ReceivingSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnedById",
                table: "Returns",
                column: "ReturnedById");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_SiteId",
                table: "Returns",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SiteId",
                table: "Stores",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferItemEquipmentAssets_EquipmentAssetId",
                table: "TransferItemEquipmentAssets",
                column: "EquipmentAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferItems_ItemId",
                table: "TransferItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ApprovedById",
                table: "Transfers",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ReceivedById",
                table: "Transfers",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ReceiveSiteId",
                table: "Transfers",
                column: "ReceiveSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RequestedById",
                table: "Transfers",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SendSiteId",
                table: "Transfers",
                column: "SendSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SentById",
                table: "Transfers",
                column: "SentById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_EmployeeId",
                table: "UserAccounts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPlans_ProjectId",
                table: "WeeklyPlans",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPlanValues_SubTaskId",
                table: "WeeklyPlanValues",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPlanValues_WeeklyPlanId",
                table: "WeeklyPlanValues",
                column: "WeeklyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyResults_WeeklyPlanId",
                table: "WeeklyResults",
                column: "WeeklyPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyResultValues_SubTaskId",
                table: "WeeklyResultValues",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyResultValues_WeeklyResultId",
                table: "WeeklyResultValues",
                column: "WeeklyResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetNumberIds");

            migrationBuilder.DropTable(
                name: "BorrowItemEquipmentAssets");

            migrationBuilder.DropTable(
                name: "BulkPurchaseItems");

            migrationBuilder.DropTable(
                name: "BuyItems");

            migrationBuilder.DropTable(
                name: "EquipmentSiteQties");

            migrationBuilder.DropTable(
                name: "IssueItems");

            migrationBuilder.DropTable(
                name: "Maintenances");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialSiteQties");

            migrationBuilder.DropTable(
                name: "Miscellaneouses");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PerformanceSheets");

            migrationBuilder.DropTable(
                name: "PurchaseItemEmployees");

            migrationBuilder.DropTable(
                name: "PurchaseItems");

            migrationBuilder.DropTable(
                name: "ReceiveItems");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "TransferItemEquipmentAssets");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "WeeklyPlanValues");

            migrationBuilder.DropTable(
                name: "BorrowItems");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "Buys");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "WeeklyResultValues");

            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropTable(
                name: "EquipmentAssets");

            migrationBuilder.DropTable(
                name: "TransferItems");

            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "WeeklyResults");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "AssetDamages");

            migrationBuilder.DropTable(
                name: "EquipmentModels");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "WeeklyPlans");

            migrationBuilder.DropTable(
                name: "BulkPurchases");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EquipmentCategories");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
