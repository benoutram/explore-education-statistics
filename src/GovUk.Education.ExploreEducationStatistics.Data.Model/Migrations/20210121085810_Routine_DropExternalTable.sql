CREATE OR ALTER PROCEDURE DropExternalTable @tableName nvarchar(max)
AS
DECLARE
    @sql nvarchar(max) =
            'IF EXISTS (SELECT * FROM sys.external_tables WHERE object_id = OBJECT_ID(QUOTENAME(@tableName)))' +
            '   DROP EXTERNAL TABLE ' + QUOTENAME(@tableName);

    DECLARE @paramDefinition nvarchar(max) = N'@tableName nvarchar(max)'
    EXEC sp_executesql @sql, @paramDefinition, @tableName = @tableName