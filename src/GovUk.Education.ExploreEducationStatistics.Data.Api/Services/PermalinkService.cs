using Newtonsoft.Json;

namespace GovUk.Education.ExploreEducationStatistics.Data.Api.Services
{
    using System;
    using System.Threading.Tasks;
    using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces;
    using GovUk.Education.ExploreEducationStatistics.Data.Api.Models;
    using GovUk.Education.ExploreEducationStatistics.Data.Api.Models.Query;
    using GovUk.Education.ExploreEducationStatistics.Data.Api.Services.Interfaces;
    using Microsoft.Azure.Cosmos.Table;
    
    public class PermalinkService : IPermalinkService
    {
        private const string PermalinkTableName = "permalink";
        private readonly ITableStorageService _tableStorageService;

        public PermalinkService(ITableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public async Task<Permalink> GetAsync(Guid id)
        {
            var table = await _tableStorageService.GetTableAsync(PermalinkTableName);
            
            var retrieveOperation = TableOperation.Retrieve<Permalink>("the-publication-id", id.ToString());
            var result = await table.ExecuteAsync(retrieveOperation);
            var permalink = result.Result as Permalink;

            return permalink;
        }

        public async Task<Permalink> CreateAsync(ObservationQueryContext tableQuery)
        {
            var table = await _tableStorageService.GetTableAsync(PermalinkTableName);

            var permalink = new Permalink()
            {
                Title = "The table title",
                Data = "the data",
                Query = JsonConvert.SerializeObject(tableQuery)
            };

            var insertOperation = TableOperation.Insert(permalink);
            var result = await table.ExecuteAsync(insertOperation);
            var insertedPermalink = result.Result as Permalink;
            
            return insertedPermalink;
        }
    }
}