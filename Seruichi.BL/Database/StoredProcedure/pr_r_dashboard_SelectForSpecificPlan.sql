IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectForSpecificPlan')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectForSpecificPlan]
END
GO

CREATE  PROCEDURE pr_r_dashboard_SelectForSpecificPlan
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  RP.ManPlanCD
              ,MP.ManPlanName
              ,MP.MaxNumber
              ,RM.TourokuSu
	FROM  M_REManPlan    RP   
	INNER JOIN M_MansionPlan  MP ON RP.ManPlanCD = MP.ManPlanCD 
    OUTER APPLY
	(SELECT Count(ConditionSEQ) AS TourokuSu
      FROM M_RECondMan 
     WHERE RealECD     = RP.RealECD
       AND DisabledFlg = 0
       AND ValidFLG = 0 
     ) AS  RM
	WHERE  RP.RealECD   = @realECD
      AND  RP.CancelFLG = 0　
END
GO
