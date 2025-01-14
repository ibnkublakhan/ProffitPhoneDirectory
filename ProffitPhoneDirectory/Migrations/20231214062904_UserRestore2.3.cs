using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProffitPhoneDirectory.Migrations
{
    /// <inheritdoc />
    public partial class UserRestore23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneOuter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneOuter", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneOuter_Id",
                table: "PhoneOuter",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneOuter");
        }
    }
}
