CREATE VIEW dbo.View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile
AS
SELECT        dbo.Files.FileID, dbo.Files.ParentFileID, dbo.Files.UserIDOfFileOwner, dbo.Files.UserIDOfLastUploaded, dbo.LookupTable_FileStatuses.FileStatus AS CurrentFileStatus, dbo.Files.FileLookupStatusID, 
                         dbo.LookupTable_ShareStatues.FileShareStatus AS CurrentFileShareStatus, dbo.Files.FileShareStatusID, dbo.Files.ContentType, dbo.Files.FileName, dbo.Files.FileExtension, 
                         dbo.Files.FileName + '.' + dbo.Files.FileExtension AS FullFileName, dbo.Files.FileSize, dbo.Files.CurrentVersionNumber, dbo.Files.DateCreated, 
                         dbo.AspNetUsers.FirstName + ' ' + dbo.AspNetUsers.LastName AS NameOfFileOwner, AspNetUsers_1.FirstName + ' ' + AspNetUsers_1.LastName AS NameOfUserThatLastUpdatedFile, '' AS FileType, 
                         '' AS ListOfUserIDThatTheFileISsharedWith
FROM            dbo.Files INNER JOIN
                         dbo.LookupTable_FileStatuses ON dbo.Files.FileLookupStatusID = dbo.LookupTable_FileStatuses.FileLookupStatusID INNER JOIN
                         dbo.LookupTable_ShareStatues ON dbo.Files.FileShareStatusID = dbo.LookupTable_ShareStatues.FileShareStatusID INNER JOIN
                         dbo.AspNetUsers ON dbo.Files.UserIDOfFileOwner = dbo.AspNetUsers.Id INNER JOIN
                         dbo.AspNetUsers AS AspNetUsers_1 ON dbo.Files.UserIDOfLastUploaded = AspNetUsers_1.Id
GO
