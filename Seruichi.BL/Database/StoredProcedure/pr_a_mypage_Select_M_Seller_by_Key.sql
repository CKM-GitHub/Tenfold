IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_uinfo_Select_M_Seller_by_Key')
BEGIN
    DROP PROCEDURE [pr_a_mypage_uinfo_Select_M_Seller_by_Key]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_uinfo_Select_M_Seller_by_Key]
(
    @SellerCD               varchar(10)
)
AS
BEGIN

    SELECT
        SellerCD
        ,MailAddress
        ,[Password]
        ,SellerName
        ,SellerKana
        ,Birthday
        ,ZipCode1
        ,ZipCode2
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