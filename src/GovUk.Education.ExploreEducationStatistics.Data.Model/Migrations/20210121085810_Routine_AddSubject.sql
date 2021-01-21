CREATE OR ALTER PROCEDURE AddSubject @releaseId UNIQUEIDENTIFIER,
                         @subjectLabel NVARCHAR(MAX),
                         @statsFileColumns NVARCHAR(MAX),
                         @statsFileLocation NVARCHAR(MAX),
                         @metaFileLocation NVARCHAR(MAX)
AS
BEGIN
    -- DELETE EXISTING SUBJECT IF IT ALREADY EXISTS (CASCADE DELETES ALL RELATED ENTITIES)
	DECLARE @existingSubjectId uniqueidentifier = (SELECT SubjectId FROM ReleaseSubject WHERE ReleaseId = @releaseId);
    DELETE FROM ReleaseSubject WHERE ReleaseId = @releaseId AND SubjectId = @existingSubjectId;
	DELETE FROM Subject WHERE Id = @existingSubjectId;

    -- CREATE SUBJECT AND LINK IT WITH RELEASE
    DECLARE @filename NVARCHAR(max) = RIGHT(@statsFileLocation, CHARINDEX('/', REVERSE(@statsFileLocation), 0) -1);
    DECLARE @subjectId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Subject (Id, Name, Filename) VALUES (@subjectId, @subjectLabel, @filename);
    INSERT INTO ReleaseSubject (SubjectId, ReleaseId) VALUES (@subjectId, @releaseId);

    -- CREATE EXTERNAL TABLES
    DECLARE @metaTableName NVARCHAR(MAX) = REPLACE(@filename, '.csv', '_meta_external');
    DECLARE @statsTableName NVARCHAR(MAX) = REPLACE(@filename, '.csv', '_external');
    EXEC DropAndCreateExternalMetaTable @metaTableName, @metaFileLocation
    EXEC DropAndCreateExternalStatsTable @statsTableName, @statsFileLocation, @statsFileColumns

    -- ADD FILTERS
    DECLARE @sql NVARCHAR(max) = 'SELECT NEWID(),' +
           'filter_hint,' +
           '[label],' +
           'col_name,' +
           '@subjectId ' +
    'FROM ' + @metaTableName + ' ' +
    'WHERE col_type = ''Filter'''
    DECLARE @paramDefinition NVARCHAR(MAX) = N'@subjectId uniqueidentifier'
    INSERT INTO Filter (Id, Hint, Label, Name, SubjectId) EXEC sp_executesql @sql, @paramDefinition, @subjectId = @subjectId

    -- ADD FILTER GROUPS
	EXEC AddFilterGroups @subjectId, @statsTableName, @metaTableName

    -- ADD INDICATOR GROUPS
    SET @sql = 'SELECT NEWID(),' +
           'IndicatorGroups.indicator_grouping,' +
           '@subjectId ' +
    'FROM (SELECT DISTINCT indicator_grouping ' +
          'FROM ' + @metaTableName + ' ' +
          'WHERE col_type = ''Indicator'') IndicatorGroups'
    SET @paramDefinition = N'@subjectId uniqueidentifier'
    INSERT INTO IndicatorGroup (Id, Label, SubjectId)
    EXEC sp_executesql @sql, @paramDefinition, @subjectId = @subjectId

    -- ADD INDICATORS
    SET @sql = 'SELECT NEWID()          AS Id,' +
       'Meta.label                      AS Label,' +
       'Meta.col_name                   AS Name,' +
       'ISNULL(Meta.indicator_unit, '''') AS Unit,' +
       'IndicatorGroup.Id               AS IndicatorGroupId,' +
       --'ISNULL(NULLIF(indicator_dp, CHAR(13)),0)    AS DecimalPlaces ' + Azure BDC importing last column as carriage return?
       'ISNULL(Meta.indicator_dp, 0)    AS DecimalPlaces ' +
       'FROM ' + @metaTableName + ' Meta '+
       '  JOIN IndicatorGroup ON Meta.indicator_grouping = IndicatorGroup.Label AND IndicatorGroup.SubjectId = @subjectId ' +
       'WHERE Meta.col_type = ''Indicator'''
    SET @paramDefinition = N'@subjectId uniqueidentifier'
    INSERT INTO Indicator (Id, Label, Name, Unit, IndicatorGroupId, DecimalPlaces)
    EXEC sp_executesql @sql, @paramDefinition, @subjectId = @subjectId

    -- DROP THE META TABLE
    EXEC DropExternalTable @metaTableName
END