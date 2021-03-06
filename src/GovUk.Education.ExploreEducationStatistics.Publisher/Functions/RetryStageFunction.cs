﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Publisher.Model;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Model.PublisherQueues;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Model.RetryStage;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Functions
{
    // ReSharper disable once UnusedType.Global
    public class RetryStageFunction
    {
        private readonly IQueueService _queueService;
        private readonly IReleaseStatusService _releaseStatusService;

        private static readonly ReleaseStatusOverallStage[] ValidStates =
        {
            ReleaseStatusOverallStage.Complete, ReleaseStatusOverallStage.Failed
        };

        public RetryStageFunction(
            IQueueService queueService,
            IReleaseStatusService releaseStatusService)
        {
            _queueService = queueService;
            _releaseStatusService = releaseStatusService;
        }

        /// <summary>
        /// BAU Azure function which retries stage of the publishing workflow, updating the latest ReleaseStatus.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="executionContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        [FunctionName("RetryStage")]
        // ReSharper disable once UnusedMember.Global
        public async Task RetryStage(
            [QueueTrigger(RetryStageQueue)]
            RetryStageMessage message,
            ExecutionContext executionContext,
            ILogger logger)
        {
            logger.LogInformation("{0} triggered at: {1}",
                executionContext.FunctionName,
                DateTime.Now);

            var releaseStatus = await _releaseStatusService.GetLatestAsync(message.ReleaseId);

            if (releaseStatus == null)
            {
                logger.LogError(
                    "Latest status not found for Release: {0} while attempting to retry",
                    message.ReleaseId);
            }
            else
            {
                if (!ValidStates.Contains(releaseStatus.State.Overall))
                {
                    logger.LogError("Can only attempt a retry of Release: {0} if the latest " +
                                    "status is in ({1}). Found: {2}",
                        message.ReleaseId,
                        string.Join(", ", ValidStates),
                        releaseStatus.State.Overall);
                }
                else
                {
                    switch (message.Stage)
                    {
                        case ContentAndPublishing:
                            await _queueService.QueuePublishReleaseContentMessageAsync(message.ReleaseId,
                                releaseStatus.Id);
                            break;
                        case StatisticsData:
                            await _queueService.QueuePublishReleaseDataMessageAsync(message.ReleaseId,
                                releaseStatus.Id);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            logger.LogInformation("{0} completed", executionContext.FunctionName);
        }
    }
}
