using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldTripLog.Web.Migrations
{
    public partial class AddedBaseEntityForWholeProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Trips",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Stops",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Stops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Stops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Stops",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Stops");
        }
    }
}
