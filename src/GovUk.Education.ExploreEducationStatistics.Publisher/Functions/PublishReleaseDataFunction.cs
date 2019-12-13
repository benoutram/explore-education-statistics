﻿using GovUk.Education.ExploreEducationStatistics.Publisher.Model;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Model.Stage;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Functions
{
    public class PublishReleaseDataFunction
    {
        private readonly IReleaseStatusService _releaseStatusService;
        
        public const string QueueName = "publish-release-data";

        public PublishReleaseDataFunction(IReleaseStatusService releaseStatusService)
        {
            _releaseStatusService = releaseStatusService;
        }

        [FunctionName("PublishReleaseData")]
        public async void PublishReleaseData(
            [QueueTrigger(QueueName)] PublishReleaseDataMessage message,
            ExecutionContext executionContext,
            ILogger logger)
        {
            logger.LogInformation($"{executionContext.FunctionName} triggered: {message}");
            // TODO EES-866 Run the importer or copy the data from the statistics database
            // TODO EES-866 to the publicly available statistics database
            await _releaseStatusService.UpdateStageAsync(message.ReleaseId, message.ReleaseStatusId, Failed);
            logger.LogInformation($"{executionContext.FunctionName} completed");
        }
    }
}