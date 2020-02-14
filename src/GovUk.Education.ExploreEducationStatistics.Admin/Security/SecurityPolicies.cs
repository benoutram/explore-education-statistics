namespace GovUk.Education.ExploreEducationStatistics.Admin.Security
{
    public enum SecurityPolicies
    {
        CanAccessSystem,
        CanAccessAnalystPages,
        CanAccessPrereleasePages,

        CanManageUsersOnSystem,
        
        CanManageMethodologiesOnSystem,
        CanUpdateSpecificMethodology,

        CanViewAllTopics,
        CanViewSpecificTheme,

        CanCreatePublicationForSpecificTopic,
        CanCreateReleaseForSpecificPublication,
        
        CanViewSpecificPublication,
        
        CanViewAllReleases,
        CanViewSpecificRelease,
        CanUpdateSpecificRelease,
        
        CanMarkSpecificReleaseAsDraft,
        CanSubmitSpecificReleaseToHigherReview,
        CanApproveSpecificRelease,
        
        CanViewPrereleaseContacts,
        CanAssignPrereleaseContactsToSpecificRelease
    }
}