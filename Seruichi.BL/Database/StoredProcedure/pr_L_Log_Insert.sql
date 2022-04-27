IF EXISTS (select * from sys.objects where name = 'pr_L_Log_Insert')
BEGIN
    DROP PROCEDURE [pr_L_Log_Insert]
END
GO

CREATE PROCEDURE [dbo].[pr_L_Log_Insert]
(
     @LogDateTime   datetime        = NULL
    ,@LoginKBN      tinyint
    ,@LoginID       varchar(10)     = NULL
    ,@RealECD       varchar(10)     = NULL
    ,@LoginName     varchar(50)     = NULL
    ,@IPAddress     varchar(20)
    ,@PageID        varchar(100)
    ,@ProcessKBN     tinyint         --1:INSERT 2:UPDATE 3:DELETE 4:Mail 
    ,@Remarks       varchar(100)
)
AS
BEGIN
    INSERT INTO L_Log
    (
        LogDateTime
        ,LoginKBN
        ,LoginID
        ,RealECD
        ,LoginName
        ,IPAddress
        ,Page
        ,Processing
        ,Remarks
    )
    VALUES
    (
        ISNULL(@LogDateTime, GETDATE())
        ,@LoginKBN
        ,@LoginID
        ,@RealECD
        ,@LoginName
        ,@IPAddress
        ,@PageID
        ,CASE @ProcessKBN WHEN 1 THEN 'INSERT' WHEN 2 THEN 'UPDATE' WHEN 3 THEN 'DELETE' WHEN 4 THEN 'MAIL' ELSE '' END
        ,@Remarks
    )
END