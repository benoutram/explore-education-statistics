CREATE PROCEDURE UpsertLocation @Location dbo.LocationType READONLY
AS
BEGIN
    MERGE dbo.Location AS target
    USING @Location AS source
    ON (target.Id = source.Id)
    WHEN MATCHED THEN
        UPDATE
        SET Country_Code                    = source.Country_Code,
            Country_Name                    = source.Country_Name,
            Institution_Code                = source.Institution_Code,
            Institution_Name                = source.Institution_Name,
            LocalAuthority_Code             = source.LocalAuthority_Code,
            LocalAuthority_OldCode          = source.LocalAuthority_OldCode,
            LocalAuthority_Name             = source.LocalAuthority_Name,
            LocalAuthorityDistrict_Code     = source.LocalAuthorityDistrict_Code,
            LocalAuthorityDistrict_Name     = source.LocalAuthorityDistrict_Name,
            LocalEnterprisePartnership_Code = source.LocalEnterprisePartnership_Code,
            LocalEnterprisePartnership_Name = source.LocalEnterprisePartnership_Name,
            MayoralCombinedAuthority_Code   = source.MayoralCombinedAuthority_Code,
            MayoralCombinedAuthority_Name   = source.MayoralCombinedAuthority_Name,
            MultiAcademyTrust_Code          = source.MultiAcademyTrust_Code,
            MultiAcademyTrust_Name          = source.MultiAcademyTrust_Name,
            OpportunityArea_Code            = source.OpportunityArea_Code,
            OpportunityArea_Name            = source.OpportunityArea_Name,
            ParliamentaryConstituency_Code  = source.ParliamentaryConstituency_Code,
            ParliamentaryConstituency_Name  = source.ParliamentaryConstituency_Name,
            Region_Code                     = source.Region_Code,
            Region_Name                     = source.Region_Name,
            RscRegion_Code                  = source.RscRegion_Code,
            Sponsor_Code                    = source.Sponsor_Code,
            Sponsor_Name                    = source.Sponsor_Name,
            Ward_Code                       = source.Ward_Code,
            Ward_Name                       = source.Ward_Name

    WHEN NOT MATCHED THEN
        INSERT (Id,
                Country_Code,
                Country_Name,
                Institution_Code,
                Institution_Name,
                LocalAuthority_Code,
                LocalAuthority_OldCode,
                LocalAuthority_Name,
                LocalAuthorityDistrict_Code,
                LocalAuthorityDistrict_Name,
                LocalEnterprisePartnership_Code,
                LocalEnterprisePartnership_Name,
                MayoralCombinedAuthority_Code,
                MayoralCombinedAuthority_Name,
                MultiAcademyTrust_Code,
                MultiAcademyTrust_Name,
                OpportunityArea_Code,
                OpportunityArea_Name,
                ParliamentaryConstituency_Code,
                ParliamentaryConstituency_Name,
                Region_Code,
                Region_Name,
                RscRegion_Code,
                Sponsor_Code)
        VALUES (source.Id,
                source.Country_Code,
                source.Country_Name,
                source.Institution_Code,
                source.Institution_Name,
                source.LocalAuthority_Code,
                source.LocalAuthority_OldCode,
                source.LocalAuthority_Name,
                source.LocalAuthorityDistrict_Code,
                source.LocalAuthorityDistrict_Name,
                source.LocalEnterprisePartnership_Code,
                source.LocalEnterprisePartnership_Name,
                source.MayoralCombinedAuthority_Code,
                source.MayoralCombinedAuthority_Name,
                source.MultiAcademyTrust_Code,
                source.MultiAcademyTrust_Name,
                source.OpportunityArea_Code,
                source.OpportunityArea_Name,
                source.ParliamentaryConstituency_Code,
                source.ParliamentaryConstituency_Name,
                source.Region_Code,
                source.Region_Name,
                source.RscRegion_Code,
                source.Sponsor_Code);
END
GO

                  