IF EXISTS (select * from sys.objects where name = 'pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD')
BEGIN
    DROP PROCEDURE [pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD]
END
GO
CREATE PROCEDURE [dbo].[pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD]
	-- Add the parameters for the stored procedure here
	@RealECD	varchar(10),
	@REStaffCD	varchar(10)
AS
BEGIN
	Select REPassword,PermissionChat,PermissionSetting,PermissionPlan,PermissionInvoice
	from M_REStaff
	where RealECD = @RealECD and REStaffCD = @REStaffCD
END
