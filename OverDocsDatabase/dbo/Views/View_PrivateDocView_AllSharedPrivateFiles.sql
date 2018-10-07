CREATE VIEW dbo.View_PrivateDocView_AllSharedPrivateFiles
AS
SELECT        dbo.Files.FileID, dbo.Files.ParentFileID, dbo.Files.UserIDOfFileOwner, dbo.Files.UserIDOfLastUploaded, dbo.Files.FileLookupStatusID, dbo.Files.FileShareStatusID, dbo.Files.ContentType, dbo.Files.FileName, 
                         dbo.Files.FileExtension, dbo.Files.FileSize, dbo.Files.CurrentVersionNumber, dbo.Files.DateCreated, dbo.LookupTable_FileStatuses.FileStatus, 
                         dbo.AspNetUsers.FirstName + ' ' + dbo.AspNetUsers.LastName AS NameOfFileOwner, AspNetUsers_1.FirstName + ' ' + AspNetUsers_1.LastName AS UserThatLastUpdatedFile, 
                         dbo.FileSharedWithUsers.UserIDOfSharedDocs AS RefID, dbo.Files.FileName + '.' + dbo.Files.FileExtension AS FullFileName, '' AS FileType
FROM            dbo.Files INNER JOIN
                         dbo.FileSharedWithUsers ON dbo.Files.FileID = dbo.FileSharedWithUsers.FileID INNER JOIN
                         dbo.LookupTable_FileStatuses ON dbo.Files.FileLookupStatusID = dbo.LookupTable_FileStatuses.FileLookupStatusID INNER JOIN
                         dbo.AspNetUsers ON dbo.Files.UserIDOfFileOwner = dbo.AspNetUsers.Id INNER JOIN
                         dbo.AspNetUsers AS AspNetUsers_1 ON dbo.Files.UserIDOfLastUploaded = AspNetUsers_1.Id
GO
