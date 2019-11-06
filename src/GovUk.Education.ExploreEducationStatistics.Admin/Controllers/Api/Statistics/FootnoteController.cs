﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Admin.Models.Api.Statistics;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Data.Services.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Data.Services.ViewModels.Meta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api.Statistics
{
    [Route("api/data/[controller]")]
    [ApiController]
    [Authorize]
    public class FootnoteController : ControllerBase
    {
        private readonly IFilterService _filterService;
        private readonly IFootnoteService _footnoteService;
        private readonly IIndicatorGroupService _indicatorGroupService;
        private readonly IReleaseMetaService _releaseMetaService;
        private readonly IMapper _mapper;

        public FootnoteController(IFilterService filterService,
            IFootnoteService footnoteService,
            IIndicatorGroupService indicatorGroupService,
            IReleaseMetaService releaseMetaService,
            IMapper mapper)
        {
            _filterService = filterService;
            _footnoteService = footnoteService;
            _indicatorGroupService = indicatorGroupService;
            _releaseMetaService = releaseMetaService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<FootnoteViewModel> CreateFootnote(CreateFootnoteViewModel footnote)
        {
            return _mapper.Map<FootnoteViewModel>(_footnoteService.CreateFootnote(footnote.Content,
                footnote.Filters,
                footnote.FilterGroups,
                footnote.FilterItems,
                footnote.Indicators,
                footnote.Subjects));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteFootnote(long id)
        {
            return CheckFootnoteExists(id, () =>
            {
                _footnoteService.DeleteFootnote(id);
                return new NoContentResult();
            });
        }

        [HttpGet("release/{releaseId}")]
        public ActionResult<FootnotesViewModel> GetFootnotes(Guid releaseId)
        {
            var footnotes = _footnoteService.GetFootnotes(releaseId).ToList();
            var subjects = _releaseMetaService.GetSubjects(releaseId).ToDictionary(subject => subject.Id, subject =>
                new FootnotesSubjectMetaViewModel
                {
                    Filters = GetFilters(subject.Id),
                    Indicators = GetIndicators(subject.Id),
                    SubjectId = subject.Id,
                    SubjectName = subject.Label
                });

            return new FootnotesViewModel
            {
                Footnotes = _mapper.Map<IEnumerable<FootnoteViewModel>>(footnotes),
                Meta = subjects
            };
        }

        [HttpPut("{id}")]
        public ActionResult<FootnoteViewModel> UpdateFootnote(long id, UpdateFootnoteViewModel footnote)
        {
            return _mapper.Map<FootnoteViewModel>(_footnoteService.UpdateFootnote(id,
                footnote.Content,
                footnote.Filters,
                footnote.FilterGroups,
                footnote.FilterItems,
                footnote.Indicators,
                footnote.Subjects));
        }

        private ActionResult CheckFootnoteExists(long id, Func<ActionResult> andThen)
        {
            var footnote = _footnoteService.GetFootnote(id);
            return footnote == null ? NotFound() : andThen.Invoke();
        }

        private Dictionary<string, IndicatorsMetaViewModel> GetIndicators(long subjectId)
        {
            return _indicatorGroupService.GetIndicatorGroups(subjectId).ToDictionary(
                group => group.Label.PascalCase(),
                group => new IndicatorsMetaViewModel
                {
                    Label = group.Label,
                    Options = _mapper.Map<IEnumerable<IndicatorMetaViewModel>>(group.Indicators)
                }
            );
        }

        // TODO copied from TableBuilderSubjectMetaService#GetFilters (optimisation for getting all filters for subject id)
        private Dictionary<string, FilterMetaViewModel> GetFilters(long subjectId)
        {
            return _filterService.GetFiltersIncludingItems(subjectId)
                .ToDictionary(
                    filter => filter.Label.PascalCase(),
                    filter => new FilterMetaViewModel
                    {
                        Hint = filter.Hint,
                        Legend = filter.Label,
                        Options = filter.FilterGroups.ToDictionary(
                            filterGroup => filterGroup.Label.PascalCase(),
                            filterGroup => BuildFilterItemsViewModel(filterGroup, filterGroup.FilterItems))
                    });
        }

        // TODO copied from AbstractTableBuidlerSubjectMetaService
        private static FilterItemsMetaViewModel BuildFilterItemsViewModel(FilterGroup filterGroup,
            IEnumerable<FilterItem> filterItems)
        {
            return new FilterItemsMetaViewModel
            {
                Label = filterGroup.Label,
                Options = filterItems.Select(item => new LabelValue
                {
                    Label = item.Label,
                    Value = item.Id.ToString()
                })
            };
        }
    }
}