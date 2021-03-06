IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_M_REMoAreaPlanSec')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_M_REMoAreaPlanSec]
END
GO
CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_M_REMoAreaPlanSec]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20),
	@SystemDateTime datetime 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	--M_REMoAreaPlanSec
	Delete 
	From M_REMoAreaPlanSec
	Where AppYYYYMM =(Select Format(@SystemDateTime,'yyyyMM'))
	--Table transfer specification F
	 INSERT INTO M_REMoAreaPlanSec
	 (
	 RealECD,
	 AppYYYYMM,
	 AreaPlanCD,
	 AreaPlanSEQ,
	 PrefCD,
	 CityCD,
	 TownCD,
	 InsertOperator,
	 InsertDateTime,
	 InsertIPAddress,
	 UpdateOperator,
	 UpdateDateTime,
	 UpdateIPAddress,
	 DeleteOperator,
	 DeleteDateTime,
	 DeleteIPAddress
	 )
	 Select 
	 A.RealECD,
	 FORMAT(getdate(),'yyyyMM'),
	 B.AreaPlanCD,
	 B.AreaPlanSEQ,
	 B.PrefCD,
	 B.CityCD,
	 B.TownCD,
	 Null,
	 @SystemDateTime,
	 @IPAddress,
	  Null,
	 @SystemDateTime,
	 @IPAddress,
	 Null,
	 Null,
	 Null

	 FROM 
	 M_RealEstate A
	 Inner Join M_RENxAreaPlanSec  B on B.RealECD=A.RealECD
	 Inner Join M_AreaPlan C on C.AreaPlanCD = B.AreaPlanCD
	 AND C.DeleteDateTime is Null 
	 Where A.DeleteDateTime is Null 

	 -- M_REAreaPlanSec--
	 --Table transfer specification M
	  Insert Into 
	 L_REAreaPlanSec
	 (
	 RecordDateTime,
	 RealECD,
	 AreaPlanCD,
	 AreaPlanSEQ,
	 PrefCD,
	 CityCD,
	 TownCD,
	 CancelFLG,
	 InsertOperator,
	 InsertDateTime,
	 InsertIPAddress,
	 UpdateOperator,
	 UpdateDateTime,
	 UpdateIPAddress,
	 DeleteOperator,
	 DeleteDateTime,
	 DeleteIPAddress
	 )
	 SELECT 
	 @SystemDateTime,
	 M.RealECD,
	 M.AreaPlanCD,
	 M.AreaPlanSEQ,
	 M.PrefCD,
	 M.CityCD,
	 M.TownCD,
	 M.CancelFLG,
	 M.InsertOperator,
	 M.InsertDateTime,
	 M.InsertIPAddress,
	 M.UpdateOperator,
	 M.UpdateDateTime,
	 M.UpdateIPAddress,
	 M.DeleteOperator,
	 M.DeleteDateTime,
	 M.DeleteIPAddress
	 From M_REAreaPlanSec M

	
	 Delete 
	 From M_REAreaPlanSec
	-- Table transfer specification G
	 Insert Into 
	 M_REAreaPlanSec(
	 RealECD,
	 AreaPlanCD,
	 AreaPlanSEQ,
	 PrefCD,
	 CityCD,
	 TownCD,
	 CancelFLG,
	 InsertOperator,
	 InsertDateTime,
	 InsertIPAddress,
	 UpdateOperator,
	 UpdateDateTime,
	 UpdateIPAddress,
	 DeleteOperator,
	 DeleteDateTime,
	 DeleteIPAddress
	 )
	 Select 
	 A.RealECD,
	 B.AreaPlanCD,
	 B.AreaPlanSEQ,
	 B.PrefCD,
	 B.CityCD,
	 B.TownCD,
	 0,
	 Null,
	 @SystemDateTime,
	 @IPAddress,
	  Null,
	 @SystemDateTime,
	 @IPAddress,
	 Null,
	 Null,
	 Null

	 FROM 
	 M_RealEstate A
	 Inner Join M_RENxAreaPlanSec  B on B.RealECD=A.RealECD
	 Inner Join M_AreaPlan C on C.AreaPlanCD = B.AreaPlanCD
	 AND C.DeleteDateTime is Null 
	 Where A.DeleteDateTime is Null 

	 --M_RENxAreaPlanSec 
	 Delete 
	 From M_RENxAreaPlanSec
	 --Table transfer specification H
	Insert Into M_RENxAreaPlanSec
		 (
			RealECD,
			AreaPlanCD,
			AreaPlanSEQ,
			PrefCD,
			CityCD,
			TownCD,
			CancelFLG,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress,
			UpdateOperator,
			UpdateDateTime,
			UpdateIPAddress,
			DeleteOperator,
			DeleteDateTime,
			DeleteIPAddress
			)
		
		  Select 
			 A.RealECD,
			 B.AreaPlanCD,
			 B.AreaPlanSEQ,
			 B.PrefCD,
			 B.CityCD,
			 B.TownCD,
			 0,
			 Null,
			 @SystemDateTime,
			 @IPAddress,
			  Null,
			 @SystemDateTime,
			 @IPAddress,
			 Null,
			 Null,
			 Null
		
			 FROM 
			 M_RealEstate A
			 Inner Join M_REAreaPlanSec  B on B.RealECD=A.RealECD
			 Inner Join M_AreaPlan C on C.AreaPlanCD = B.AreaPlanCD
			 AND C.DeleteDateTime is Null 
			 Where A.DeleteDateTime is Null  


	
	
END
