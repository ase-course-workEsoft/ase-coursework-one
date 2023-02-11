using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelIn.Migrations
{
    public partial class inittt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customerRequests",
                columns: table => new
                {
                    ReqId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StaId = table.Column<int>(type: "int", nullable: false),
                    cusID = table.Column<int>(type: "int", nullable: false),
                    IsIdUsed = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReqStatus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpectedFillingTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RequestedQuota = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerRequests", x => x.ReqId);
                    table.ForeignKey(
                        name: "FK_customerRequests_customers_cusID",
                        column: x => x.cusID,
                        principalTable: "customers",
                        principalColumn: "cusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customerRequests_stations_StaId",
                        column: x => x.StaId,
                        principalTable: "stations",
                        principalColumn: "staID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_customerRequests_cusID",
                table: "customerRequests",
                column: "cusID");

            migrationBuilder.CreateIndex(
                name: "IX_customerRequests_StaId",
                table: "customerRequests",
                column: "StaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerRequests");
        }
    }
}
