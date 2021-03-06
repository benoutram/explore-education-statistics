﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GovUk.Education.ExploreEducationStatistics.Common.Model.FileType;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Api
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ReleaseFileController : ControllerBase
    {
        private readonly IDataBlockService _dataBlockService;
        private readonly IReleaseFileService _releaseFileService;

        public ReleaseFileController(IDataBlockService dataBlockService,
            IReleaseFileService releaseFileService)
        {
            _dataBlockService = dataBlockService;
            _releaseFileService = releaseFileService;
        }

        [HttpDelete("release/{releaseId}/ancillary/{id}")]
        public async Task<ActionResult> DeleteFile(
            Guid releaseId, Guid id)
        {
            return await _releaseFileService
                .Delete(releaseId, id)
                .HandleFailuresOrNoContent();
        }

        [HttpDelete("release/{releaseId}/chart/{id}")]
        public async Task<ActionResult> DeleteChartFile(
            Guid releaseId, Guid id)
        {
            return await _dataBlockService.RemoveChartFile(releaseId, id)
                .HandleFailuresOrNoContent();
        }

        [HttpGet("release/{releaseId}/file/{id}")]
        public async Task<ActionResult> Stream(Guid releaseId, Guid id)
        {
            return await _releaseFileService
                .Stream(releaseId: releaseId, id: id)
                .HandleFailures();
        }

        [HttpGet("release/{releaseId}/ancillary")]
        public async Task<ActionResult<IEnumerable<FileInfo>>> GetAncillaryFiles(Guid releaseId)
        {
            return await _releaseFileService
                .ListAll(releaseId, Ancillary)
                .HandleFailuresOrOk();
        }

        [HttpPut("release/{releaseId}/chart/{id}")]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult<FileInfo>> UpdateChartFile(Guid releaseId, Guid id, IFormFile file)
        {
            return await _releaseFileService
                .UploadChart(releaseId, file, replacingId: id)
                .HandleFailuresOrOk();
        }

        [HttpPost("release/{releaseId}/ancillary")]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult<FileInfo>> UploadAncillary(Guid releaseId,
            [FromQuery(Name = "name"), Required] string name, IFormFile file)
        {
            return await _releaseFileService
                .UploadAncillary(releaseId, file, name)
                .HandleFailuresOrOk();
        }

        [HttpPost("release/{releaseId}/chart")]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult<FileInfo>> UploadChart(Guid releaseId, IFormFile file)
        {
            return await _releaseFileService
                .UploadChart(releaseId, file)
                .HandleFailuresOrOk();
        }
    }
}
