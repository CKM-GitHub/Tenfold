IF EXISTS (select * from sys.objects where name = 'pr_r_login_Select_M_RealEstate_by_RealECD')
BEGIN
    DROP PROCEDURE [pr_r_login_Select_M_RealEstate_by_RealECD]
END
GO
CREATE PROCEDURE [dbo].[pr_r_login_Select_M_RealEstate_by_RealECD]
	-- Add the parameters for the stored procedure here
	@RealECD varchar(10)
AS
BEGIN
	
	Select * from M_RealEstate where RealECD = @RealECD;

END
