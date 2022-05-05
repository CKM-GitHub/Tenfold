IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectREFaceImage')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectREFaceImage]
END
GO
CREATE PROCEDURE [dbo].[pr_r_dashboard_SelectREFaceImage]
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10),
	@restaffCD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select REFaceImage
	From M_REImage													
	Where RealECD = @realECD				
	and REStaffCD = @restaffCD				

END
