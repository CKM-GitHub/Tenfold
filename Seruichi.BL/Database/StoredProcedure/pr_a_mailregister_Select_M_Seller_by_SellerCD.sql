IF EXISTS (select * from sys.objects where name = 'pr_a_mailregister_Select_M_Seller_by_SellerCD')
BEGIN
    DROP PROCEDURE [pr_a_mailregister_Select_M_Seller_by_SellerCD]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mailregister_Select_M_Seller_by_SellerCD]
(
    @SellerCD               varchar(10)
)
AS
BEGIN

    SELECT
        SellerCD
        ,MailAddress
        ,SellerName
    FROM M_Seller
    WHERE SellerCD = @SellerCD

END