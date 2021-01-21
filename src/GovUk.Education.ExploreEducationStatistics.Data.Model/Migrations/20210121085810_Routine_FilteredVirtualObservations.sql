CREATE OR ALTER PROCEDURE FilteredVirtualObservations
    @subjectId uniqueidentifier,
    @geographicLevel nvarchar(6) = NULL,
    @timePeriodList TimePeriodListType READONLY,
    @filterItemList IdListGuidType READONLY,
    @indicatorList IdListGuidType READONLY
AS
BEGIN
    DECLARE @tableName NVARCHAR(MAX) = (SELECT REPLACE(filename, '.csv', '_external') FROM Subject where Id = @subjectId);

    DECLARE @timePeriodCount INT = (SELECT count(year) FROM @timePeriodList);

    DECLARE @filterItems TABLE(filter NVARCHAR(MAX), items NVARCHAR(MAX));

    -- Turn the filter item parameter into a table of filters and a csv list of the filter item labels
    INSERT INTO @filterItems
    SELECT Filter.Name, STRING_AGG(QUOTENAME(FI.Label, ''''), ',')
    FROM Filter
             JOIN FilterGroup FG on Filter.Id = FG.FilterId
             JOIN FilterItem FI on FG.Id = FI.FilterGroupId
             JOIN @filterItemList FilterItems ON FI.Id = FilterItems.id
    GROUP BY Filter.Name;

    -- Build the requested filter items clause
    DECLARE @filterClause NVARCHAR(MAX) = (SELECT STRING_AGG(CONCAT(QUOTENAME(filter), ' IN (', items, ')'), ' AND ') FROM @filterItems);

    -- Build the list of all filter columns
    DECLARE @filterColumns NVARCHAR(MAX) = (SELECT STRING_AGG(CONCAT(QUOTENAME(name), ' AS ', QUOTENAME(Id)), ', ') FROM Filter WHERE SubjectId = @subjectId);

    -- Build the list of requested indicator columns
    DECLARE @indicatorColumns NVARCHAR(MAX) = (SELECT STRING_AGG(CONCAT(QUOTENAME(name), ' AS ', QUOTENAME(Indicator.Id)), ', ') FROM Indicator JOIN @indicatorList Indicators ON Indicator.Id = Indicators.id);

    -- Build the dynamic Select statement
    DECLARE @sqlString NVARCHAR(MAX)= N'SELECT CAST(SUBSTRING(o.time_period, 1, 4) AS INT) AS Year, ' +
                                'CASE ' +
                                '  WHEN time_identifier = ''Academic Year'' THEN ''AY'' ' +
                                'END AS TimeIdentifier, ' +
                                'CASE ' +
                                '  WHEN geographic_level = ''National'' THEN ''NAT'' ' +
                                '  WHEN geographic_level = ''Local Authority'' THEN ''LA'' ' +
                                '  WHEN geographic_level = ''Regional'' THEN ''REG'' ' +
                                'END AS GeographicLevel, ' +
                                + @filterColumns + ', '
                                + @indicatorColumns + ' ' +
                                'FROM ' + QUOTENAME(@tableName) + 'o ' +
                                'WHERE ' +
                                @filterClause

    IF (@geographicLevel IS NOT NULL)
        SET @sqlString += N' AND o.geographic_level = @geographicLevel '

    -- TODO reintroduce time identifier and location
    IF (@timePeriodCount > 0)
       SET @sqlString += N' AND EXISTS(SELECT 1 from @timePeriodList t WHERE t.year = CAST(SUBSTRING(o.time_period, 1, 4) AS INT))';-- AND t.timeIdentifier = o.time_identifier) ';

    -- Execute the statement
    DECLARE @paramDefinition NVARCHAR(MAX) = N'@geographicLevel nvarchar(6) = NULL,
                           @timePeriodList TimePeriodListType READONLY'
    EXEC sp_executesql @sqlString, @paramDefinition,
         @geographicLevel = @geographicLevel,
         @timePeriodList = @timePeriodList;
END