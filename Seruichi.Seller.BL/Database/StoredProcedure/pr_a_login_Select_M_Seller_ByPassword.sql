IF EXISTS (select * from sys.objects where name = 'pr_a_login_Select_M_Seller_ByPassword')
BEGIN
    DROP PROCEDURE [pr_a_login_Select_M_Seller_ByPassword]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Select_M_Seller_ByPassword]
(
    @Password            varchar(300)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT SellerCD, SellerName, MailAddress
    FROM M_Seller
    WHERE Password = @Password

END