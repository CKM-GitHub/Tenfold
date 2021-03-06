IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_M_REMoAreaPlan')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_M_REMoAreaPlan]
END
GO
CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_M_REMoAreaPlan] 
	-- Add the parameters for the stored procedure here
	
	 @IPAddress as varchar(20),
	 @SystemDateTime as datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    

		Delete 
		From  M_REMoAreaPlan
		Where AppYYYYMM =(SELECT FORMAT(@SystemDateTime,'yyyyMM'))

		--Table transfer specification C
		INSERT INTO M_REMoAreaPlan
			(
				RealECD,
				AppYYYYMM,
				AreaPlanCD,
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
		SELECT 
			A.RealECD,
			FORMAT(getdate(),'yyyyMM'),
			B.AreaPlanCD,
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

		FROM M_RealEstate A
		Inner Join M_RENxAreaPlan  B on B.RealECD=A.RealECD
		AND		B.DeleteDateTime is Null 
		AND		B.CancelFLG =0
		
		Inner Join M_AreaPlan C on C.AreaPlanCD = B.AreaPlanCD
		AND		C.DeleteDateTime is Null 
		Where	A.DeleteDateTime is Null 
		--Table transfer specification L
		INSERT INTO  L_REAreaPlan
			(
				 RecordDateTime,
				 RealECD,
				 AreaPlanCD,
				 ImmediatelyFLG,
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
		    From M_REAreaPlan M

        DELETE From M_REAreaPlan
		  
        --Table transfer specification D
		INSERT INTO M_REAreaPlan
		 (
			RealECD,
			AreaPlanCD,
			ImmediatelyFLG,
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
			R.RealECD,
			E.AreaPlanCD,
			0,
			0,
			Null,
			@SystemDateTime,
			@IPAddress,
			Null,
			@SystemDateTime,
			@IPAddress,
			Null ,
			Null,
			Null 
		FROM M_RealEstate R
		Inner Join M_RENxAreaPlan E on E.RealECD = R.RealECD
		AND       E.DeleteDateTime is Null 
		AND       E.CancelFLG =0

		Inner Join M_AreaPlan P on P.AreaPlanCD = E.AreaPlanCD
		AND       P.DeleteDateTime is Null 
		Where     R.DeleteDateTime is Null 



		DELETE From M_RENXAreaPlan
		
		--Table transfer specification E
		INSERT INTO M_RENXAreaPlan 
		(
			 RealECD,
			 AreaPlanCD,
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
			   R.RealECD,
			   E.AreaPlanCD,
			   0,
			   Null,
			   @SystemDateTime,
			   @IPAddress,
			   Null,
			   @SystemDateTime,
			   @IPAddress,
			   Null ,
			   Null,
			   Null 
		FROM   M_RealEstate R
		Inner Join M_REAreaPlan E on E.RealECD = R.RealECD
		AND		 E.DeleteDateTime is Null 
		AND		 E.CancelFLG =0
		Inner Join M_AreaPlan A on A.AreaPlanCD = E.AreaPlanCD
		AND		 A.DeleteDateTime is Null 
		Where	 R.DeleteDateTime is Null 



		
END
