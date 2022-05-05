IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectForREName')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectForREName]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_SelectForREName]
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT REName 
	From M_RealEstate
	where RealECD = @realECD
END
