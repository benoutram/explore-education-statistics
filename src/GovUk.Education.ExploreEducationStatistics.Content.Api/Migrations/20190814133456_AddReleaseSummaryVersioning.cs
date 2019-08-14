﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GovUk.Education.ExploreEducationStatistics.Content.Api.Migrations
{
    public partial class AddReleaseSummaryVersioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReleaseSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseSummary_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseSummaryVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseSummaryId = table.Column<Guid>(nullable: false),
                    ReleaseName = table.Column<string>(nullable: true),
                    Published = table.Column<DateTime>(nullable: true),
                    PublishScheduled = table.Column<DateTime>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: false),
                    TimePeriodCoverage = table.Column<int>(nullable: false),
                    NextReleaseDate = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseSummaryVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseSummaryVersions_ReleaseSummary_ReleaseSummaryId",
                        column: x => x.ReleaseSummaryId,
                        principalTable: "ReleaseSummary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseSummaryVersions_ReleaseTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ReleaseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseSummary_ReleaseId",
                table: "ReleaseSummary",
                column: "ReleaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseSummaryVersions_ReleaseSummaryId",
                table: "ReleaseSummaryVersions",
                column: "ReleaseSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseSummaryVersions_TypeId",
                table: "ReleaseSummaryVersions",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseSummaryVersions");

            migrationBuilder.DropTable(
                name: "ReleaseSummary");
        }
    }
}
