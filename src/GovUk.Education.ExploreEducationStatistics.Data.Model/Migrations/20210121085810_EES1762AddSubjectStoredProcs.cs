using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;
using static GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations.MigrationConstants;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Migrations
{
    public partial class EES1762AddSubjectStoredProcs : Migration
    {
        private const string MigrationId = "20210121085810";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_DataSource_Hdfs.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_FileFormat_CsvFile.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_DropExternalTable.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_DropAndCreateExternalMetaTable.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_DropAndCreateExternalStatsTable.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_AddFilterGroups.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_AddSubject.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_Routine_FilteredVirtualObservations.sql");
            migrationBuilder.SqlFromFile(MigrationsPath, $"{MigrationId}_View_FiltersAndIndicators.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.FiltersAndIndicators");
            migrationBuilder.Sql("DROP PROCEDURE dbo.FilteredVirtualObservations");
            migrationBuilder.Sql("DROP PROCEDURE dbo.AddSubject");
            migrationBuilder.Sql("DROP PROCEDURE dbo.AddFilterGroups");
            migrationBuilder.Sql("DROP PROCEDURE dbo.DropAndCreateExternalStatsTable");
            migrationBuilder.Sql("DROP PROCEDURE dbo.DropAndCreateExternalMetaTable");
            migrationBuilder.Sql("DROP PROCEDURE dbo.DropExternalTable");
            migrationBuilder.Sql("DROP EXTERNAL FILE FORMAT csv_file");
            migrationBuilder.Sql("DROP EXTERNAL DATA SOURCE MyHadoopDs");
        }
    }
}
