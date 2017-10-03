DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Trips') AND [c].[name] = N'Deleted');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Trips] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Trips] DROP COLUMN [Deleted];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Stops') AND [c].[name] = N'Deleted');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Stops] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Stops] DROP COLUMN [Deleted];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171003114101_RemoveDeleteBool', N'2.0.0-rtm-26452');

GO

ALTER TABLE [Trips] ADD [Deleted] datetime2 NULL;

GO

ALTER TABLE [Stops] ADD [Deleted] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171003114205_AddDeleteAsDateTime', N'2.0.0-rtm-26452');

GO

