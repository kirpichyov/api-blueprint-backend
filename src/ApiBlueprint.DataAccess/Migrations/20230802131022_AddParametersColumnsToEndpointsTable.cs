using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBlueprint.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddParametersColumnsToEndpointsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestParametersJson",
                table: "Endpoints",
                type: "text",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "ResponseParametersJson",
                table: "Endpoints",
                type: "text",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestParametersJson",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "ResponseParametersJson",
                table: "Endpoints");
        }
    }
}
