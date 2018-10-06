CREATE TABLE [dbo].[Notifications] (
    [NotificationID] INT      IDENTITY (1, 1) NOT NULL,
    [DateCreated]    DATETIME NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC)
);

