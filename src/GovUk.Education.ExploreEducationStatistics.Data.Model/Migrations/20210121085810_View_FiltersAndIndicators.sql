CREATE OR ALTER VIEW FiltersAndIndicators
AS
    SELECT R.Id                        As ReleaseId,
           CONCAT(P.Slug, '/', R.Slug) AS Url,
           S.Id                        AS SubjectId,
           S.Name                      AS SubjectName,
           F.Id                        AS FilterId,
           F.Label                     AS Filter,
           FG.Id                       AS GroupId,
           FG.Label                    AS GroupLabel,
           FI.Id                       AS ItemId,
           FI.Label                    AS ItemLabel
    FROM Subject S
             JOIN Filter F ON S.Id = F.SubjectId
             JOIN FilterGroup FG ON F.Id = FG.FilterId
             JOIN FilterItem FI ON FG.Id = FI.FilterGroupId
             JOIN ReleaseSubject RS on S.Id = RS.SubjectId
             JOIN Release R on RS.ReleaseId = R.Id
             JOIN Publication P on R.PublicationId = P.Id
    UNION
    SELECT R.Id                        As ReleaseId,
           CONCAT(P.Slug, '/', R.Slug) AS Url,
           S.Id                        AS SubjectId,
           S.Name                      AS SubjectName,
           NULL                        AS FilterId,
           NULL                        AS Filter,
           IG.Id                       AS GroupId,
           IG.Label                    AS GroupLabel,
           I.Id                        AS ItemId,
           I.Label                     AS ItemLabel
    FROM Subject S
             JOIN IndicatorGroup IG ON S.Id = IG.SubjectId
             JOIN Indicator I ON IG.Id = I.IndicatorGroupId
             JOIN ReleaseSubject RS on S.Id = RS.SubjectId
             JOIN Release R on RS.ReleaseId = R.Id
             JOIN Publication P on R.PublicationId = P.Id