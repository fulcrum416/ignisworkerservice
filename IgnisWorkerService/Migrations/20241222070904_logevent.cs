using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgnisWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class logevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "log_event",
                table: "applogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "log_event",
                table: "applogs");
        }
    }
}
