using System;
using System.IO;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Publisher.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static GovUk.Education.ExploreEducationStatistics.Publisher.Model.Stage;

namespace GovUk.Education.ExploreEducationStatistics.Publisher.Functions
{
    public class DataFactoryPipelineStatusFunction
    {
        private readonly IReleaseStatusService _releaseStatusService;

        public DataFactoryPipelineStatusFunction(IReleaseStatusService releaseStatusService)
        {
            _releaseStatusService = releaseStatusService;
        }

        [FunctionName("DataFactoryPipelineStatusFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "datafactory/pipeline/status/")]
            HttpRequest req,
            ILogger logger,
            ExecutionContext executionContext)
        {
            logger.LogInformation($"{executionContext.FunctionName} triggered");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<PipelineResponse>(requestBody);

            if (response.Status != "Complete")
            {
                logger.LogError(
                    $"Datafactory pipelined failed with status: {response.Status}, error: {response.ErrorMessage}");
            }

            await _releaseStatusService.UpdateDataStageAsync(response.ReleaseId, response.ReleaseStatusId,
                response.Status == "Complete" ? Complete : Failed);

            logger.LogInformation($"{executionContext.FunctionName} completed");

            return response.Status != null
                ? (ActionResult) new OkObjectResult($"status, {response.Status}")
                : new BadRequestObjectResult("No status was passed in the request body");
        }
    }

    internal class PipelineResponse
    {
        public Guid ReleaseId { get; set; }
        public Guid ReleaseStatusId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}