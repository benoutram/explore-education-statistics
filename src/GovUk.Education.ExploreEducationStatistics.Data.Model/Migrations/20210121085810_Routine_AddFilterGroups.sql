CREATE OR ALTER PROCEDURE AddFilterGroups @subjectId UNIQUEIDENTIFIER,
                                          @statsTableName nvarchar(max),
                                          @metaTableName nvarchar(max)
AS
BEGIN
    -- GET THE FILTER GROUPING COLUMNS
    DECLARE @filterGroupColumns TABLE(FilterId UNIQUEIDENTIFIER, FilterColumn NVARCHAR(max), GroupColumn NVARCHAR(MAX));
    DECLARE @sql NVARCHAR(MAX) = 'SELECT Filter.Id AS FilterId,' +
       'Filter.Name AS FilterColumn,' +
       'filter_grouping_column AS GroupColumn ' +
       'FROM ' + QUOTENAME(@metaTableName) + ' Meta ' +
       'JOIN Filter ON Meta.col_name = Filter.Name AND Meta.col_type = ''Filter'' ' +
       'WHERE Filter.SubjectId = @subjectId'

    DECLARE @paramDefinition NVARCHAR(MAX) = N'@subjectId UNIQUEIDENTIFIER'
    INSERT INTO @filterGroupColumns (FilterId, FilterColumn, GroupColumn)
    EXEC sp_executesql @sql, @paramDefinition, @subjectId = @subjectId

    -- GET THE FILTER ITEMS
    DECLARE @filterItems TABLE(FilterId UNIQUEIDENTIFIER, Item NVARCHAR(MAX), GroupLabel NVARCHAR(MAX));

    SET @sql = 'SELECT DISTINCT x.* ' +
               'FROM ' + @statsTableName + ' ' +
               'CROSS APPLY ( ' +
               'VALUES '
    SET @sql += (SELECT STRING_AGG(CONCAT('({guid''', FilterId, '''},', QUOTENAME(FilterColumn), ',', ISNULL(QUOTENAME(GroupColumn), '''Default'''), ')'), ',') FROM @filterGroupColumns)
    SET @sql += ') x (FilterId, Item, GroupLabel)'
    INSERT INTO @filterItems (FilterId, Item, GroupLabel)
    EXEC sp_executesql @sql

    --INSERT THE FILTER GROUPS
    INSERT INTO FilterGroup (Id, FilterId, Label)
    SELECT NEWID(),
           FilterGroups.FilterId,
           FilterGroups.GroupLabel
    FROM (SELECT DISTINCT FilterId, GroupLabel FROM @filterItems) FilterGroups

    -- INSERT THE FILTER ITEMS FOR ALL THE GROUPS
    INSERT INTO FilterItem (Id, FilterGroupId, Label)
    SELECT NEWID(),
           FilterItems.FilterGroupId,
           FilterItems.FilterItemLabel
    FROM (SELECT DISTINCT FilterGroup.Id  AS FilterGroupId,
                          InnerItems.Item AS FilterItemLabel
          FROM @filterItems InnerItems
                   JOIN FilterGroup ON InnerItems.FilterId = FilterGroup.FilterId AND
                                       InnerItems.GroupLabel = FilterGroup.Label) FilterItems
END