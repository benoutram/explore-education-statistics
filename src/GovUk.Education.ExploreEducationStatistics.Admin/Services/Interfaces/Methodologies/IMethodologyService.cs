﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels.Methodology;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.Methodologies
{
    public interface IMethodologyService
    {
        Task<Either<ActionResult, List<MethodologySummaryViewModel>>> ListAsync();

        Task<Either<ActionResult, List<MethodologyPublicationsViewModel>>> ListWithPublicationsAsync();

        Task<Either<ActionResult, MethodologySummaryViewModel>> GetSummaryAsync(Guid id);

        Task<Either<ActionResult, MethodologySummaryViewModel>>
            CreateMethodologyAsync(MethodologyCreateRequest request);

        Task<Either<ActionResult, MethodologySummaryViewModel>> UpdateMethodologyAsync(Guid id,
            MethodologyUpdateRequest request);
    }
}
