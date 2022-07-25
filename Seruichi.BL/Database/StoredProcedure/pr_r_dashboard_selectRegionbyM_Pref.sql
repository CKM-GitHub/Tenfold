IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_selectRegionbyM_Pref')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_selectRegionbyM_Pref]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_selectRegionbyM_Pref] 
	-- Add the parameters for the stored procedure here
	@prefCD varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RegionCD From M_Pref where PrefCD = @prefCD
END
