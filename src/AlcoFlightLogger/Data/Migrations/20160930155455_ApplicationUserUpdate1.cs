using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlcoFlightLogger.Migrations
{
    public partial class ApplicationUserUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
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
    }
}
