﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Ees183UpdateFilteredObservationsStoredProc : Migration
    {
        private const string MigrationsPath = "Migrations";
        private const string MigrationId = "20191125114340";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ExecuteFile(migrationBuilder, $"{MigrationId}_Routine_FilteredObservations.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string previousVersionMigrationId = "20191101153547";
            ExecuteFile(migrationBuilder, $"{previousVersionMigrationId}_Routine_FilteredObservations.sql");
        }

        private static void ExecuteFile(MigrationBuilder migrationBuilder, string filename)
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                $"{MigrationsPath}{Path.DirectorySeparatorChar}{filename}");

            migrationBuilder.Sql(File.ReadAllText(file));
        }
    }
}