using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Query;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Services;
using GovUk.Education.ExploreEducationStatistics.Data.Services.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GovUk.Education.ExploreEducationStatistics.Data.Services
{
    public class ObservationService : AbstractRepository<Observation, long>, IObservationService
    {
        public ObservationService(
            ApplicationDbContext context,
            ILogger<ObservationService> logger) : base(context, logger)
        {
        }

        public IEnumerable<Observation> FindObservations(ObservationQueryContext query)
        {
            var subjectIdParam = new SqlParameter("subjectId", query.SubjectId);
            var geographicLevelParam = new SqlParameter("geographicLevel",
                query.GeographicLevel?.GetEnumValue() ?? (object) DBNull.Value);
            var timePeriodListParam = CreateTimePeriodListType("timePeriodList", GetTimePeriodRange(query));
            var countriesListParam = CreateIdListType("countriesList", query.Country);
            var institutionListParam =
                CreateIdListType("institutionList", query.Institution);
            var localAuthorityListParam = CreateIdListType("localAuthorityList", query.LocalAuthority);
            var localAuthorityDistrictListParam =
                CreateIdListType("localAuthorityDistrictList", query.LocalAuthorityDistrict);
            var localEnterprisePartnershipListParam =
                CreateIdListType("localEnterprisePartnershipList", query.LocalEnterprisePartnership);
            var mayoralCombinedAuthorityListParam =
                CreateIdListType("mayoralCombinedAuthorityList", query.MayoralCombinedAuthority);
            var multiAcademyTrustListParam =
                CreateIdListType("multiAcademyTrustList", query.MultiAcademyTrust);
            var opportunityAreaListParam =
                CreateIdListType("opportunityAreaList", query.OpportunityArea);
            var parliamentaryConstituencyListParam =
                CreateIdListType("parliamentaryConstituencyList", query.ParliamentaryConstituency);
            var regionsListParam = CreateIdListType("regionsList", query.Region);
            var rscRegionListParam = CreateIdListType("rscRegionsList", query.RscRegion);
            var sponsorListParam = CreateIdListType("sponsorList", query.Sponsor);
            var wardListParam =
                CreateIdListType("wardList", query.Ward);
            var filtersListParam = CreateIdListType("filtersList", query.Filters);

            var inner = _context.Query<IdWrapper>().AsNoTracking()
                .FromSql("EXEC dbo.FilteredObservations " +
                         "@subjectId," +
                         "@geographicLevel," +
                         "@timePeriodList," +
                         "@countriesList," +
                         "@institutionList," +
                         "@localAuthorityList," +
                         "@localAuthorityDistrictList," +
                         "@localEnterprisePartnershipList," +
                         "@mayoralCombinedAuthorityList," +
                         "@multiAcademyTrustList," +
                         "@opportunityAreaList," +
                         "@parliamentaryConstituencyList," +
                         "@regionsList," +
                         "@rscRegionsList," +
                         "@sponsorList," +
                         "@wardList," +
                         "@filtersList",
                    subjectIdParam,
                    geographicLevelParam,
                    timePeriodListParam,
                    countriesListParam,
                    institutionListParam,
                    localAuthorityListParam,
                    localAuthorityDistrictListParam,
                    localEnterprisePartnershipListParam,
                    mayoralCombinedAuthorityListParam,
                    multiAcademyTrustListParam,
                    opportunityAreaListParam,
                    parliamentaryConstituencyListParam,
                    regionsListParam,
                    rscRegionListParam,
                    sponsorListParam,
                    wardListParam,
                    filtersListParam);

            var ids = inner.Select(obs => obs.Id).ToList();
            var batches = ids.Batch(10000);

            var result = new List<Observation>();

            foreach (var batch in batches)
            {
                result.AddRange(DbSet()
                    .AsNoTracking()
                    .Where(observation => batch.Contains(observation.Id))
                    .Include(observation => observation.Location)
                    .Include(observation => observation.FilterItems)
                    .ThenInclude(item => item.FilterItem.FilterGroup.Filter));
            }

            // Nullify all of the Observational Unit fields which are empty
            // Do this here on the set of distinct Locations rather than when building each ObservationViewModel
            // (since a Location may be referenced by more than one Observation) 
            result.Select(observation => observation.Location)
                .Distinct()
                .ToList()
                .ForEach(location => location.ReplaceEmptyOwnedTypeValuesWithNull());

            return result;
        }

        public IEnumerable<Observation> FindObservations(SubjectMetaQueryContext query)
        {
            return DbSet().AsNoTracking().Where(ObservationPredicateBuilder.Build(query));
        }

        private static SqlParameter CreateTimePeriodListType(string parameterName,
            IEnumerable<(int Year, TimeIdentifier TimeIdentifier)> values)
        {
            return CreateListType(parameterName, values.AsTimePeriodListTable(), "dbo.TimePeriodListType");
        }

        private static IEnumerable<(int Year, TimeIdentifier TimeIdentifier)> GetTimePeriodRange(
            ObservationQueryContext query)
        {
            if (query.TimePeriod.StartCode.IsNumberOfTerms() || query.TimePeriod.EndCode.IsNumberOfTerms())
            {
                return TimePeriodUtil.RangeForNumberOfTerms(query.TimePeriod.StartYear, query.TimePeriod.EndYear);
            }

            return TimePeriodUtil.Range(query.TimePeriod);
        }
    }
}