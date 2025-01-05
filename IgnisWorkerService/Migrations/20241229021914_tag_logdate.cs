using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgnisWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class tag_logdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tagvalues_tags_tagid",
                table: "tagvalues");

            migrationBuilder.DropIndex(
                name: "ix_tagvalues_tagid",
                table: "tagvalues");

            migrationBuilder.AddColumn<DateTime>(
                name: "logdate",
                table: "tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logdate",
                table: "tags");

            migrationBuilder.CreateIndex(
                name: "ix_tagvalues_tagid",
                table: "tagvalues",
                column: "tagid");

            migrationBuilder.AddForeignKey(
                name: "fk_tagvalues_tags_tagid",
                table: "tagvalues",
                column: "tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
