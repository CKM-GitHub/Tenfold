IF EXISTS (select * from sys.objects where name = 'pr_t_seller_mansion_list_Insert_L_Log')
BEGIN
    DROP PROCEDURE [pr_t_seller_mansion_list_Insert_L_Log]
END
GO
CREATE PROCEDURE [dbo].[pr_t_seller_mansion_list_Insert_L_Log]
	 @LogDateTime   datetime        = NULL
    ,@LoginKBN      tinyint
    ,@LoginID       varchar(10)     = NULL
    ,@RealECD       varchar(10)     = NULL
    ,@LoginName     varchar(50)     = NULL
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