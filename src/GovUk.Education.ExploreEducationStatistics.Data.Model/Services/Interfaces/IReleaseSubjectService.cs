using System;
using System.Threading.Tasks;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Services.Interfaces
{
    public interface IReleaseSubjectService
    {
        Task SoftDeleteAllReleaseSubjects(Guid releaseId);

        Task SoftDeleteReleaseSubject(Guid releaseId, Guid subjectId);

        Task DeleteAllReleaseSubjects(Guid releaseId, bool softDeleteOrphanedSubjects = false);

        Task DeleteReleaseSubject(Guid releaseId, Guid subjectId, bool softDeleteOrphanedSubject = false);
    }
}