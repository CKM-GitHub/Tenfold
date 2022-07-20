IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectForSendCustomerInfo')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectForSendCustomerInfo]
END
GO

CREATE  PROCEDURE pr_r_dashboard_SelectForSendCustomerInfo 
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT AppYYYYMM,(BasicTimes+ExtraTimes)as total ,((BasicTimes+ExtraTimes)-IntroTimes) as IntroTimes
    From M_REMoIntroAmount
	Where RealECD = @realECD
      and AppYYYYMM = (SELECT MasterYYYYMM From M_Monthly Where DataKey=1)

END
GO
