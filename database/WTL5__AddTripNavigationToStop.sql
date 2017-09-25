ALTER TABLE [Stops] DROP CONSTRAINT [FK_Stops_Trips_TripId];

GO

EXEC sp_rename N'Stops.TripId', N'TripID', N'COLUMN';

GO

EXEC sp_rename N'Stops.IX_Stops_TripId', N'IX_Stops_TripID', N'INDEX';

GO

DROP INDEX [IX_Stops_TripID] ON [Stops];
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Stops') AND [c].[name] = N'TripID');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Stops] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Stops] ALTER COLUMN [TripID] int NOT NULL;
CREATE INDEX [IX_Stops_TripID] ON [Stops] ([TripID]);

GO

ALTER TABLE [Stops] ADD CONSTRAINT [FK_Stops_Trips_TripID] FOREIGN KEY ([TripID]) REFERENCES [Trips] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170925115921_AddTripNavigationToStop', N'2.0.0-rtm-26452');

GO

