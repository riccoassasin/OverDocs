CREATE TABLE [dbo].[FileRequestNotifications] (
    [NotificationID] INT NOT NULL,
    CONSTRAINT [PK_FileRequestNotifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_FileRequestNotifications_Notifications] FOREIGN KEY ([NotificationID]) REFERENCES [dbo].[Notifications] ([NotificationID])
);

