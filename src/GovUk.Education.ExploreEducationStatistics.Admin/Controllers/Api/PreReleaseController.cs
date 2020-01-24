using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Utils;
using GovUk.Education.ExploreEducationStatistics.Admin.Services;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class PreReleaseController : ControllerBase
    {
        private readonly IPreReleaseService _preReleaseService;

        public PreReleaseController(IPreReleaseService preReleaseService)
        {
            _preReleaseService = preReleaseService;
        }

        [HttpGet("prerelease/contacts")]
        public async Task<ActionResult<List<PrereleaseCandidateViewModel>>> GetAvailablePreReleaseContacts()
        {
            return await _preReleaseService
                .GetAvailablePreReleaseContactsAsync()
                .HandleFailuresOr(Ok);
        }

        [HttpGet("release/{releaseId}/prerelease-contacts")]
        public async Task<ActionResult<List<PrereleaseCandidateViewModel>>> GetPreReleaseContactsForRelease(Guid releaseId)
        {
            return await _preReleaseService
                .GetPreReleaseContactsForReleaseAsync(releaseId)
                .HandleFailuresOr(Ok);
        }

        [HttpPost("release/{releaseId}/prerelease-contact/{email}")]
        public async Task<ActionResult<List<PrereleaseCandidateViewModel>>> AddPreReleaseContactToRelease(Guid releaseId, string email)
        {
            return await _preReleaseService
                .AddPreReleaseContactToReleaseAsync(releaseId, email)
                .HandleFailuresOr(Ok);
        }

        [HttpDelete("release/{releaseId}/prerelease-contact/{email}")]
        public async Task<ActionResult<List<PrereleaseCandidateViewModel>>> RemovePreReleaseContactFromRelease(Guid releaseId, string email)
        {
            return await _preReleaseService
                .RemovePreReleaseContactFromReleaseAsync(releaseId, email)
                .HandleFailuresOr(Ok);
        }
    }
}