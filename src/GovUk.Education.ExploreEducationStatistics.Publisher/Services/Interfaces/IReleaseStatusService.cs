﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Publisher.Model;
using Microsoft.Azure.Cosmos.Table;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces
{
    public interface IReleaseStatusService
    {
        Task CreateAsync(Guid releaseId, ReleaseStatusState state,
            IEnumerable<ReleaseStatusLogMessage> logMessages = null);

        Task<IEnumerable<ReleaseStatus>> ExecuteQueryAsync(TableQuery<ReleaseStatus> query);

        Task UpdateStateAsync(Guid releaseId, Guid releaseStatusId, ReleaseStatusState state);

        Task UpdateContentStageAsync(Guid releaseId, Guid releaseStatusId, ReleaseStatusContentStage stage,
            ReleaseStatusLogMessage logMessage = null);

        Task UpdateDataStageAsync(Guid releaseId, Guid releaseStatusId, ReleaseStatusDataStage stage,
            ReleaseStatusLogMessage logMessage = null);

        Task UpdateFilesStageAsync(Guid releaseId, Guid releaseStatusId, ReleaseStatusFilesStage stage,
            ReleaseStatusLogMessage logMessage = null);

        Task UpdatePublishingStageAsync(Guid releaseId, Guid releaseStatusId, ReleaseStatusPublishingStage stage,
            ReleaseStatusLogMessage logMessage = null);
    }
}