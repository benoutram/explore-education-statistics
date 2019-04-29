namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services
{
    using GovUk.Education.ExploreEducationStatistics.Data.Importer.Services;
    using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
    using GovUk.Education.ExploreEducationStatistics.Data.Processor.Models;
    using GovUK.Education.ExploreEducationStatistics.Data.Processor.Services;
    using Microsoft.Extensions.Logging;

    public class SeedService : ISeedService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IImporterService _importerService;
        private readonly IBlobService _blobService;

        public SeedService(
            ILogger<SeedService> logger,
            ApplicationDbContext context,
            IImporterService importerService,
            IBlobService blobService)
        {
            _logger = logger;
            _context = context;
            _importerService = importerService;
            _blobService = blobService;
        }

        public void SeedRelease(Release release)
        {
            _logger.LogInformation("Seeding Release for {Publication}, {Release}", release.PublicationId, release.Name);

            var releaseDb = CreateRelease(release);

            foreach (var subject in release.Subjects)
            {
                SeedSubject(releaseDb, subject);
            }
        }

        private void SeedSubject(Model.Release release, Subject subject)
        {
            _logger.LogInformation("Seeding Subject for {Publication}, {Subject}", release.PublicationId, subject.Name);
 
            var subjectDb = CreateSubject(release, subject);
            var sSubject = subjectDb.Name.Split("_");
            var destFolder = sSubject[0] + "/" + release.PublicationId.ToString();

            _importerService.Import(subject.GetCsvLines(), subject.GetMetaLines(), subjectDb);

            _blobService.MoveBlobBetweenContainers(subject.CsvDataBlob, "processed", destFolder);
            _blobService.MoveBlobBetweenContainers(subject.CsvMetaDataBlob, "processed", destFolder);
        }

        private Model.Release CreateRelease(Release release)
        {
            var releaseDb = _context.Release.Add(new Model.Release(release.ReleaseDate, release.PublicationId)).Entity;
            _context.SaveChanges();
            return releaseDb;
        }

        private Model.Subject CreateSubject(Model.Release release, Subject subject)
        {
            var subjectDb = _context.Subject.Add(new Model.Subject(subject.Name, release)).Entity;
            _context.SaveChanges();
            return subjectDb;
        }
    }
}