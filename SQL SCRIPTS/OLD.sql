USE [WebDocs]
GO
/****** Object:  StoredProcedure [dbo].[PublicDocs_R_GetMostRecentFileVersion]    Script Date: 2018/10/07 1:07:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE  [dbo].[PublicDocs_R_GetMostRecentFileVersion]
--DROP Procedure PublicDocs_R_GetMostRecentFileVersion
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
				CREATE TABLE #MyTEmpTable
				(
						[FileID] int
					  ,[ParentFileID] int
					  ,[UserIDOfFileOwner] nvarchar(100)
					  ,[UserIDOfLastUploaded] nvarchar(100)
					  ,[CurrentFileStatus] varchar(50)
					  ,[FileLookupStatusID] int
					  ,[CurrentFileShareStatus] varchar(500)
					  ,[FileShareStatusID] int
					  ,[ContentType] varchar(500)
					  ,[FileName] varchar(500)
					  ,[FileExtension] varchar(10)
					  ,[FullFileName] varchar(500)
					  ,[FileSize] int
					  ,[CurrentVersionNumber] int
					  ,[DateCreated] datetime
					  ,[NameOfFileOwner] varchar(500)
					  ,[NameOfUserThatLastUpdatedFile] varchar(500)
					  ,[ListOfUserIDThatTheFileISsharedWith] varchar(MAX) --10|32|21|43
					  ,ComponentLevel int
				);

----Start
DECLARE @FileID int 


				DECLARE AllMostRecentVersionOfFile_Cursor CURSOR FOR   
				SELECT
						[FileID]
					FROM View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile
					where [ParentFileID]  = 0; 

				OPEN AllMostRecentVersionOfFile_Cursor  

				FETCH NEXT FROM AllMostRecentVersionOfFile_Cursor   
				INTO @FileID  

				WHILE @@FETCH_STATUS = 0  
				BEGIN  
													----START OF CURSOR 

													WITH LastFileUpdated([FileID]
													  ,[ParentFileID]
													  ,[UserIDOfFileOwner]
													  ,[UserIDOfLastUploaded]
													  ,[CurrentFileStatus]
													  ,[FileLookupStatusID]
													  ,[CurrentFileShareStatus]
													  ,[FileShareStatusID]
													  ,[ContentType]
													  ,[FileName]
													  ,[FileExtension]
													  ,[FullFileName]
													  ,[FileSize]
													  ,[CurrentVersionNumber]
													  ,[DateCreated]
													  ,[NameOfFileOwner]
													  ,[NameOfUserThatLastUpdatedFile],
													  [ListOfUserIDThatTheFileISsharedWith],
													   ComponentLevel) AS
												(
													SELECT
	
	 

														b.[FileID]
													  ,b.[ParentFileID]
													  ,b.[UserIDOfFileOwner]
													  ,b.[UserIDOfLastUploaded]
													  ,b.[CurrentFileStatus]
													  ,b.[FileLookupStatusID]
													  ,b.[CurrentFileShareStatus]
													  ,b.[FileShareStatusID]
													  ,b.[ContentType]
													  ,b.[FileName]
													  ,b.[FileExtension]
													  ,b.[FullFileName]
													  ,b.[FileSize]
													  ,b.[CurrentVersionNumber]
													  ,b.[DateCreated]
													  ,b.[NameOfFileOwner]
													  ,b.[NameOfUserThatLastUpdatedFile],
														'' AS [ListOfUserIDThatTheFileISsharedWith],
														0 AS ComponentLevel
    
	
													FROM View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile AS b
													--Change to where PArentid = 0 just now
												   WHERE b.Fileid = @FileID
         
													UNION ALL
													SELECT 
	
													bom.[FileID]
													  ,bom.[ParentFileID]
													  ,bom.[UserIDOfFileOwner]
													  ,bom.[UserIDOfLastUploaded]
													  ,bom.[CurrentFileStatus]

													  ,bom.[FileLookupStatusID]
													  ,bom.[CurrentFileShareStatus]
													  ,bom.[FileShareStatusID]
													  ,bom.[ContentType]
													  ,bom.[FileName]
													  ,bom.[FileExtension]
													  ,bom.[FullFileName]
													  ,bom.[FileSize]
													  ,bom.[CurrentVersionNumber]
													  ,bom.[DateCreated]
													  ,bom.[NameOfFileOwner]
													  ,bom.[NameOfUserThatLastUpdatedFile],
													  '' AS [ListOfUserIDThatTheFileISsharedWith],
														ComponentLevel + 1
													FROM View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile AS bom 
														INNER JOIN LastFileUpdated AS p
														ON bom.[ParentFileID] = p.[FileID]
       
												)
												insert into #MyTEmpTable
												SELECT top 1 [FileID]
													  ,[ParentFileID]
													  ,[UserIDOfFileOwner]
													  ,[UserIDOfLastUploaded]
													  ,[CurrentFileStatus]
													  ,[FileLookupStatusID]
													  ,[CurrentFileShareStatus]
													  ,[FileShareStatusID]
													  ,[ContentType]
													  ,[FileName]
													  ,[FileExtension]
													  ,[FullFileName]
													  ,[FileSize]
													  ,[CurrentVersionNumber]
													  ,[DateCreated]
													  ,[NameOfFileOwner]
													  ,[NameOfUserThatLastUpdatedFile],
														ComponentLevel 
												FROM LastFileUpdated AS p
													--INNER JOIN Production.Product AS pr
													--ON p.<component_id, sysname, ComponentID> = pr.ProductID
												ORDER BY ComponentLevel desc,[FileID],[ParentFileID];



				--END OF COURsOR
					FETCH NEXT FROM AllMostRecentVersionOfFile_Cursor 
					INTO @FileID  
				END   
				CLOSE AllMostRecentVersionOfFile_Cursor;  
				DEALLOCATE AllMostRecentVersionOfFile_Cursor;  
				--------------------------------
				--end of section
				-----------------------------------

 Create table #TempOfAllSharedUsersID(
		UserID Varchar(500)
 );


	-------------------------------------------------
	--Find All Users that the file is shared with for each file
	--------------------------------------------------

	Declare @FileIDToDetermineUseredUser int;

DECLARE UserIDOfTheUsersThatTheFileIsSharedWith_Cursor CURSOR FOR   
SELECT
		[FileID]
	FROM #MyTEmpTable; 

OPEN UserIDOfTheUsersThatTheFileIsSharedWith_Cursor  

FETCH NEXT FROM UserIDOfTheUsersThatTheFileIsSharedWith_Cursor   
INTO @FileIDToDetermineUseredUser  

WHILE @@FETCH_STATUS = 0  
BEGIN  
    ----START OF CURSOR 

	insert into #TempOfAllSharedUsersID
   SELECT [UserIDOfSharedDocs]
  FROM [FileSharedWithUsers]
  where @FileIDToDetermineUseredUser = [FileID];

  ----------------------------------------------------------
  --Beginning of inner loop
							  Declare @InnerLoop_UserID varchar(500);
							  Declare @InnerLoop_ListOfUserID varchar(max);

							  DECLARE UserIdOfSharedDocs_Cursor CURSOR FOR   
								SELECT
									UserID
								FROM #TempOfAllSharedUsersID; 

							OPEN UserIdOfSharedDocs_Cursor  

							FETCH NEXT FROM UserIdOfSharedDocs_Cursor   
							INTO @InnerLoop_UserID  

							WHILE @@FETCH_STATUS = 0  
							BEGIN  
							---------------begin
							Declare @LentghtOfUSERIDSet int;

							select @LentghtOfUSERIDSet = Len(@InnerLoop_ListOfUserID);

							IF @LentghtOfUSERIDSet > 0
							BEGIN
									SET @InnerLoop_ListOfUserID = @InnerLoop_ListOfUserID + '|';
							END

								SET @InnerLoop_ListOfUserID = @InnerLoop_ListOfUserID + @InnerLoop_UserID;

							--END OF COURsOR
								FETCH NEXT FROM UserIdOfSharedDocs_Cursor 
								INTO @InnerLoop_UserID  
							END   
							CLOSE UserIdOfSharedDocs_Cursor;  
							DEALLOCATE UserIdOfSharedDocs_Cursor;



							---------------------
							--Update The Main Table with the value
							---------

							Update #MyTEmpTable
							SET [ListOfUserIDThatTheFileISsharedWith] = @InnerLoop_ListOfUserID
							where @FileIDToDetermineUseredUser = FileID;
  ------------------------------------------------
  --endofINNER loop

	--END OF COURsOR
    FETCH NEXT FROM UserIDOfTheUsersThatTheFileIsSharedWith_Cursor 
	INTO @FileIDToDetermineUseredUser  
END   
CLOSE UserIDOfTheUsersThatTheFileIsSharedWith_Cursor;  
DEALLOCATE UserIDOfTheUsersThatTheFileIsSharedWith_Cursor; 


select [FileID]
      ,[ParentFileID]
      ,[UserIDOfFileOwner]
      ,[UserIDOfLastUploaded]
      ,[CurrentFileStatus]
      ,[FileLookupStatusID]
      ,[CurrentFileShareStatus]
      ,[FileShareStatusID]
      ,[ContentType]
      ,[FileName]
      ,[FileExtension]
      ,[FullFileName]
      ,[FileSize]
      ,[CurrentVersionNumber]
      ,[DateCreated]
      ,[NameOfFileOwner]
      ,[NameOfUserThatLastUpdatedFile],
		'' AS FileType
	   from #MyTEmpTable
	   where [CurrentFileShareStatus] != 'Hidden';
---End

DROP TABLE #MyTEmpTable;
END
