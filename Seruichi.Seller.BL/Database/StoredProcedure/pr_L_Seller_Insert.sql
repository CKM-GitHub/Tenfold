IF EXISTS (select * from sys.objects where name = 'pr_L_Seller_Insert')
BEGIN
    DROP PROCEDURE [pr_L_Seller_Insert]
END
GO

CREATE PROCEDURE [dbo].[pr_L_Seller_Insert]
(
    @SellerCD       varchar(10)
)
AS
BEGIN
    INSERT INTO L_Seller
    (
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
        ,PossibleTimes
        ,LeaveDateTime
        ,TestFLG
        ,InvalidFLG
        ,InvalidDateTime
        ,Remark
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    )
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
        ,PossibleTimes
        ,LeaveDateTime
        ,TestFLG
        ,InvalidFLG
        ,InvalidDateTime
        ,Remark
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    FROM M_Seller
    WHERE SellerCD = @SellerCD
END