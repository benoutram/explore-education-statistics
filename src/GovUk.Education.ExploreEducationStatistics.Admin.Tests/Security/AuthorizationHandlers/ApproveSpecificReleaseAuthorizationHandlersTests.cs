using GovUk.Education.ExploreEducationStatistics.Admin.Security.AuthorizationHandlers;
using GovUk.Education.ExploreEducationStatistics.Content.Model;
using Xunit;
using static GovUk.Education.ExploreEducationStatistics.Admin.Security.SecurityClaimTypes;
using static GovUk.Education.ExploreEducationStatistics.Admin.Tests.Services.PermissionTestUtil;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Tests.Security.AuthorizationHandlers
{
    public class ApproveSpecificReleaseAuthorizationHandlersTests
    {
        [Fact]
        public void ApproveSpecificReleaseCanApproveAllReleasesAuthorizationHandler()
        {
            AssertHandlerSucceedsWithCorrectClaims(
                new ApproveSpecificReleaseCanApproveAllReleasesAuthorizationHandler(), ApproveAllReleases);
        }
        
        [Fact]
        public void ApproveSpecificReleaseHasApproverRoleOnReleaseAuthorizationHandler()
        {
            AssertHandlerSucceedsWithCorrectReleaseRoles(
                contentDbContext => new ApproveSpecificReleaseHasApproverRoleOnReleaseAuthorizationHandler(contentDbContext), 
                ReleaseRole.Approver);
        }
    }
}