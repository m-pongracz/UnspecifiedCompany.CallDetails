using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Giacom.CallDetails.Persistence.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCallerDateTypeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CallDetails_CallerId_CallDate_CallType",
                table: "CallDetails",
                columns: new[] { "CallerId", "CallDate", "CallType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CallDetails_CallerId_CallDate_CallType",
                table: "CallDetails");
        }
    }
}
