using System.Collections.Generic;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Data.Services.ViewModels.Meta;
using Microsoft.EntityFrameworkCore.Internal;

namespace GovUk.Education.ExploreEducationStatistics.Data.Services
{
    public abstract class AbstractTableBuilderSubjectMetaService
    {
        private readonly IFilterItemService _filterItemService;
        private static IEqualityComparer<Filter> FilterComparer { get; } = new FilterEqualityComparer();
        private static IEqualityComparer<FilterGroup> FilterGroupComparer { get; } = new FilterGroupEqualityComparer();

        protected AbstractTableBuilderSubjectMetaService(IFilterItemService filterItemService)
        {
            _filterItemService = filterItemService;
        }

        protected Dictionary<string, FilterMetaViewModel> GetFilters(IQueryable<Observation> observations)
        {
            return _filterItemService.GetFilterItemsIncludingFilters(observations)
                .GroupBy(item => item.FilterGroup.Filter, item => item, FilterComparer)
                .ToDictionary(
                    itemsGroupedByFilter => itemsGroupedByFilter.Key.Label.PascalCase(),
                    itemsGroupedByFilter => new FilterMetaViewModel
                    {
                        Hint = itemsGroupedByFilter.Key.Hint,
                        Legend = itemsGroupedByFilter.Key.Label,
                        Options = itemsGroupedByFilter
                            .GroupBy(item => item.FilterGroup, item => item, FilterGroupComparer)
                            .ToDictionary(
                                itemsGroupedByFilterGroup => itemsGroupedByFilterGroup.Key.Label.PascalCase(),
                                itemsGroupedByFilterGroup => BuildFilterItemsViewModel(itemsGroupedByFilterGroup.Key,
                                    itemsGroupedByFilterGroup)
                            ),
                        TotalValue = GetTotalValue(itemsGroupedByFilter)
                    });
        }

        protected static FilterItemsMetaViewModel BuildFilterItemsViewModel(FilterGroup filterGroup,
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

        protected static IEnumerable<T> TransformDuplicateObservationalUnitsWithUniqueLabels<T>(
            IEnumerable<T> viewModels) where T : LabelValue
        {
            /*
             The list of Observational Units should in theory already be unique.
             If they are not, there's three possibilities:
              * Duplicates exist where the label-value pairs are distinct but the Level attribute is different
                i.e. where the same Observational Unit is reused across multiple Geographic Levels e.g. LA and LAD.
                These need transforming to give them distinct labels.
              * Duplicates where the labels are the same but the values are different.
                These need transforming to give them distinct labels.
              * Duplicates where the values are the same but the labels are different.
                These don't need any action.
            */

            var case1 = viewModels
                .GroupBy(model => (model.Value, model.Label))
                .Where(grouping => grouping.Count() > 1)
                .SelectMany(grouping => grouping)
                .ToList();

            var case2 = viewModels.Except(case1)
                .GroupBy(model => model.Label)
                .Where(grouping => grouping.Count() > 1)
                .SelectMany(grouping => grouping)
                .ToList();

            if (!(case1.Any() || case2.Any()))
            {
                return viewModels;
            }

            return viewModels.Select(value =>
            {
                if (case1.Contains(value))
                {
                    if (value is ObservationalUnitMetaViewModel observationalUnitMetaViewModel)
                    {
                        observationalUnitMetaViewModel.Label +=
                            $" ({observationalUnitMetaViewModel.Level.GetEnumLabel()})";
                    }
                }

                if (case2.Contains(value))
                {
                    value.Label += $" ({value.Value})";
                }

                return value;
            });
        }

        private string GetTotalValue(IEnumerable<FilterItem> filterItems)
        {
            return _filterItemService.GetTotal(filterItems)?.Id.ToString() ?? string.Empty;
        }

        private sealed class FilterEqualityComparer : IEqualityComparer<Filter>
        {
            public bool Equals(Filter x, Filter y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Filter obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        private sealed class FilterGroupEqualityComparer : IEqualityComparer<FilterGroup>
        {
            public bool Equals(FilterGroup x, FilterGroup y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(FilterGroup obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}