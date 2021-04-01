using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scarif.Web.Server.Migrations
{
    public partial class ChangeToSqlServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    AppId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.AppId);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "AppId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => new { x.LogId, x.Key });
                    table.ForeignKey(
                        name: "FK_Property_Log_LogId",
                        column: x => x.LogId,
                        principalTable: "Log",
                        principalColumn: "LogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_AppId",
                table: "Log",
                column: "AppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Apps");
        }
    }
}
