using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Giacom.CallDetails.Persistence.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCallDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallDetails",
                columns: table => new
                {
                    CallDetailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CallerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CallDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(3,2)", precision: 3, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CallType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallDetails", x => x.CallDetailId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallDetails");
        }
    }
}
