IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectNumForNewRequest')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectNumForNewRequest]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_SelectNumForNewRequest]
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Count(*) as Number From D_PurchReq											
	Where RealECD = @realECD				
	and EndStatus      = 0											
	and REConfDateTime Is Null											

END
