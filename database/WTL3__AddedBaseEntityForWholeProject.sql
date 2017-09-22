ALTER TABLE [Trips] ADD [CreatedBy] nvarchar(max) NULL;

GO

ALTER TABLE [Trips] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.000';

GO

ALTER TABLE [Trips] ADD [ModifiedBy] nvarchar(max) NULL;

GO

ALTER TABLE [Trips] ADD [ModifiedDate] datetime2 NULL;

GO

ALTER TABLE [Trips] ADD [Version] rowversion NULL;

GO

ALTER TABLE [Stops] ADD [CreatedBy] nvarchar(max) NULL;

GO

ALTER TABLE [Stops] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.000';

GO

ALTER TABLE [Stops] ADD [ModifiedBy] nvarchar(max) NULL;

GO

ALTER TABLE [Stops] ADD [ModifiedDate] datetime2 NULL;

GO

ALTER TABLE [Stops] ADD [Version] rowversion NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170922140956_AddedBaseEntityForWholeProject', N'2.0.0-rtm-26452');

GO

