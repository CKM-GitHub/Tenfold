IF EXISTS (select * from sys.objects where name = 'pr_a_login_Select_M_Seller_All')
BEGIN
    DROP PROCEDURE [pr_a_login_Select_M_Seller_All]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Select_M_Seller_All]
AS
BEGIN

    SET NOCOUNT ON

    SELECT SellerCD, SellerName, SellerKana, Birthday, MailAddress, HandyPhone
    FROM M_Seller

END