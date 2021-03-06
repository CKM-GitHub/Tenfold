IF EXISTS (select * from sys.objects where name = 'pr_r_login_Insert_L_Login')
BEGIN
    DROP PROCEDURE [pr_r_login_Insert_L_Login]
END
GO
CREATE PROCEDURE [dbo].[pr_r_login_Insert_L_Login]
	 @REStaffCD               varchar(10)
	,@RealECD				  varchar(10)
    ,@LoginName               varchar(50)
    ,@IPAddress               varchar(20)
AS
BEGIN
	 DECLARE @SysDatetime datetime = GETDATE()

    INSERT INTO L_Login
    (
        LoginDateTime
        ,LoginKBN
        ,LoginID
        ,RealECD
        ,LoginName
        ,IPAddress
    ) VALUES (
        @SysDatetime
        ,2
        ,@REStaffCD
        ,@RealECD
        ,@LoginName
        ,@IPAddress
    )

    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 2
    ,@LoginID       = @REStaffCD
    ,@RealECD       = @RealECD
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'r_login'
    ,@ProcessKBN    = 1
    ,@Remarks       = @LoginName
END
