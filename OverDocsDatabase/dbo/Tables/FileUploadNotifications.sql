CREATE TABLE [dbo].[FileUploadNotifications] (
    [NotificationID] INT NOT NULL,
    CONSTRAINT [PK_FileUploadNotifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_FileUploadNotifications_Notifications] FOREIGN KEY ([NotificationID]) REFERENCES [dbo].[Notifications] ([NotificationID])
);

