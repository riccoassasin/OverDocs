CREATE TABLE [dbo].[FileDownloadLogs] (
    [FileDownloadLogID] INT            IDENTITY (1, 1) NOT NULL,
    [FileID]            INT            NOT NULL,
    [Id]                NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_FileDownloadLogs] PRIMARY KEY CLUSTERED ([FileDownloadLogID] ASC),
    CONSTRAINT [FK_FileDownloadLogs_AspNetUsers] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_FileDownloadLogs_Files] FOREIGN KEY ([FileID]) REFERENCES [dbo].[Files] ([FileID])
);

