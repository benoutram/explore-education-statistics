using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Services.Interfaces.Methodologies;
using GovUk.Education.ExploreEducationStatistics.Common.Services.Security;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using Microsoft.AspNetCore.Authorization;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Security.AuthorizationHandlers
{
    public class UpdateSpecificMethodologyRequirement : IAuthorizationRequirement
    {
    }

    public class UpdateSpecificMethodologyAuthorizationHandler
        : CompoundAuthorizationHandler<UpdateSpecificMethodologyRequirement, Methodology>
    {
        public UpdateSpecificMethodologyAuthorizationHandler(IMethodologyRepository methodologyRepository)
            : base(
                new CanUpdateAllSpecificMethodologies(),
                new HasNonPrereleaseRoleOnAnyAssociatedRelease(methodologyRepository)) {}
    }

    public class CanUpdateAllSpecificMethodologies
        : HasClaimAuthorizationHandler<UpdateSpecificMethodologyRequirement>
    {
        public CanUpdateAllSpecificMethodologies()
            : base(SecurityClaimTypes.UpdateAllMethodologies) {}
    }

    public class HasNonPrereleaseRoleOnAnyAssociatedRelease
        : AuthorizationHandler<UpdateSpecificMethodologyRequirement, Methodology>
    {
        private readonly IMethodologyRepository _methodologyRepository;

        public HasNonPrereleaseRoleOnAnyAssociatedRelease(
            IMethodologyRepository methodologyRepository)
        {
            _methodologyRepository = methodologyRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext authContext,
            UpdateSpecificMethodologyRequirement requirement,
            Methodology methodology)
        {
            if (await _methodologyRepository.UserHasReleaseRoleAssociatedWithMethodology(
                authContext.User.GetUserId(),
                methodology.Id))
            {
                authContext.Succeed(requirement);
            }
        }
    }
}
