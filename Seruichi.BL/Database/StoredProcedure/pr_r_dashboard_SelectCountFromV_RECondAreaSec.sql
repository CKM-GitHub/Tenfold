IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectCountFromV_RECondAreaSec')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectCountFromV_RECondAreaSec]
END
GO

CREATE PROCEDURE pr_r_dashboard_SelectCountFromV_RECondAreaSec
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Count(*)as Num From V_RECondAreaSec 
                       Where  RealECD = @realECD
END
GO
