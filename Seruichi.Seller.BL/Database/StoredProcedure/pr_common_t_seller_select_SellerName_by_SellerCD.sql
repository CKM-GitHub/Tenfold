IF EXISTS (select * from sys.objects where name = 'pr_common_t_seller_select_SellerName_by_SellerCD')
BEGIN
    DROP PROCEDURE [pr_common_t_seller_select_SellerName_by_SellerCD] 
END
GO

CREATE PROCEDURE [dbo].[pr_common_t_seller_select_SellerName_by_SellerCD] 
	-- Add the parameters for the stored procedure here
	@SellerCD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select SellerName from  M_Seller     where SellerCD = @SellerCD 
END
