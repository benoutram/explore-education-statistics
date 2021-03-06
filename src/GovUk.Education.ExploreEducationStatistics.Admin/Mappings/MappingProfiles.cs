using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Admin.Mappings.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Methodologies;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels.ManageContent;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels.Methodology;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using PublicationViewModel = GovUk.Education.ExploreEducationStatistics.Admin.ViewModels.PublicationViewModel;
using ReleaseStatus = GovUk.Education.ExploreEducationStatistics.Publisher.Model.ReleaseStatus;
using ReleaseViewModel = GovUk.Education.ExploreEducationStatistics.Admin.ViewModels.ReleaseViewModel;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Mappings
{
    /**
     * AutoMapper Profile which is configured by AutoMapper.Extensions.Microsoft.DependencyInjection.
     */
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Release, ReleaseViewModel>()
                .ForMember(
                    dest => dest.LatestRelease,
                    m => m.MapFrom(r => r.Publication.LatestPublishedRelease().Id == r.Id))
                .ForMember(dest => dest.Contact,
                    m => m.MapFrom(r => r.Publication.Contact))
                .ForMember(dest => dest.PublicationTitle,
                    m => m.MapFrom(r => r.Publication.Title))
                .ForMember(dest => dest.PublicationId,
                    m => m.MapFrom(r => r.Publication.Id))
                .ForMember(dest => dest.PublicationSlug,
                    m => m.MapFrom(r => r.Publication.Slug))
                .ForMember(model => model.PublishScheduled,
                    m => m.MapFrom(model =>
                        model.PublishScheduled.HasValue
                            ? model.PublishScheduled.Value.ConvertUtcToUkTimeZone()
                            : (DateTime?) null));

            CreateMap<Release, MyReleaseViewModel>()
                .ForMember(
                    dest => dest.LatestRelease,
                    m => m.MapFrom(r => r.Publication.LatestPublishedRelease().Id == r.Id))
                .ForMember(dest => dest.Contact,
                    m => m.MapFrom(r => r.Publication.Contact))
                .ForMember(dest => dest.PublicationTitle,
                    m => m.MapFrom(r => r.Publication.Title))
                .ForMember(dest => dest.PublicationId,
                    m => m.MapFrom(r => r.Publication.Id))
                .ForMember(dest => dest.PublicationSlug,
                    m => m.MapFrom(r => r.Publication.Slug))
                .ForMember(model => model.PublishScheduled,
                    m => m.MapFrom(model =>
                        model.PublishScheduled.HasValue
                            ? model.PublishScheduled.Value.ConvertUtcToUkTimeZone()
                            : (DateTime?) null))
                .ForMember(dest => dest.Permissions, exp => exp.MapFrom<IMyReleasePermissionSetPropertyResolver>());

            CreateMap<ReleaseCreateViewModel, Release>()
                .ForMember(dest => dest.PublishScheduled, m => m.MapFrom(model =>
                    model.PublishScheduledDate));

            CreateMap<ReleaseStatus, ReleaseStatusViewModel>()
                .ForMember(model => model.LastUpdated, m => m.MapFrom(status => status.Timestamp));

            CreateMap<Methodology, MethodologySummaryViewModel>();

            CreateMap<Methodology, MethodologyTitleViewModel>();
            CreateMap<Methodology, MethodologyPublicationsViewModel>();

            CreateMap<Publication, IdTitlePair>();

            CreateMap<Publication, PublicationViewModel>()
                .ForMember(dest => dest.Releases,
                    m => m.MapFrom(p => p.Releases
                        .FindAll(r => IsLatestVersionOfRelease(p.Releases, r.Id))))
                .ForMember(
                    dest => dest.LegacyReleases,
                    m => m.MapFrom(p => p.LegacyReleases.OrderByDescending(r => r.Order))
                )
                .ForMember(
                    dest => dest.ThemeId,
                    m => m.MapFrom(p => p.Topic.ThemeId));

            CreateMap<Publication, MyPublicationViewModel>()
                .ForMember(
                    dest => dest.ThemeId,
                    m => m.MapFrom(p => p.Topic.ThemeId))
                .ForMember(dest => dest.Releases,
                    m => m.MapFrom(p => p.Releases
                        .FindAll(r => IsLatestVersionOfRelease(p.Releases, r.Id))
                        .OrderByDescending(r => r.Year)
                        .ThenByDescending(r => r.TimePeriodCoverage)))
                .ForMember(dest => dest.Permissions, exp => exp.MapFrom<IMyPublicationPermissionSetPropertyResolver>());

            CreateMap<Contact, ContactViewModel>();

            CreateContentBlockMap();
            CreateMap<DataBlockCreateViewModel, DataBlock>();
            CreateMap<DataBlockUpdateViewModel, DataBlock>();
            CreateMap<DataBlock, DataBlockSummaryViewModel>()
                .ForMember(
                    dest => dest.ChartsCount,
                    m => m.MapFrom(d => d.Charts.Count));

            CreateMap<Theme, Data.Model.Theme>()
                .ForMember(dest => dest.Topics, m => m.Ignore());
            CreateMap<Topic, Data.Model.Topic>()
                .ForMember(dest => dest.Publications, m => m.Ignore())
                .ForMember(dest => dest.Theme, m => m.Ignore());
            CreateMap<Publication, Data.Model.Publication>()
                .ForMember(dest => dest.Releases, m => m.Ignore())
                .ForMember(dest => dest.Topic, m => m.Ignore());
            CreateMap<Release, Data.Model.Release>()
                .ForMember(dest => dest.Publication, m => m.Ignore())
                .ForMember(dest => dest.TimeIdentifier, m => m.MapFrom(r => r.TimePeriodCoverage));

            CreateMap<Theme, ThemeViewModel>()
                .ForMember(theme => theme.Topics, m => m.MapFrom(t => t.Topics.OrderBy(topic => topic.Title)));
            CreateMap<Topic, TopicViewModel>();

            CreateMap<ContentSection, ContentSectionViewModel>().ForMember(dest => dest.Content,
                m => m.MapFrom(section => section.Content.OrderBy(contentBlock => contentBlock.Order)));

            CreateMap<Release, ManageContentPageViewModel.ReleaseViewModel>()
                .ForMember(dest => dest.Content,
                    m => m.MapFrom(r => r.GenericContent.OrderBy(s => s.Order)))
                .ForMember(
                    dest => dest.Updates,
                    m => m.MapFrom(r => r.Updates.OrderByDescending(update => update.On)))
                .ForMember(dest => dest.Publication,
                    m => m.MapFrom(r => new ManageContentPageViewModel.PublicationViewModel
                    {
                        Id = r.Publication.Id,
                        Description = r.Publication.Description,
                        Title = r.Publication.Title,
                        Slug = r.Publication.Slug,
                        Summary = r.Publication.Summary,
                        DataSource = r.Publication.DataSource,
                        Contact = r.Publication.Contact,
                        Topic = new ManageContentPageViewModel.TopicViewModel
                        {
                            Theme = new ManageContentPageViewModel.ThemeViewModel
                            {
                                Title = r.Publication.Topic.Theme.Title
                            }
                        },
                        OtherReleases = r.Publication.Releases
                            .FindAll(otherRelease => r.Id != otherRelease.Id &&
                                                     IsLatestVersionOfRelease(r.Publication.Releases, otherRelease.Id))
                            .OrderByDescending(otherRelease => otherRelease.Year)
                            .ThenByDescending(otherRelease => otherRelease.TimePeriodCoverage)
                            .Select(otherRelease => new PreviousReleaseViewModel
                            {
                                Id = otherRelease.Id,
                                Slug = otherRelease.Slug,
                                Title = otherRelease.Title,
                            })
                            .ToList(),
                        LegacyReleases = r.Publication.LegacyReleases
                            .OrderByDescending(legacyRelease => legacyRelease.Order)
                            .Select(legacy => new ManageContentPageViewModel.LegacyReleaseViewModel
                            {
                                Id = legacy.Id,
                                Description = legacy.Description,
                                Url = legacy.Url,
                            })
                            .ToList(),
                        ExternalMethodology = r.Publication.ExternalMethodology != null
                            ? new ExternalMethodology
                            {
                                Title = r.Publication.ExternalMethodology.Title,
                                Url = r.Publication.ExternalMethodology.Url
                            }
                            : null,
                        Methodology = r.Publication.Methodology != null
                            ? new MethodologyTitleViewModel
                                {
                                    Id = r.Publication.Methodology.Id,
                                    Title = r.Publication.Methodology.Title
                                }
                            : null
                    }))
                .ForMember(
                    dest => dest.LatestRelease,
                    m => m.MapFrom(r => r.Publication.LatestPublishedRelease().Id == r.Id))
                .ForMember(dest => dest.CoverageTitle,
                    m => m.MapFrom(r => r.TimePeriodCoverage.GetEnumLabel()))
                .ForMember(
                    dest => dest.HasPreReleaseAccessList,
                    m => m.MapFrom(r => !r.PreReleaseAccessList.IsNullOrEmpty()))
                .ForMember(model => model.PublishScheduled,
                    m => m.MapFrom(model =>
                        model.PublishScheduled.HasValue
                            ? model.PublishScheduled.Value.ConvertUtcToUkTimeZone()
                            : (DateTime?) null));

            CreateMap<Update, ReleaseNoteViewModel>();

            CreateMap<LegacyRelease, LegacyReleaseViewModel>();

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.CreatedBy,
                    m => m.MapFrom(comment =>
                        comment.CreatedById == null
                            ? new User
                            {
#pragma warning disable 612
                                FirstName = comment.LegacyCreatedBy,
#pragma warning restore 612
                                LastName = ""
                            }
                            : comment.CreatedBy));

            CreateMap<ContentSection, ContentSectionViewModel>()
                .ForMember(dest => dest.Content,
                m => m.MapFrom(section => section.Content.OrderBy(contentBlock => contentBlock.Order)));

            CreateMap<Methodology, ManageMethodologyContentViewModel>()
                .ForMember(dest => dest.Content,
                    m => m.MapFrom(methodology => methodology.Content.OrderBy(contentSection => contentSection.Order)))
                .ForMember(dest => dest.Annexes,
                    m => m.MapFrom(methodology => methodology.Annexes.OrderBy(annexSection => annexSection.Order)));

            CreateMap<Release, ReleasePublicationStatusViewModel>();
        }

        private void CreateContentBlockMap()
        {
            CreateMap<ContentBlock, IContentBlockViewModel>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Comments,
                    m => m.MapFrom(block => block.Comments.OrderBy(comment => comment.Created)));

            CreateMap<DataBlock, DataBlockViewModel>();

            CreateMap<HtmlBlock, HtmlBlockViewModel>();

            CreateMap<MarkDownBlock, MarkDownBlockViewModel>();
        }

        private static bool IsLatestVersionOfRelease(IEnumerable<Release> releases, Guid releaseId)
        {
            return !releases.Any(r => r.PreviousVersionId == releaseId && r.Id != releaseId);
        }
    }
}
