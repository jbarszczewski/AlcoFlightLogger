using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlcoFlightLogger.Migrations
{
    public partial class FlightModelUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightEntries_AspNetUsers_UserId",
                table: "FlightEntries");

            migrationBuilder.DropIndex(
                name: "IX_FlightEntries_UserId",
                table: "FlightEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlightEntries");
            
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "FlightEntries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FlightEntries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "FlightEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightEntries_ApplicationUserId",
                table: "FlightEntries",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightEntries_AspNetUsers_ApplicationUserId",
                table: "FlightEntries",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightEntries_AspNetUsers_ApplicationUserId",
                table: "FlightEntries");

            migrationBuilder.DropIndex(
                name: "IX_FlightEntries_ApplicationUserId",
                table: "FlightEntries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "FlightEntries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FlightEntries");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "FlightEntries");

            migrationBuilder.CreateTable(
                name: "AdditionalData",
                columns: table => new
                {
                    AdditionalDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataValue = table.Column<string>(nullable: true),
                    FlightEntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalData", x => x.AdditionalDataId);
                    table.ForeignKey(
                        name: "FK_AdditionalData_FlightEntries_FlightEntryId",
                        column: x => x.FlightEntryId,
                        principalTable: "FlightEntries",
                        principalColumn: "FlightEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FlightEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightEntries_UserId",
                table: "FlightEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalData_FlightEntryId",
                table: "AdditionalData",
                column: "FlightEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightEntries_AspNetUsers_UserId",
                table: "FlightEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
