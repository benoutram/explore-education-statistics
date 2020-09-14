﻿using System;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.ViewModels;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces
{
    public interface IReplacementService
    {
        Task<Either<ActionResult, ReplacementPlanViewModel>> GetReplacementPlan(
            Guid originalReleaseFileReferenceId,
            Guid replacementReleaseFileReferenceId);
        
        Task<Either<ActionResult, Unit>> Replace(
            Guid originalReleaseFileReferenceId,
            Guid replacementReleaseFileReferenceId);
    }
}