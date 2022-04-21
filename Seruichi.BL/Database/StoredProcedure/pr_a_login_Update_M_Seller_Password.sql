IF EXISTS (select * from sys.objects where name = 'pr_a_login_Update_M_Seller_Password')
BEGIN
    DROP PROCEDURE [pr_a_login_Update_M_Seller_Password]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Update_M_Seller_Password]
(
    @SellerCD                   varchar(10)
    ,@Password                  varchar(300)
    ,@IPAddress                 varchar(20)
    ,@LoginName                 varchar(50)
)
AS
BEGIN
    DECLARE @SysDatetime datetime = GETDATE()

    UPDATE M_Seller SET 
    [Password]          = @Password
    ,UpdateOperator     = @SellerCD
    ,UpdateIPAddress    = @IPAddress
    ,UpdateDateTime     = @SysDatetime

    WHERE SellerCD = @SellerCD


    EXEC pr_L_Seller_Insert
     @SellerCD      = @SellerCD


    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @SellerCD
    --,@RealECD       
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_login'
    ,@ProcessKBN    = 2
    ,@Remarks       = @LoginName

END