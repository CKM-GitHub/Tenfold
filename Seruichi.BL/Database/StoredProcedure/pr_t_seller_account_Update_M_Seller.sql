IF EXISTS (select * from sys.objects where name = 'pr_t_seller_account_Update_M_Seller')
BEGIN
    DROP PROCEDURE [pr_t_seller_account_Update_M_Seller] 
END
GO
CREATE PROCEDURE [dbo].[pr_t_seller_account_Update_M_Seller] 
	-- Add the parameters for the stored procedure here
	 @SellerCD varchar(10),
	 @TestFLG tinyint,
     @InvalidFLG tinyint,
	 @LoginID varchar(10),
	 @IPAddress varchar(20),
	 @LoginName  varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @SysDatetime datetime = GETDATE()
	

	Update M_Seller
	SET LeaveDateTime	 =  CASE  
							WHEN @InvalidFLG=1 THEN @SysDatetime
							WHEN @InvalidFLG=0 THEN Null
							END ,
		TestFLG			 =  @TestFLG,
		InvalidFLG       =  @InvalidFLG,
		InvalidDateTime  =  CASE  
							WHEN @InvalidFLG=1 THEN @SysDatetime
							WHEN @InvalidFLG=0 THEN Null
							END ,
		UpdateOperator   =  @LoginID,
		UpdateDateTime   =  @SysDatetime,
		UpdateIPAddress  =  @IPAddress
		Where  SellerCD= @SellerCD
   
	 EXEC pr_L_Seller_Insert
     @SellerCD     = @SellerCD


	EXEC pr_a_mypage_plus_Insert_L_Log
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @LoginID
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 't_seller_account'
    ,@Processing    = 'Update'
    ,@Remarks       = @SellerCD

END