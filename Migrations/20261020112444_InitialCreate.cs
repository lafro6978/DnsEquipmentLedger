using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DnsEquipmentLedger.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TabNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ZoneType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoomNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SupportContact = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentItems",
                columns: table => new
                {
                    EquipmentItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponsibleEmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    InventoryNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SerialNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentItems", x => x.EquipmentItemId);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_Employees_ResponsibleEmployeeId",
                        column: x => x.ResponsibleEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "ManufacturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMovements",
                columns: table => new
                {
                    EquipmentMovementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromLocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    ToLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OperationType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMovements", x => x.EquipmentMovementId);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_EquipmentItems_EquipmentItemId",
                        column: x => x.EquipmentItemId,
                        principalTable: "EquipmentItems",
                        principalColumn: "EquipmentItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Locations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Locations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "ТСД, сканеры штрих-кодов, онлайн-кассы", "Торговое и кассовое оборудование" },
                    { 2, "Рабочие станции продавцов-консультантов и управляющего", "Компьютеры и ноутбуки" },
                    { 3, "Маршрутизаторы, коммутаторы, точки доступа Wi-Fi", "Сетевое и серверное оборудование" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "FullName", "Phone", "Position", "TabNumber" },
                values: new object[,]
                {
                    { 1, "Смирнов Алексей Викторович", "+7(913)555-12-34", "Управляющий магазином", "DNS-00142" },
                    { 2, "Козлов Дмитрий Игоревич", "+7(913)555-56-78", "Старший кладовщик", "DNS-00289" },
                    { 3, "Орлова Екатерина Сергеевна", "+7(913)555-90-12", "Специалист сервисного центра", "DNS-00415" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name", "RoomNumber", "ZoneType" },
                values: new object[,]
                {
                    { 1, "Основной склад филиала", "П-101", "Складская зона" },
                    { 2, "Торговый зал (Зона выдачи и касс)", "ТЗ-1", "Торговая зона" },
                    { 3, "Сервисный центр / Приёмка по гарантии", "СЦ-2", "Сервисная зона" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "ManufacturerId", "Name", "SupportContact" },
                values: new object[,]
                {
                    { 1, "DEXP", "support@dexp.club" },
                    { 2, "ARDOR GAMING", "service@ardor-gaming.com" },
                    { 3, "Honeywell", "8-800-100-20-30" },
                    { 4, "АТОЛ", "support@atol.ru" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentItems",
                columns: new[] { "EquipmentItemId", "CategoryId", "Cost", "InventoryNumber", "LocationId", "ManufacturerId", "ModelName", "PurchaseDate", "ResponsibleEmployeeId", "SerialNumber", "Status" },
                values: new object[,]
                {
                    { 1, 1, 42500.00m, "INV-DNS-2026-0001", 1, 3, "Терминал сбора данных Honeywell ScanPal EDA51", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "HW-EDA51-994812", "В эксплуатации" },
                    { 2, 2, 38990.00m, "INV-DNS-2026-0002", 2, 1, "Ноутбук DEXP Atlas H15 (Пост проверки товара)", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "DEXP-ATLAS-773120", "В эксплуатации" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentMovements",
                columns: new[] { "EquipmentMovementId", "Comment", "EmployeeId", "EquipmentItemId", "FromLocationId", "MovementDate", "OperationType", "ToLocationId" },
                values: new object[,]
                {
                    { 1, "Постановка ТСД на учёт на основной склад филиала", 2, 1, null, new DateTime(2026, 2, 10, 10, 30, 0, 0, DateTimeKind.Unspecified), "Первичная приёмка", 1 },
                    { 2, "Выдача ноутбука со склада в торговый зал на стойку проверки техники", 1, 2, 1, new DateTime(2026, 3, 16, 14, 15, 0, 0, DateTimeKind.Unspecified), "Перемещение", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TabNumber",
                table: "Employees",
                column: "TabNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_CategoryId",
                table: "EquipmentItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_InventoryNumber",
                table: "EquipmentItems",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_LocationId",
                table: "EquipmentItems",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_ManufacturerId",
                table: "EquipmentItems",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_ResponsibleEmployeeId",
                table: "EquipmentItems",
                column: "ResponsibleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_EmployeeId",
                table: "EquipmentMovements",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_EquipmentItemId",
                table: "EquipmentMovements",
                column: "EquipmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_FromLocationId",
                table: "EquipmentMovements",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_ToLocationId",
                table: "EquipmentMovements",
                column: "ToLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMovements");

            migrationBuilder.DropTable(
                name: "EquipmentItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
