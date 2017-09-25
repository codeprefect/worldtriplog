CREATE TABLE [Trips] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [Name] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    CONSTRAINT [PK_Trips] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Stops] (
    [Id] int NOT NULL IDENTITY,
    [Arrival] datetime2 NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
    [Name] nvarchar(max) NULL,
    [Order] int NOT NULL,
    [TripId] int NULL,
    CONSTRAINT [PK_Stops] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stops_Trips_TripId] FOREIGN KEY ([TripId]) REFERENCES [Trips] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Stops_TripId] ON [Stops] ([TripId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170921124449_AddedTripAndStopModels', N'2.0.0-rtm-26452');

GO

