-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[PublicDocs_R_GetMostRecentFileVersion]
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
	  ,ComponentLevel int
);

----Start
DECLARE @FileID int 


DECLARE vendor_cursor CURSOR FOR   
SELECT
		[FileID]
	FROM View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile
	where [ParentFileID]  = 0; 

OPEN vendor_cursor  

FETCH NEXT FROM vendor_cursor   
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
	   ComponentLevel) AS
(
    SELECT
	
	 --b.ProductAssemblyID, b.ComponentID, b.PerAssemblyQty,
       -- b.EndDate, 

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
	--bom.ProductAssemblyID, bom.ComponentID, p.PerAssemblyQty,
 --       bom.EndDate, 
		
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
ORDER BY ComponentLevel desc,[FileID],[ParentFileID]

--END OF COURsOR
    FETCH NEXT FROM vendor_cursor 
	INTO @FileID  
END   
CLOSE vendor_cursor;  
DEALLOCATE vendor_cursor;  


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
	   from #MyTEmpTable;
---End

DROP TABLE #MyTEmpTable;
END