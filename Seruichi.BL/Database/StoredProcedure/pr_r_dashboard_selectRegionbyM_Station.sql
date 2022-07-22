IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_selectRegionbyM_Station')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_selectRegionbyM_Station]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_selectRegionbyM_Station]
	-- Add the parameters for the stored procedure here
	@StationCD varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select RegionCD From M_Pref 
	where PrefCD = (Select PrefCD from M_Station where StationCD = @StationCD)
END
