using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBlueprint.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MergeJsonColumnsForEndpointsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseParametersJson",
                table: "Endpoints",
                newName: "ResponseContractJson");

            migrationBuilder.RenameColumn(
                name: "RequestParametersJson",
                table: "Endpoints",
                newName: "RequestContractJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseContractJson",
                table: "Endpoints",
                newName: "ResponseParametersJson");

            migrationBuilder.RenameColumn(
                name: "RequestContractJson",
                table: "Endpoints",
                newName: "RequestParametersJson");
        }
    }
}
