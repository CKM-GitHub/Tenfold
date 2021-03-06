IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectCountFromV_RECondLineSta')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectCountFromV_RECondLineSta]
END
GO

CREATE  PROCEDURE pr_r_dashboard_SelectCountFromV_RECondLineSta
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Top 1 StationCD, Count(*)as Num  From V_RECondLineSta
                       Where  RealECD = @realECD and 期限切れ間近=1 group by StationCD
END
GO
