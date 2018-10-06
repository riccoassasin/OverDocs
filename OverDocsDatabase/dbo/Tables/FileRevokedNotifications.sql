CREATE TABLE [dbo].[FileRevokedNotifications] (
    [NotificationID] INT NOT NULL,
    CONSTRAINT [PK_FileRevokedNotifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_FileRevokedNotifications_Notifications] FOREIGN KEY ([NotificationID]) REFERENCES [dbo].[Notifications] ([NotificationID])
);

