using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldTripLog.Web.Migrations
{
    public partial class AddTripNavigationToStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "Stops",
                newName: "TripID");

            migrationBuilder.RenameIndex(
                name: "IX_Stops_TripId",
                table: "Stops",
                newName: "IX_Stops_TripID");

            migrationBuilder.AlterColumn<int>(
                name: "TripID",
                table: "Stops",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Trips_TripID",
                table: "Stops",
                column: "TripID",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Trips_TripID",
                table: "Stops");

            migrationBuilder.RenameColumn(
                name: "TripID",
                table: "Stops",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_Stops_TripID",
                table: "Stops",
                newName: "IX_Stops_TripId");

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
