CREATE TYPE LocationType AS TABLE
(
    Id                              UNIQUEIDENTIFIER NOT NULL,
    Country_Code                    VARCHAR(MAX),
    Country_Name                    VARCHAR(MAX),
    Institution_Code                VARCHAR(MAX),
    Institution_Name                VARCHAR(MAX),
    LocalAuthority_Code             VARCHAR(MAX),
    LocalAuthority_OldCode          VARCHAR(MAX),
    LocalAuthority_Name             VARCHAR(MAX),
    LocalAuthorityDistrict_Code     VARCHAR(MAX),
    LocalAuthorityDistrict_Name     VARCHAR(MAX),
    LocalEnterprisePartnership_Code VARCHAR(MAX),
    LocalEnterprisePartnership_Name VARCHAR(MAX),
    MayoralCombinedAuthority_Code   VARCHAR(MAX),
    MayoralCombinedAuthority_Name   VARCHAR(MAX),
    MultiAcademyTrust_Code          VARCHAR(MAX),
    MultiAcademyTrust_Name          VARCHAR(MAX),
    OpportunityArea_Code            VARCHAR(MAX),
    OpportunityArea_Name            VARCHAR(MAX),
    ParliamentaryConstituency_Code  VARCHAR(MAX),
    ParliamentaryConstituency_Name  VARCHAR(MAX),
    Region_Code                     VARCHAR(MAX),
    Region_Name                     VARCHAR(MAX),
    RscRegion_Code                  VARCHAR(MAX),
    Sponsor_Code                    VARCHAR(MAX),
    Sponsor_Name                    VARCHAR(MAX),
    Ward_Code                       VARCHAR(MAX),
    Ward_Name                       VARCHAR(MAX)
);