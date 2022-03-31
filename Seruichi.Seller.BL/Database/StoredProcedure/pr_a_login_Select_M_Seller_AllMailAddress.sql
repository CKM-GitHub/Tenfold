IF EXISTS (select * from sys.objects where name = 'pr_a_login_Select_M_Seller_AllMailAddress')
BEGIN
    DROP PROCEDURE [pr_a_login_Select_M_Seller_AllMailAddress]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Select_M_Seller_AllMailAddress]
(
    @Password            varchar(20)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT MailAddress
    FROM M_Seller

END