﻿using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;
using static GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations.MigrationConstants;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations
{
    public partial class EES1830_Routine_MigrateObservationsAndObservationFilterItems : Migration
    {
        private const string MigrationId = "20210126094900";
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_MigrateObservationsAndObservationFilterItems.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE MigrateObservationsAndObservationFilterItems");
        }
    }
}
