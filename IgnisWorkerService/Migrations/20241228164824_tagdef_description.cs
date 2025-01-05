using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgnisWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class tagdef_description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "tagsdefinitions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "indate",
                table: "tagsdefinitions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "tagsdefinitions");

            migrationBuilder.DropColumn(
                name: "indate",
                table: "tagsdefinitions");
        }
    }
}
