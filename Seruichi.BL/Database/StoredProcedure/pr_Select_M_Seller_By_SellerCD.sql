IF EXISTS (select * from sys.objects where name = 'pr_Select_M_Seller_By_SellerCD')
BEGIN
    DROP PROCEDURE [pr_Select_M_Seller_By_SellerCD]
END
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pr_Select_M_Seller_By_SellerCD] 
	-- Add the parameters for the stored procedure here
	@SellerCD   varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM M_Seller Where SellerCD =@SellerCD
END
