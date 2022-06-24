IF EXISTS (select * from sys.objects where name = 'pr_a_login_Select_M_Seller_CheckInvalid')
BEGIN
    DROP PROCEDURE [pr_a_login_Select_M_Seller_CheckInvalid]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Select_M_Seller_CheckInvalid]
(
    @SellerCD            varchar(10)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT InvalidFLG, LeaveDateTime
    FROM M_Seller
    WHERE SellerCD = @SellerCD

END