IF EXISTS (select * from sys.objects where name = 'pr_a_mailregister_Update_M_Seller_MailAddress')
BEGIN
    DROP PROCEDURE [pr_a_mailregister_Update_M_Seller_MailAddress]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mailregister_Update_M_Seller_MailAddress]
(
    @SellerCD                   varchar(10)
    ,@MailAddress               varchar(300)
    ,@Password                  varchar(300)
    ,@IPAddress                 varchar(20)
    ,@LoginName                 varchar(50)
)
AS
BEGIN

    DECLARE @SysDatetime datetime = GETDATE()

    UPDATE M_Seller SET 
     MailAddress        = @MailAddress
    ,[Password]         = @Password
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
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_mailregister'
    ,@ProcessKBN    = 1
    ,@Remarks       = @LoginName

END