IF EXISTS (select * from sys.objects where name = 'pr_r_login_select_REStaffName_by_REStaffCD')
BEGIN
    DROP PROCEDURE [pr_r_login_select_REStaffName_by_REStaffCD]
END
GO
ALTER PROCEDURE [dbo].[pr_r_login_select_REStaffName_by_REStaffCD]
	-- Add the parameters for the stored procedure here
	@REStaffCD varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT REStaffName from M_REStaff where REStaffCD = @REStaffCD COLLATE SQL_Latin1_General_CP1_CS_AS 
END

