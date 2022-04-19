IF EXISTS (select * from sys.objects where name = 'pr_a_login_Insert_L_Login')
BEGIN
    DROP PROCEDURE [pr_a_login_Insert_L_Login]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Insert_L_Login]
(
    @SellerCD               varchar(10)
    ,@LoginName             varchar(50)
    ,@IPAddress             varchar(20)
)
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
        ,1
        ,@SellerCD
        ,NULL
        ,@LoginName
        ,@IPAddress
    )

    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @SellerCD
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_login'
    ,@ProcessKBN    = 1
    ,@Remarks       = @LoginName

END