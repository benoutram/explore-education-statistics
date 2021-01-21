CREATE OR ALTER PROCEDURE DropAndCreateExternalStatsTable @tableName nvarchar(max),
                                                          @location nvarchar(max),
                                                          @columns nvarchar(max)
AS
BEGIN
    EXEC DropExternalTable @tableName

    DECLARE @columnDefinition nvarchar(max) = REPLACE(@columns, ',' ,'] NVARCHAR(100),[')

    -- Remove trailing comma
    --SET @columnDefinition = SUBSTRING(@columnDefinition, 1, (LEN(@columnDefinition)-1))

    -- Add type for the last column
    SET @columnDefinition = CONCAT('[', @columnDefinition, '] NVARCHAR(100)')

-- COULD CONCAT A LIST OF KNOWN FIXED COLUMNS WITH META FILE COLUMNS IF COLUMNS WERE IN SAME ORDER

--     DECLARE @columnDefinition nvarchar(max) = (SELECT CONCAT('time_identifier NVARCHAR(25),',
--                                           'year_breakdown NVARCHAR(25),',
--                                           'time_period NVARCHAR(25),',
--                                           'geographic_level NVARCHAR(25),',
--                                           'country_code NVARCHAR(25),',
--                                           'country_name NVARCHAR(25),',
--                                           'region_code NVARCHAR(25),',
--                                           'region_name NVARCHAR(25),',
--                                           'old_la_code NVARCHAR(25),',
--                                           'new_la_code NVARCHAR(25),',
--                                           'la_name NVARCHAR(50),',
--                                           'lad_code NVARCHAR(25),',
--                                           'lad_name NVARCHAR(50),',
--                                           STRING_AGG(QUOTENAME(Columns.ColumnName), ' NVARCHAR(100),'),
--                                           ' NVARCHAR(100)')
-- FROM (
--          SELECT col_name AS ColumnName
--          FROM absence_by_characteristic_meta_external
--          WHERE col_type IN ('Filter', 'Indicator')
--          UNION
--          (SELECT filter_grouping_column AS ColumnName
--           from absence_by_characteristic_meta_bentest
--           WHERE col_type = 'Filter'
--             AND filter_grouping_column IS NOT NULL)) Columns)

    DECLARE @sql nvarchar(max) = 'CREATE EXTERNAL TABLE ' + QUOTENAME(@tableName) + ' (' + @columnDefinition + ')' +
               'WITH (' +
               '    DATA_SOURCE = MyHadoopDs,' +
               '    LOCATION = ''' + @location + ''',' +
               '    FILE_FORMAT= csv_file)';

    EXEC sp_executesql @sql
END