using System;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Interfaces.Security;
using GovUk.Education.ExploreEducationStatistics.Common.Utils;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using GovUk.Education.ExploreEducationStatistics.Content.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Publisher.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationErrorMessages;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationUtils;
using static GovUk.Education.ExploreEducationStatistics.Common.Services.QueueUtils;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Model.PublisherQueues;
using ReleaseStatus = GovUk.Education.ExploreEducationStatistics.Content.Model.ReleaseStatus;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services
{
    public class PublishingService : IPublishingService
    {
        private readonly IPersistenceHelper<ContentDbContext> _persistenceHelper;
        private readonly string _storageConnectionString;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public PublishingService(IPersistenceHelper<ContentDbContext> persistenceHelper,
            IUserService userService,
            IConfiguration config,
            ILogger<PublishingService> logger
        )
        {
            _persistenceHelper = persistenceHelper;
            _userService = userService;
            _storageConnectionString = config.GetValue<string>("PublisherStorage");
            _logger = logger;
        }

        /// <summary>
        /// Retry a stage of the publishing workflow
        /// </summary>
        /// <remarks>
        /// This results in the Publisher updating the latest ReleaseStatus for this Release rather than creating a new one.
        /// </remarks>
        /// <param name="releaseId"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        public async Task<Either<ActionResult, bool>> RetryStage(Guid releaseId, RetryStage stage)
        {
            return await _persistenceHelper
                .CheckEntityExists<Release>(releaseId)
                .OnSuccess(release => _userService.CheckCanPublishRelease(release))
                .OnSuccess(async release =>
                {
                    if (release.Status != ReleaseStatus.Approved)
                    {
                        return ValidationActionResult(ReleaseNotApproved);
                    }

                    var queue = await GetQueueReferenceAsync(_storageConnectionString, RetryStageQueue);
                    await queue.AddMessageAsync(ToCloudQueueMessage(new RetryStageMessage(releaseId, stage)));

                    _logger.LogTrace($"Sent retry stage message for Release: {releaseId}");
                    return new Either<ActionResult, bool>(true);
                });
        }

        /// <summary>
        /// <para>Notify the Publisher that there has been a change to the Release status.</para>
        /// <para>This could result in:</para>
        /// <list type="bullet">
        /// <item><term>Scheduling publication of a Release after approval</term></item>
        /// <item><term>Cancelling a schedule after un-approval</term></item>
        /// <item><term>Superseding an existing schedule with a new one</term></item>
        /// <item><term>Publishing a release immediately</term></item>
        /// <item><term>Publishing a release immediately superseding a schedule</term></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// Publishing will fail at the validation stage if the Release is already in the process of being published.
        /// Since the Data task deletes all existing Release statistics data before copying there will be downtime if this is called with a Release that is already published.
        /// A future schedule for publishing a Release that's not yet started will be cancelled.
        /// </remarks>
        /// <param name="releaseId"></param>
        /// <param name="immediate">If true, runs all of the stages of the publishing workflow except that they are combined to act immediately.</param>
        /// <returns></returns>
        public async Task<Either<ActionResult, bool>> NotifyChange(Guid releaseId, bool immediate = false)
        {
            return await _persistenceHelper
                .CheckEntityExists<Release>(releaseId)
                .OnSuccess(async release =>
                {
                    var queue = await GetQueueReferenceAsync(_storageConnectionString, NotifyChangeQueue);
                    await queue.AddMessageAsync(ToCloudQueueMessage(new NotifyChangeMessage(immediate, release.Id)));

                    _logger.LogTrace($"Sent validate message for Release: {releaseId}");
                    return true;
                });
        }
    }
}