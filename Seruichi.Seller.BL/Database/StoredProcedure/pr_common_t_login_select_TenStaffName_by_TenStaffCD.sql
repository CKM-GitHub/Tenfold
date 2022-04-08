IF EXISTS (select * from sys.objects where name = 'pr_common_t_login_select_TenStaffName_by_TenStaffCD')
BEGIN
    DROP PROCEDURE [pr_common_t_login_select_TenStaffName_by_TenStaffCD]
END
GO
CREATE PROCEDURE [dbo].[pr_common_t_login_select_TenStaffName_by_TenStaffCD]
	-- Add the parameters for the stored procedure here
	 @TenStaffCD varchar(10)
AS
BEGIN
	select  TenStaffName from M_TenfoldStaff where TenstaffCD=@TenStaffCD
END
