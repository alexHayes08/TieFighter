﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TieFighter.Data.Migrations
{
    public partial class Migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            //migrationBuilder.AddColumn<double>(
            //    name: "DisplayLevel",
            //    table: "AspNetUsers",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<string>(
            //    name: "DisplayName",
            //    table: "AspNetUsers",
            //    unicode: false,
            //    maxLength: 120,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Thumbnail",
            //    table: "AspNetUsers",
            //    maxLength: 512,
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Medals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    MedalName = table.Column<string>(nullable: true),
                    PointsWorth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medals", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Ships",
            //    columns: table => new
            //    {
            //        ShipID = table.Column<int>(nullable: false),
            //        ShipFolder = table.Column<string>(unicode: false, maxLength: 120, nullable: true),
            //        ShipName = table.Column<string>(unicode: false, maxLength: 120, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Ships", x => x.ShipID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tours",
            //    columns: table => new
            //    {
            //        TourID = table.Column<int>(nullable: false),
            //        TourName = table.Column<string>(unicode: false, maxLength: 120, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tours", x => x.TourID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Missions",
            //    columns: table => new
            //    {
            //        MissionID = table.Column<int>(nullable: false),
            //        FK_Tour = table.Column<int>(nullable: true),
            //        MissionName = table.Column<string>(unicode: false, maxLength: 120, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Missions", x => x.MissionID);
            //        table.ForeignKey(
            //            name: "FK__Missions__FK_Tou__2C3393D0",
            //            column: x => x.FK_Tour,
            //            principalTable: "Tours",
            //            principalColumn: "TourID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Missions_FK_Tour",
            //    table: "Missions",
            //    column: "FK_Tour");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medals");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }
    }
}
