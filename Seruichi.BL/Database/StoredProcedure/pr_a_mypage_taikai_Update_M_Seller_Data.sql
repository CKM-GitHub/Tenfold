IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_taikai_Update_M_Seller_Data')
BEGIN
    DROP PROCEDURE [pr_a_mypage_taikai_Update_M_Seller_Data]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_taikai_Update_M_Seller_Data] 
	-- Add the parameters for the stored procedure here
	 @SellerCD varchar(10)
	,@IPAddress varchar(20)
	,@LoginName  varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SysDatetime datetime = GETDATE()

    UPDATE M_Seller SET 
         LeaveDateTime   = @SysDatetime
        ,InvalidFLG      = 1
		,InvalidDateTime = @SysDatetime
		,UpdateOperator  = @SellerCD
        ,UpdateDateTime  = @SysDatetime
        ,UpdateIPAddress = @IPAddress
    WHERE SellerCD = @SellerCD

	 EXEC pr_L_Seller_Insert
     @SellerCD     = @SellerCD


	EXEC pr_a_mypage_plus_Insert_L_Log
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @SellerCD
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_mypage_taikai'
    ,@Processing    = 'Update'
    ,@Remarks       = @SellerCD

END
