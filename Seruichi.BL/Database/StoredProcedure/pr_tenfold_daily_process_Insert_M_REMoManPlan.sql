IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_M_REMoManPlan')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_M_REMoManPlan]
END
GO

CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_M_REMoManPlan]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20),
	@SystemDateTime datetime 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

	Delete 
	From M_REMoManPlan
	Where AppYYYYMM =(SELECT Format(@SystemDateTime,'yyyyMM'))
	--Table transfer specification I
	Insert Into M_REMoManPlan
		(
			RealECD,
			AppYYYYMM,
			ManPlanCD,
			CancelFLG,
			Amount,
			InvoiceNo,
			InvoiceSEQ,
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
			B.ManPlanCD,
			0,
			C.Amount,
			Null,
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

	 FROM   M_RealEstate A
	 Inner Join M_RENxManPlan  B on B.RealECD=A.RealECD
	 AND B.DeleteDateTime is Null 
	 AND B.CancelFLG = 0
			
	 Inner Join M_MansionPlan C on C.ManPlanCD = B.ManPlanCD
	 AND C.DeleteDateTime is Null 
	 Where A.DeleteDateTime is Null 

	 --M_REManPlan
	 --Table transfer specification N
	 Insert Into L_REManPlan
		(
			RecordDateTime,
			RealECD,
			ManPlanCD,
			ImmediatelyFLG,
			CancelFLG,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress,
			UpdateOperator,
			UpdateDateTime,
			UpdateIPAddress,
			DeleteOperator	,
			DeleteDateTime,
			DeleteIPAddress
		)
	 SELECT 
			@SystemDateTime,
			M.RealECD,
			M.ManPlanCD,
			M.ImmediatelyFLG,
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
	 From M_REManPlan M


	 Delete 
	 From M_REManPlan
	 --Table transfer specification J
	 Insert Into M_REManPlan
	   (
			RealECD,
			ManPlanCD,
			ImmediatelyFLG,
			CancelFLG,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress,
			UpdateOperator	,
			UpdateDateTime	,
			UpdateIPAddress,
			DeleteOperator,
			DeleteDateTime,
			DeleteIPAddress
	  )
	 Select 
			A.RealECD,
			B.ManPlanCD,
			0,
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

	 FROM  M_RealEstate A
	 Inner Join M_RENxManPlan  B on B.RealECD=A.RealECD
	 AND B.DeleteDateTime is Null 
	 AND B.CancelFLG =0 
	 Inner Join M_MansionPlan C on C.ManPlanCD = B.ManPlanCD
	 AND C.DeleteDateTime is Null 
	 Where A.DeleteDateTime is Null 
	 
	-- M_RENxManPlan
	 Delete 
	 From M_RENxManPlan
	 --Table transfer specification K
	 INSERT INTO M_RENxManPlan
		(
			RealECD,
			ManPlanCD,
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
			A.RealECD,
			B.ManPlanCD,
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
	 FROM   M_RealEstate A
	 INNER JOIN M_REManPlan  B on B.RealECD=A.RealECD
	 AND B.DeleteDateTime IS NULL 
	 AND B.CancelFLG =0 
	 INNER JOIN M_MansionPlan C on C.ManPlanCD = B.ManPlanCD
	 AND C.DeleteDateTime is Null 
	 Where A.DeleteDateTime is Null  

	

END
