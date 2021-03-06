using System.Data;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Models;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces
{
    public interface IImporterMetaService
    {
        SubjectMeta Import(DataColumnCollection cols, DataRowCollection rows, Subject subject, StatisticsDbContext context);

        SubjectMeta Get(DataColumnCollection cols, DataRowCollection rows, Subject subject, StatisticsDbContext context);
    }
}