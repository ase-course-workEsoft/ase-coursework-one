using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelIn.Migrations
{
    public partial class create_other_superAdmin_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stations",
                columns: table => new
                {
                    staID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    staDistrict = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    totalFualQuota = table.Column<int>(type: "int", nullable: false),
                    avaFualQuota = table.Column<int>(type: "int", nullable: false),
                    nextFillingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stations", x => x.staID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicleTypes",
                columns: table => new
                {
                    vehTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    weeklyQuota = table.Column<int>(type: "int", nullable: false),
                    vehType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicleTypes", x => x.vehTypeID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fualDistributions",
                columns: table => new
                {
                    disID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    staID = table.Column<int>(type: "int", nullable: false),
                    stationstaID = table.Column<int>(type: "int", nullable: false),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    USER_ID1 = table.Column<int>(type: "int", nullable: false),
                    distributionStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    expectedEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isDistributionStatus = table.Column<int>(type: "int", nullable: false),
                    arrivalHours = table.Column<int>(type: "int", nullable: false),
                    accepted = table.Column<int>(type: "int", nullable: false),
                    disLocation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fualDistributions", x => x.disID);
                    table.ForeignKey(
                        name: "FK_fualDistributions_stations_stationstaID",
                        column: x => x.stationstaID,
                        principalTable: "stations",
                        principalColumn: "staID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fualDistributions_USER_USER_ID1",
                        column: x => x.USER_ID1,
                        principalTable: "USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    cusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    staID = table.Column<int>(type: "int", nullable: false),
                    stationstaID = table.Column<int>(type: "int", nullable: false),
                    vehTypeID = table.Column<int>(type: "int", nullable: false),
                    vehicleTypevehTypeID = table.Column<int>(type: "int", nullable: false),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    USER_ID1 = table.Column<int>(type: "int", nullable: false),
                    cusName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cusNIC = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicleRegNum = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avaWeeklyQuota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.cusID);
                    table.ForeignKey(
                        name: "FK_customers_stations_stationstaID",
                        column: x => x.stationstaID,
                        principalTable: "stations",
                        principalColumn: "staID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customers_USER_USER_ID1",
                        column: x => x.USER_ID1,
                        principalTable: "USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customers_vehicleTypes_vehicleTypevehTypeID",
                        column: x => x.vehicleTypevehTypeID,
                        principalTable: "vehicleTypes",
                        principalColumn: "vehTypeID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_customers_stationstaID",
                table: "customers",
                column: "stationstaID");

            migrationBuilder.CreateIndex(
                name: "IX_customers_USER_ID1",
                table: "customers",
                column: "USER_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_customers_vehicleTypevehTypeID",
                table: "customers",
                column: "vehicleTypevehTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_fualDistributions_stationstaID",
                table: "fualDistributions",
                column: "stationstaID");

            migrationBuilder.CreateIndex(
                name: "IX_fualDistributions_USER_ID1",
                table: "fualDistributions",
                column: "USER_ID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "fualDistributions");

            migrationBuilder.DropTable(
                name: "vehicleTypes");

            migrationBuilder.DropTable(
                name: "stations");
        }
    }
}
