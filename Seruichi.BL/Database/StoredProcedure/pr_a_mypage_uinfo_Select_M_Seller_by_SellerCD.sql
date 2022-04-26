IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_uinfo_Select_M_Seller_by_SellerCD')
BEGIN
    DROP PROCEDURE [pr_a_mypage_uinfo_Select_M_Seller_by_SellerCD]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_uinfo_Select_M_Seller_by_SellerCD]
(
    @SellerCD               varchar(10)
)
AS
BEGIN

    SELECT
        SellerCD
        ,MailAddress
        ,SellerName
        ,SellerKana
        ,Birthday
        ,ZipCode1
        ,ZipCode2
		,PrefCD
        ,PrefName
        ,CityName
        ,TownName
        ,Address1
        ,Address2
        ,HandyPhone
        ,HousePhone
        ,Fax
    FROM M_Seller
    WHERE SellerCD = @SellerCD

END