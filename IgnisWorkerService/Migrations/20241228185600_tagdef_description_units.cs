using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgnisWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class tagdef_description_units : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "systemtag",
                table: "tagsdefinitions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tagsdefinitions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "systemtag",
                table: "tagsdefinitions");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tagsdefinitions");
        }
    }
}
