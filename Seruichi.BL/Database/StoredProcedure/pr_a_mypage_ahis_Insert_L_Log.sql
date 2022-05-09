IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_ahis_Insert_L_Log')
BEGIN
    DROP PROCEDURE [pr_a_mypage_ahis_Insert_L_Log]
END
GO
Create PROCEDURE [dbo].[pr_a_mypage_ahis_Insert_L_Log]
	 @LogDateTime   datetime         
    ,@LoginKBN      tinyint
    ,@LoginID       varchar(10)      
    ,@RealECD       varchar(10)      
    ,@LoginName     varchar(50)      
    ,@IPAddress     varchar(20)
    ,@PageID        varchar(100)
    ,@Processing    varchar(30)
    ,@Remarks       varchar(100)
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
        ,@Processing
        ,@Remarks
    )
END