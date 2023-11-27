using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoHubWeb.Migrations
{
    public partial class InitalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "depots",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    province = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    number = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    weight = table.Column<int>(type: "int", nullable: false),
                    toDepot = table.Column<int>(type: "int", nullable: false),
                    fromDepot = table.Column<int>(type: "int", nullable: false),
                    customerID = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    employeeNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__FD291E4050980A2F", x => x.number);
                    table.ForeignKey(
                        name: "FK__orders__customer__76969D2E",
                        column: x => x.customerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__orders__employee__778AC167",
                        column: x => x.employeeNum,
                        principalTable: "Employees",
                        principalColumn: "Number");
                    table.ForeignKey(
                        name: "FK__orders__fromDepo__75A278F5",
                        column: x => x.fromDepot,
                        principalTable: "depots",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__orders__toDepot__74AE54BC",
                        column: x => x.toDepot,
                        principalTable: "depots",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_customerID",
                table: "orders",
                column: "customerID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_employeeNum",
                table: "orders",
                column: "employeeNum");

            migrationBuilder.CreateIndex(
                name: "IX_orders_fromDepot",
                table: "orders",
                column: "fromDepot");

            migrationBuilder.CreateIndex(
                name: "IX_orders_toDepot",
                table: "orders",
                column: "toDepot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "depots");
        }
    }
}
