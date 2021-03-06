using System;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Publisher.Model.ViewModels;

namespace GovUk.Education.ExploreEducationStatistics.Content.Api.ViewModels
{
    public class ReleaseSummaryViewModel
    {
        public Guid Id { get; }

        public string Title { get; }

        public string Slug { get; }

        public string YearTitle { get; }

        public string CoverageTitle { get; }

        public DateTime? Published { get; }

        public string ReleaseName { get; }

        public PartialDate NextReleaseDate { get; }

        public ReleaseTypeViewModel Type { get; }

        public bool LatestRelease { get; }

        public DateTime? DataLastPublished { get; }

        public PublicationSummaryViewModel Publication { get; }

        public ReleaseSummaryViewModel(CachedReleaseViewModel release, CachedPublicationViewModel publication)
        {
            Id = release.Id;
            Title = release.Title;
            Slug = release.Slug;
            YearTitle = release.YearTitle;
            CoverageTitle = release.CoverageTitle;
            Published = release.Published;
            ReleaseName = release.ReleaseName;
            NextReleaseDate = release.NextReleaseDate;
            Type = release.Type;
            LatestRelease = Id == publication.LatestReleaseId;
            DataLastPublished = release.DataLastPublished;
            Publication = new PublicationSummaryViewModel(publication);
        }
    }
}