using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Publisher.Model.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Publisher.Models;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using IReleaseService = GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces.IReleaseService;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Utils.PublisherUtils;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly ContentDbContext _contentDbContext;
        private readonly StatisticsDbContext _statisticsDbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IReleaseSubjectService _releaseSubjectService;
        private readonly IMapper _mapper;

        public ReleaseService(ContentDbContext contentDbContext,
            StatisticsDbContext statisticsDbContext,
            IFileStorageService fileStorageService,
            IReleaseSubjectService releaseSubjectService,
            IMapper mapper)
        {
            _contentDbContext = contentDbContext;
            _statisticsDbContext = statisticsDbContext;
            _fileStorageService = fileStorageService;
            _releaseSubjectService = releaseSubjectService;
            _mapper = mapper;
        }

        public async Task<Release> GetAsync(Guid id)
        {
            return await _contentDbContext.Releases
                .Include(release => release.Publication)
                .Include(r => r.PreviousVersion)
                .SingleOrDefaultAsync(release => release.Id == id);
        }

        public async Task<IEnumerable<Release>> GetAsync(IEnumerable<Guid> ids)
        {
            return await _contentDbContext.Releases
                .Where(release => ids.Contains(release.Id))
                .Include(release => release.Publication)
                .Include(release => release.PreviousVersion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Release>> GetAmendedReleases(IEnumerable<Guid> releaseIds)
        {
            return await _contentDbContext.Releases
                .Include(r => r.PreviousVersion)
                .Include(r => r.Publication)
                .Where(r => releaseIds.Contains(r.Id) && r.PreviousVersionId != null)
                .ToListAsync();
        }

        public CachedReleaseViewModel GetReleaseViewModel(Guid id, PublishContext context)
        {
            var release = _contentDbContext.Releases
                .Include(r => r.Type)
                .Include(r => r.Content)
                .ThenInclude(releaseContentSection => releaseContentSection.ContentSection)
                .ThenInclude(section => section.Content)
                .Include(r => r.Publication)
                .Include(r => r.Updates)
                .Single(r => r.Id == id);

            var releaseViewModel = _mapper.Map<CachedReleaseViewModel>(release);
            releaseViewModel.DownloadFiles =
                _fileStorageService.ListPublicFiles(release.Publication.Slug, release.Slug).Result.ToList();

            // If the release isn't live yet set the published date based on what we expect it to be
            releaseViewModel.Published ??= context.Published;

            return releaseViewModel;
        }

        public Release GetLatestRelease(Guid publicationId, IEnumerable<Guid> includedReleaseIds)
        {
            return _contentDbContext.Releases
                .Include(r => r.Publication)
                .Where(release => release.PublicationId == publicationId)
                .ToList()
                .Where(release => IsLatestVersionOfRelease(release.Publication.Releases, release, includedReleaseIds))
                .OrderBy(release => release.Year)
                .ThenBy(release => release.TimePeriodCoverage)
                .LastOrDefault();
        }

        public CachedReleaseViewModel GetLatestReleaseViewModel(Guid publicationId,
            IEnumerable<Guid> includedReleaseIds, PublishContext context)
        {
            var latestRelease = GetLatestRelease(publicationId, includedReleaseIds);
            return GetReleaseViewModel(latestRelease.Id, context);
        }

        public async Task SetPublishedDatesAsync(Guid id, DateTime published)
        {
            var contentRelease = await _contentDbContext.Releases
                .Include(release => release.Publication)
                .ThenInclude(publication => publication.Methodology)
                .SingleOrDefaultAsync(r => r.Id == id);

            var statisticsRelease = await _statisticsDbContext.Release
                .SingleOrDefaultAsync(r => r.Id == id);

            if (contentRelease == null)
            {
                throw new ArgumentException("Content Release does not exist", nameof(id));
            }

            if (contentRelease.Amendment)
            {
                var previousVersion = await _contentDbContext.Releases.AsNoTracking()
                    .SingleOrDefaultAsync(r => r.Id == contentRelease.PreviousVersionId);

                if (previousVersion?.Published == null)
                {
                    throw new ArgumentException("Previous version of release does not exist or is not live", nameof(contentRelease.PreviousVersionId));
                }

                published = previousVersion.Published.Value;
            }

            _contentDbContext.Releases.Update(contentRelease);
            contentRelease.Published ??= published;
            contentRelease.DataLastPublished = DateTime.UtcNow;

            // Update the Publication published date since we always generate the Publication when generating Release Content
            contentRelease.Publication.Published = published;

            // Update the Methodology published date if it's the first time it's published
            var methodology = contentRelease.Publication.Methodology;
            if (methodology != null)
            {
                methodology.Published ??= published;
            }

            await _contentDbContext.SaveChangesAsync();

            // The Release in the statistics database can be absent if no Subjects were created
            if (statisticsRelease != null)
            {
                _statisticsDbContext.Release.Update(statisticsRelease);
                statisticsRelease.Published ??= published;
                await _statisticsDbContext.SaveChangesAsync();
            }
        }

        public List<ReleaseFileReference> GetReleaseFileReferences(Guid releaseId, params ReleaseFileTypes[] types)
        {
            return _contentDbContext
                .ReleaseFiles
                .Include(rf => rf.ReleaseFileReference)
                .Where(rfr => rfr.ReleaseId == releaseId)
                .Select(rf => rf.ReleaseFileReference)
                .Where(rfr => types.Contains(rfr.ReleaseFileType))
                .ToList();
        }

        public async Task DeletePreviousVersionsStatisticalData(params Guid[] releaseIds)
        {
            var releases = await GetAmendedReleases(releaseIds);
            var previousVersions = releases.Select(r => r.PreviousVersionId)
                .Where(id => id.HasValue)
                .Cast<Guid>()
                .ToList();

            foreach (var previousVersion in previousVersions)
            {
                await _releaseSubjectService.SoftDeleteAllReleaseSubjects(previousVersion);
            }

            // Remove Statistical Releases for each of the Content Releases
            await RemoveStatisticalReleases(previousVersions);

            await _statisticsDbContext.SaveChangesAsync();
        }

        private async Task RemoveStatisticalReleases(IEnumerable<Guid> releaseIds)
        {
            var releases = await _statisticsDbContext.Release
                .Where(r => releaseIds.Contains(r.Id))
                .ToListAsync();

            _statisticsDbContext.Release.RemoveRange(releases);
        }
    }
}