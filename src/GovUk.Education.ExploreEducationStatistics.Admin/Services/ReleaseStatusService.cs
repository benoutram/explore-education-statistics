﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using static GovUk.Education.ExploreEducationStatistics.Common.TableStorageTableNames;
using ReleaseStatus = GovUk.Education.ExploreEducationStatistics.Publisher.Model.ReleaseStatus;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class ReleaseStatusService : IReleaseStatusService
    {
        private readonly IMapper _mapper;
        private readonly ITableStorageService _publisherTableStorageService;
        private readonly IUserService _userService;
        private readonly IPersistenceHelper<ContentDbContext> _persistenceHelper;

        public ReleaseStatusService(
            IMapper mapper,
            IUserService userService,
            IPersistenceHelper<ContentDbContext> persistenceHelper,
            ITableStorageService publisherTableStorageService)
        {
            _mapper = mapper;
            _userService = userService;
            _persistenceHelper = persistenceHelper;
            _publisherTableStorageService = publisherTableStorageService;
        }

        public async Task<Either<ActionResult, ReleaseStatusViewModel>> GetReleaseStatusAsync(Guid releaseId)
        {
            return await _persistenceHelper
                .CheckEntityExists<Release>(releaseId)
                .OnSuccess(_userService.CheckCanViewRelease)
                .OnSuccess(async _ =>
                {
                    var query = new TableQuery<ReleaseStatus>()
                        .Where(TableQuery.GenerateFilterCondition(nameof(ReleaseStatus.PartitionKey),
                            QueryComparisons.Equal, releaseId.ToString()));

                    var result = await _publisherTableStorageService.ExecuteQueryAsync(PublisherReleaseStatusTableName, query);
                    var first = result.OrderByDescending(releaseStatus => releaseStatus.Created).FirstOrDefault();
                    return _mapper.Map<ReleaseStatusViewModel>(first);
                });
        }
    }
}