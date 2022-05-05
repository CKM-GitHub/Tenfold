IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectForOldestDatecount')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectForOldestDatecount]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_SelectForOldestDatecount]
	-- Add the parameters for the stored procedure here
	@ConfDateTime as datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select　'(' +CONVERT(VARCHAR(10),DATEDIFF(day, @ConfDateTime,GetDate())) + N'日前)' as datecount	
END
