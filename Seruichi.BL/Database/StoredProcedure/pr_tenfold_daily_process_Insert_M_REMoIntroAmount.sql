
IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_M_REMoIntroAmount')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_M_REMoIntroAmount]
END
GO
CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_M_REMoIntroAmount]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20),
	@SystemDateTime datetime 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	--M_REMoIntroAmount
	Delete 
	From M_REMoIntroAmount
	Where AppYYYYMM = (Select Format(@SystemDateTime,'yyyyMM'))
	--Table transfer specification O
	INSERT INTO M_REMoIntroAmount
		(
			RealECD,
			AppYYYYMM,
			BasicAmount,
			ExtraAmount,
			BasicTimes,
			ExtraTimes,
			IntroTimes,
			AcceptTimes,
			RejectTimes,
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
			B.BasicAmount,
			B.ExtraAmount,
			B.BasicTimes,
			0,
			0,
			0,
			0,
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

	FROM	M_RealEstate A
	Inner Join M_RENxIntroAmount  B on B.RealECD=A.RealECD
	AND		B.DeleteDateTime is Null 
	Where	A.DeleteDateTime is Null 

--M_REIntroAmount
--Table transfer specification R
     Insert Into L_REIntroAmount
		(
			RecordDateTime,
			RealECD,
			BasicAmount,
			ExtraAmount,
			BasicTimes,
			ImmediatelyFLG,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress	,
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
			M.BasicAmount,
			M.ExtraAmount,
			M.BasicTimes,
			M.ImmediatelyFLG,
			M.InsertOperator,
			M.InsertDateTime,
			M.InsertIPAddress,
			M.UpdateOperator,
			M.UpdateDateTime,
			M.UpdateIPAddress,
			M.DeleteOperator,
			M.DeleteDateTime,
			M.DeleteIPAddress
	From	M_REIntroAmount M 

	 Delete 
	 From M_REIntroAmount
--Table transfer specification P
	 Insert Into  M_REIntroAmount
		(
			RealECD,
			BasicAmount,
			ExtraAmount,
			BasicTimes,
			ImmediatelyFLG,
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
			B.BasicAmount,
			B.ExtraAmount,
			B.BasicTimes,
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

	FROM    M_RealEstate A
	Inner Join M_RENxIntroAmount  B on B.RealECD=A.RealECD
	AND		B.DeleteDateTime is Null 
	Where	A.DeleteDateTime is Null 


	 Delete 
	 From M_RENxIntroAmount
--Table transfer specification Q	 
	Insert Into M_RENxIntroAmount
		(
			RealECD,
			BasicAmount,
			ExtraAmount,
			BasicTimes,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress	,
			UpdateOperator,
			UpdateDateTime,
			UpdateIPAddress,
			DeleteOperator,
			DeleteDateTime,
			DeleteIPAddress
	   )
	  Select 
			A.RealECD,
			B.BasicAmount,
			B.ExtraAmount,
			B.BasicTimes,
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
	 Inner Join M_REIntroAmount  B on B.RealECD=A.RealECD
	 AND    B.DeleteDateTime is Null 
	 Where  A.DeleteDateTime is Null 


	

END

