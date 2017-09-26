ALTER TABLE [Trips] ADD [Deleted] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Stops] ADD [Deleted] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170926143249_ChangedEntityDeletionMethod', N'2.0.0-rtm-26452');

GO

