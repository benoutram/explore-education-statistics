CREATE OR ALTER PROCEDURE DropAndCreateExternalMetaTable @tableName nvarchar(max),
                                                         @location nvarchar(max)
AS
BEGIN
    EXEC DropExternalTable @tableName
    DECLARE @sql nvarchar(max) = 'CREATE EXTERNAL TABLE ' + QUOTENAME(@tableName) + ' (' +
               'col_name               NVARCHAR(50),' +
               'col_type               NVARCHAR(50),' +
               '[label]                NVARCHAR(75),' +
               'indicator_grouping     NVARCHAR(50),' +
               'indicator_unit         NVARCHAR(1),' +
               'filter_hint            NVARCHAR(50),' +
               'filter_grouping_column NVARCHAR(50),' +
               'indicator_dp           NVARCHAR(1))' +
               'WITH (' +
               '    DATA_SOURCE = MyHadoopDs,' +
               '    LOCATION = ''' + @location + ''',' +
               '    FILE_FORMAT= csv_file)';

    EXEC sp_executesql @sql
END