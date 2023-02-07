using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelIn.Migrations
{
    public partial class add_email_field_to_customers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cusEmail",
                table: "customers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cusEmail",
                table: "customers");
        }
    }
}
