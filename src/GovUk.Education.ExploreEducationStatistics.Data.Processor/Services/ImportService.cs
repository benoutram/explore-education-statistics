using System.Linq;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Data.Importer.Services;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Publication = GovUk.Education.ExploreEducationStatistics.Data.Model.Publication;
using Release = GovUk.Education.ExploreEducationStatistics.Data.Model.Release;
using Theme = GovUk.Education.ExploreEducationStatistics.Data.Model.Theme;
using Topic = GovUk.Education.ExploreEducationStatistics.Data.Model.Topic;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services
{
    public class ImportService : IImportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IImporterService _importerService;
        private readonly IMapper _mapper;

        public ImportService(
            ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IImporterService importerService,
            IMapper mapper)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _importerService = importerService;
            _mapper = mapper;
        }

        public void Import(ImportMessage message)
        {
            var subjectData = _fileStorageService.GetSubjectData(message).Result;
            var release = CreateOrUpdateRelease(message);
            var subject = CreateSubject(subjectData.Name, release);
            _context.SaveChanges();

            _importerService.Import(subjectData.GetCsvLines(), subjectData.GetMetaLines(), subject);
        }

        private Subject CreateSubject(string name, Release release)
        {
            var subject = _context.Subject.Add(new Subject(name, release)).Entity;
            return subject;
        }

        private Release CreateOrUpdateRelease(ImportMessage message)
        {
            var release = _context.Release
                .Include(r => r.Publication)
                .ThenInclude(p => p.Topic)
                .ThenInclude(t => t.Theme)
                .FirstOrDefault(r => r.Id.Equals(message.Release.Id));

            if (release == null)
            {
                release = new Release
                {
                    Id = message.Release.Id,
                    Title = message.Release.Title,
                    Slug = message.Release.Slug,
                    Publication = CreateOrUpdatePublication(message)
                };
                return _context.Release.Add(release).Entity;
            }

            release = _mapper.Map(message.Release, release);
            return _context.Release.Update(release).Entity;
        }

        private Publication CreateOrUpdatePublication(ImportMessage message)
        {
            var publication = _context.Publication
                .Include(p => p.Topic)
                .ThenInclude(t => t.Theme)
                .FirstOrDefault(p => p.Id.Equals(message.Release.Publication.Id));

            if (publication == null)
            {
                publication = new Publication
                {
                    Id = message.Release.Publication.Id,
                    Title = message.Release.Publication.Title,
                    Slug = message.Release.Publication.Slug,
                    Topic = CreateOrUpdateTopic(message)
                };
                return _context.Publication.Add(publication).Entity;
            }

            publication = _mapper.Map(message.Release.Publication, publication);
            return _context.Publication.Update(publication).Entity;
        }

        private Topic CreateOrUpdateTopic(ImportMessage message)
        {
            var topic = _context.Topic
                .Include(p => p.Theme)
                .FirstOrDefault(t => t.Id.Equals(message.Release.Publication.Topic.Id));

            if (topic == null)
            {
                topic = new Topic
                {
                    Id = message.Release.Publication.Id,
                    Title = message.Release.Publication.Title,
                    Slug = message.Release.Publication.Slug,
                    Theme = CreateOrUpdateTheme(message)
                };
                return _context.Topic.Add(topic).Entity;
            }

            topic = _mapper.Map(message.Release.Publication.Topic, topic);
            return _context.Topic.Update(topic).Entity;
        }

        private Theme CreateOrUpdateTheme(ImportMessage message)
        {
            var theme = _context.Theme
                .FirstOrDefault(t => t.Id.Equals(message.Release.Publication.Topic.Theme.Id));

            if (theme == null)
            {
                theme = _mapper.Map<Theme>(message.Release.Publication.Topic.Theme);
                return _context.Theme.Add(theme).Entity;
            }

            theme = _mapper.Map(message.Release.Publication.Topic.Theme, theme);
            return _context.Theme.Update(theme).Entity;
        }
    }
}