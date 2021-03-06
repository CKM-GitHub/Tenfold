IF EXISTS (select * from sys.objects where name = 'pr_tenfold_billing_process')
BEGIN
    DROP PROCEDURE [pr_tenfold_billing_process]
END
GO
Create PROCEDURE [dbo].[pr_tenfold_billing_process]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 Declare @TargetDate DateTime = getdate()
	 Declare @YYYYMM  varchar(6) = FORMAT(@TargetDate,'yyyyMM'),
	 @InvoiceNo varchar(20)

	Update A																
	set InvalidFLG=1								
	From D_BillingDetail A										
	Inner Join D_Billing B on B.InvoiceNo=A.InvoiceNo							
	Where B.InvoiceYYYYMM=@YYYYMM	
													
	Update D_Billing																
	set InvalidFLG=1 ,								
	DeleteOperator = Null ,							
	DeleteDateTime = @TargetDate,					
	DeleteIPAddress= @IPAddress				
	Where InvoiceYYYYMM=@YYYYMM

	DECLARE @RealECD varchar(11);
    
    --Cursor for RealECD
    DECLARE d_cursor CURSOR FOR
        Select RealECD							
		From M_RealEstate							
		Where DeleteDateTime is null	
		Order by RealECD

    --CursorOpen
    OPEN d_cursor;

    FETCH NEXT FROM d_cursor
    INTO @RealECD;

    WHILE @@FETCH_STATUS = 0
    BEGIN

		---for invoiceNo
		EXEC pr_M_Counter_GetNewID 
		@CounterKey = 9, 
		@NewID = @InvoiceNo OUTPUT
				
		---Table transfer speci A
				Insert Into D_billing
				(InvoiceNo,
				InvoiceYYYYMM,
				InvalidFLG,
				RealECD,
				TotalPrevAmount,
				CreditedAmount,
				RemainingAmount,
				InitialFee,
				AreaPlanFee,
				ManPlanFee,--10
				BasicTimes,
				ExtraTimes,
				BasicAmount,
				ExtraAmount,
				BasicFee,
				ExtraFee,
				OffsetTimes,
				OffsetFee,
				BaseAmount,
				TaxAmount,--20
				BillingAmount,
				TaxRate,
				TotaAmount,
				PrintDateTime,
				PrintCounts,
				TransferDateTime,
				TransferNo,
				TransferLinkDateTime,
				TransferResultKBN,
				TransferDate,--30
				TransferResultDateTime,
				InsertOperator,
				InsertOperatorName,
				InsertDateTime,
				InsertIPAddress,
				UpdateOperator,
				UpdateOperatorName,
				UpdateDateTime,
				UpdateIPAddress,
				DeleteOperator,--40
				DeleteOperatorName,
				DeleteDateTime,
				DeleteIPAddress,
				InsertSellerCD,
				InsertREStaffCD,
				InsertTenStaffCD,
				UpdateSellerCD,
				UpdateREStaffCD,
				UpdateTenStaffCD,
				DeleteSellerCD,
				DeleteREStaffCD,
				DeleteTenStaffCD
			)
			SELECT @InvoiceNo, 
				@YYYYMM,
				0,
				@RealECD,
				IsNull(db1.TotaAmount,0),
				ISNUll((select SUM(isnull(db2.TotaAmount,0))  from D_Billing as db2 where FORMAT(db2.TransferDateTime,'YYYYMM') = FORMAT(DATEADD(MM,-1,@TargetDate),'yyyyMM') and 
				db2.RealECD = mr.RealECD and InvalidFLG = 0 
				group by db2.RealECD),0),
				(ISNULL(db1.TotalPrevAmount,0) - ISNULL(db1.CreditedAmount,0)),
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				10,
				(ISNULL(db1.TotalPrevAmount,0) - ISNULL(db1.CreditedAmount,0)),
				Null,
				0,
				Null,
				0,
				Null,
				0,
				Null,
				Null,
				Null,
				Null,
			    @TargetDate,
			    @IPAddress,
				Null,
				Null,
			    @TargetDate,
			    @IPAddress,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null
			FROM M_RealEstate mr
			left outer join D_Billing db1 on db1.InvoiceYYYYMM = FORMAT(DATEADD(MM,-1,@TargetDate),'yyyyMM') and 
												db1.RealECD = mr.RealECD and db1.InvalidFLG=0
			WHERE mr.DeleteDateTime is null and mr.RealECD= @RealECD
			
		---D_RERegistration	B1,B2,B3

			---B1
			Insert into D_BillingDetail(
				InvoiceNo,
				InvoiceSEQ,
				InvalidFLG,
				InvoiceKBN,
				IntroKBN,
				MinusFlag,
				BillingAmount,
				BillingTimes,
				BillingFee,
				PlanCD,
				BillingText1,
				BillingText2,
				BillingText3,
				IntroReqID,
				PurchReqID)
			select 
			@InvoiceNo,
				(ISNULL((SELECT TOP 1 InvoiceSEQ FROM D_BillingDetail where InvoiceNo=@InvoiceNo ORDER BY InvoiceSEQ DESC),0) + (ROW_NUMBER() OVER(ORDER BY dre.RealECD ASC))),		
				0,
				1,
				0,
				0,
				dre.InitialFee,
				1,
				dre.InitialFee,
				Null,
				Char2,
				Null,
				Null,
				Null,
				Null
			from D_RERegistration dre
			left outer join M_MultPurpose mm on mm.DataID =120 and DataKey=1
			where dre.RealECD = @RealECD and 
			FORMAT(dre.JoinedDate,'yyyyMM') <= @YYYYMM and dre.InitialFee != 0 and
			dre.InvoiceNo is null and 
			dre.DeleteDateTime is null

			---B2
			Update  db
			Set db.InitialFee = IsNull(dbd.BillingAmount,0),
			db.BaseAmount = ISNull(db.BaseAmount,0) + IsNull(dbd.BillingAmount,0)
			from D_Billing db
			Inner Join D_BillingDetail dbd on dbd.InvoiceNo = db.InvoiceNo
			where db.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 1

			---B3
			update dr
			set dr.InvoiceNo = dbd.InvoiceNo,
			dr.InvoiceSEQ = dbd.InvoiceSEQ,
			dr.UpdateOperator = null ,
			dr.UpdateDateTime = @TargetDate,
			dr.UpdateIPAddress = @IPAddress
			from D_BillingDetail dbd
			inner join D_Billing db on db.InvoiceNo = dbd.InvoiceNo
			inner join D_RERegistration dr on dr.RealECD = db.RealECD
			where dbd.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN =1

		---M_REMoAreaPlan C1,C2,C3
			---C1
			Insert Into D_BillingDetail
				(InvoiceNo,
				InvoiceSEQ,
				InvalidFLG,
				InvoiceKBN,
				IntroKBN,
				MinusFlag,
				BillingAmount,
				BillingTimes,
				BillingFee,
				PlanCD,
				BillingText1,
				BillingText2,
				BillingText3,
				IntroReqID,
				PurchReqID)
			select @InvoiceNo,
				(ISNULL((SELECT TOP 1 InvoiceSEQ FROM D_BillingDetail where InvoiceNo=@InvoiceNo ORDER BY InvoiceSEQ DESC),0)+(ROW_NUMBER() OVER(ORDER BY Mre.RealECD ASC))),
				0,
				2,
				0,
				0,
				Mre.Amount,
				1,
				Mre.Amount,
				Mre.AreaPlanCD,
				Ma.AreaPlanName,
				Null,
				Null,
				Null,
				Null
			from M_REMoAreaPlan Mre 
				Inner Join M_AreaPlan Ma on Ma.AreaPlanCD = Mre.AreaPlanCD
				where Mre.RealECD = @RealECD and Mre.AppYYYYMM <= @YYYYMM and
				Mre.Amount != 0 and Mre.InvoiceNo is null and Mre.DeleteDateTime is null
		
			---C2
			Update  db
			Set db.AreaPlanFee = ISNull(dbd.BillingAmount,0),
			db.BaseAmount = ISNull(db.BaseAmount,0) + ISNull(dbd.BillingAmount,0)
			from D_Billing db
			Inner Join(select InvoiceNo,InvoiceKBN,Sum(IsNull(BillingAmount,0)) as BillingAmount
			from D_BillingDetail 
			group by InvoiceNo,InvoiceKBN) as dbd on dbd.InvoiceNo = db.InvoiceNo
			where db.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 2
		
			---C3
			Update mre
			Set mre.InvoiceNo = dbd.InvoiceNo,
			mre.InvoiceSEQ = dbd.InvoiceSEQ,
			mre.UpdateOperator = null,
			mre.UpdateDateTime = @TargetDate,
			mre.UpdateIPAddress = @IPAddress
			From D_BillingDetail dbd
			Inner Join D_Billing db on db.InvoiceNo = dbd.InvoiceNo
			Inner Join M_REMoAreaPlan mre on mre.RealECD = db.RealECD and 
			mre.AppYYYYMM = db.InvoiceYYYYMM and mre.AreaPlanCD = dbd.PlanCD
			where dbd.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 2

		---M_REMoManPlan D1,D2,D3
			---D1
			Insert Into D_BillingDetail
				(InvoiceNo,
				InvoiceSEQ,
				InvalidFLG,
				InvoiceKBN,
				IntroKBN,
				MinusFlag,
				BillingAmount,
				BillingTimes,
				BillingFee,
				PlanCD,
				BillingText1,
				BillingText2,
				BillingText3,
				IntroReqID,
				PurchReqID)
			select @InvoiceNo,
				(ISNULL((SELECT TOP 1 InvoiceSEQ FROM D_BillingDetail where InvoiceNo=@InvoiceNo ORDER BY InvoiceSEQ DESC),0)+(ROW_NUMBER() OVER(ORDER BY Mremo.RealECD ASC))),
				0,
				3,
				0,
				0,
				Mremo.Amount,
				1,
				Mremo.Amount,
				Mremo.ManPlanCD,
				Mma.ManPlanName,
				Null,
				Null,
				Null,
				Null
			from M_REMoManPlan Mremo 
				Inner Join M_MansionPlan Mma on Mma.ManPlanCD = Mremo.ManPlanCD
				where Mremo.RealECD = @RealECD and Mremo.AppYYYYMM <= @YYYYMM and
				Mremo.Amount != 0 and Mremo.InvoiceNo is null and Mremo.DeleteDateTime is null
		
			---D2
			Update  db
			Set db.ManPlanFee = ISNull(dbd.BillingAmount,0),
			db.BaseAmount = ISNull(db.BaseAmount,0) + ISNull(dbd.BillingAmount,0)
			from D_Billing db
			Inner Join(select InvoiceNo,InvoiceKBN,Sum(IsNull(BillingAmount,0)) as BillingAmount
			from D_BillingDetail 
			group by InvoiceNo,InvoiceKBN) as dbd on dbd.InvoiceNo = db.InvoiceNo
			where db.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 3
		
			---D3
			Update mremo
			Set mremo.InvoiceNo = dbd.InvoiceNo,
			mremo.InvoiceSEQ = dbd.InvoiceSEQ,
			mremo.UpdateOperator = null,
			mremo.UpdateDateTime = @TargetDate,
			mremo.UpdateIPAddress = @IPAddress

			From D_BillingDetail dbd
			Inner Join D_Billing db on db.InvoiceNo = dbd.InvoiceNo
			Inner Join M_REMoManPlan mremo on mremo.RealECD = db.RealECD and 
			mremo.AppYYYYMM = db.InvoiceYYYYMM and mremo.ManPlanCD = dbd.PlanCD
			where dbd.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 3

		---D_IntroReq E1,E2,E3
		---E1
			Insert Into D_BillingDetail
			(InvoiceNo,
			InvoiceSEQ,
			InvalidFLG,
			InvoiceKBN,
			IntroKBN,
			MinusFlag,
			BillingAmount,
			BillingTimes,
			BillingFee,
			PlanCD,
			BillingText1,
			BillingText2,
			BillingText3,
			IntroReqID,
			PurchReqID)
			select @InvoiceNo,
			(ISNULL((SELECT TOP 1 InvoiceSEQ FROM D_BillingDetail where InvoiceNo=@InvoiceNo ORDER BY InvoiceSEQ DESC),0)+(ROW_NUMBER() OVER(ORDER BY din.RealECD ASC))),
			0,
			4,
			din.IntroKBN,
			0,
			din.IntroAmounts,
			1,
			din.IntroAmounts,
			Null,
			Format(din.IntroDateTime,'YYYY/MM/dd'),
			mse.SellerName,
			msm.MansionName,
			din.IntroReqID,
			Null
			from D_IntroReq din 
			left outer Join M_Seller mse on mse.SellerCD = din.SellerCD
			left outer Join M_SellerMansion msm on msm.SellerMansionID = din.SellerMansionID
			where din.RealECD = @RealECD and din.IntroYYYYMM <=@YYYYMM and din.IntroYYYYMM != 0 and
			din.InvoiceNo is null and din.ProgressKBN = 9 and din.DeleteDateTime is null
		
		---E2
			update db
			set db.BasicTimes =(select  Count(*) as BasicTime 
				from D_BillingDetail  where IntroKBN =1  
				group by InvoiceNo),
			db.ExtraTimes =(select  Count(*) as ExtraTime
				from D_BillingDetail  where IntroKBN =2  
				group by InvoiceNo),
			db.BasicAmount =remo.BasicAmount,
			db.ExtraAmount = remo.ExtraAmount,
			db.BasicFee = ISNULL((select Sum(BillingAmount) as IntroAmount 
				from D_BillingDetail  where IntroKBN =1  
				group by InvoiceNo),0),
			db.ExtraFee = ISNULL((select Sum(BillingAmount) as IntroAmount 
				from D_BillingDetail  where IntroKBN =2  
				group by InvoiceNo),0),
			db.BaseAmount = ISNULL(db.BaseAmount,0) + ISNULL(dbd.IntroAmount,0)
			from D_Billing db 
			Inner Join M_REMoIntroAmount remo on remo.RealECD = db.RealECD and remo.AppYYYYMM =@YYYYMM
			Inner Join (select  InvoiceNo,Sum(BillingAmount) as IntroAmount 
			from D_BillingDetail  
			group by InvoiceNo) as dbd on dbd.InvoiceNo = db.InvoiceNo
			where db.InvoiceNo = @InvoiceNo
		
		---E3
			Update din
			Set din.InvoiceNo = dbd.InvoiceNo,
			din.InvoiceSEQ = dbd.InvoiceSEQ,
			din.UpdateOperator = null,
			din.UpdateDateTime = @TargetDate,
			din.UpdateIPAddress = @IPAddress
			From D_BillingDetail dbd
			Inner Join D_IntroReq din on din.IntroReqID = dbd.IntroReqID
			where dbd.InvoiceNo = @InvoiceNo and dbd.InvoiceKBN = 4

		---D_PurchReq F1,F2,F3
		---F1
			---to add select data from main query except IntroAmount Compare condtion
			CREATE TABLE #temp1         
			(
			   SEQ int,
			   IntroKBN int,
			   BillingAmount money,
			   BillingFee money,
			   BillingText1 varchar(300),
			   BillingText2 varchar(300),
			   BillingText3 varchar(300),
			   IntroReqID varchar(10),
			   PurchReqID varchar(10),
			   flg int ---to control (data compare)
			);

			---insert selected data(from main query except IntroAmount Compare condtion) into temp table
			Insert into #temp1(
				SEQ ,
				IntroKBN ,
				BillingAmount ,
				BillingFee ,
				BillingText1,
				BillingText2,
				BillingText3,
				IntroReqID,
				PurchReqID,
				flg)
			Select 
				ROW_NUMBER() OVER(ORDER BY dpc.RealECD ASC) as SEQ,
				din.IntroKBN,
				(ISNULL(din.IntroAmounts,0)) as BillingAmount,
				(ISNULL(din.IntroAmounts,0)) as BillingFee,
				Format(dpc.RERejectDateTime,'yyyy/MM/dd') as BillingText1,
				mse.SellerName as BillingText2,
				msm.MansionName as BillingText3,
				din.IntroReqID,
				dpc.PurchReqID,
				0
			from D_PurchReq dpc 
			left outer Join M_Seller mse on mse.SellerCD = dpc.SellerCD
			left outer Join M_SellerMansion msm on msm.SellerMansionID = dpc.SellerMansionID
			left outer join D_IntroReq din on din.IntroReqID = dpc.IntroReqID
			where dpc.RealECD = @RealECD and 
			dpc.RERejectfYYYYMM <= @YYYYMM and 
			dpc.RERejectfYYYYMM != 0 and 
			dpc.RERejectfYYYYMM >= FORMAT(DATEADD(MM,-3,@TargetDate),'yyyyMM') and 
			dpc.InvoiceNo is null and
			dpc.DeleteDateTime is null and 
			dpc.RERejectDateTime is not null
			Order by RERejectDateTime

			declare @sum money=0.00
			---cursor for sum of introAmount
			DECLARE @SEQ int,
				@IntroKBN int,
				@BillingAmount money,
				@BillingFee money,
				@BillingText1 varchar(300),
				@BillingText2 varchar(300),
				@BillingText3 varchar(300),
				@IntroReqID varchar(10),
				@PurchReqID varchar(10),
				@flg int;

			DECLARE d_amount CURSOR FOR
						Select * from #temp1
			
			OPEN d_amount;
			
			FETCH NEXT FROM d_amount
			INTO @SEQ ,
			@IntroKBN ,
			@BillingAmount,
			@BillingFee,
			@BillingText1,
			@BillingText2,
			@BillingText3,
			@IntroReqID,
			@PurchReqID,
			@flg;

			WHILE @@FETCH_STATUS = 0
			BEGIN
					set @sum += @BillingAmount
					
					IF @sum <= (select ISNULL(BaseAmount,0) from D_Billing where RealECD=@RealECD and InvoiceNo=@InvoiceNo)
					BEGIN
					----(when sum of IntroAmount is less than baseAmount of E2, change flg value into 1)
					   update #temp1
					   set flg = 1
					   where SEQ = @SEQ
					END
		
				FETCH NEXT FROM d_amount INTO @SEQ ,
										  @IntroKBN ,
										  @BillingAmount,
										  @BillingFee,
										  @BillingText1,
										  @BillingText2,
										  @BillingText3,
										  @IntroReqID,
										  @PurchReqID,
										  @flg;

				END 
			---cursorclosed for IntroAmount
			CLOSE d_amount  
			DEALLOCATE d_amount;

			---Insert data that is flg 1 from cursor looping into D_billingDetail 
			Insert Into D_BillingDetail
			(InvoiceNo,
			InvoiceSEQ,
			InvalidFLG,
			InvoiceKBN,
			IntroKBN,
			MinusFlag,
			BillingAmount,
			BillingTimes,
			BillingFee,
			PlanCD,
			BillingText1,
			BillingText2,
			BillingText3,
			IntroReqID,
			PurchReqID)
			select @InvoiceNo,
			ISNULL((SELECT TOP 1 InvoiceSEQ FROM D_BillingDetail where InvoiceNo=@InvoiceNo ORDER BY InvoiceSEQ DESC),0) + SEQ,
			0,
			5,
			IntroKBN,
			1,
			(ISNULL(BillingAmount,0) * (-1)),
			1,
			(ISNULL(BillingFee,0) * (-1)),
			Null,
			BillingText1,
			BillingText2,
			BillingText3,
			IntroReqID,
			PurchReqID
			from #temp1 where flg=1

			drop table #temp1
			
		---F2
			Update db
			Set db.OffsetTimes = dpc.OffsetTime,
			db.OffsetFee = ISNull(din.IntroAmount,0),
			db.BaseAmount = (ISNull(BaseAmount,0) - ISNull(din.IntroAmount,0))
			From D_Billing db 
			Inner Join (Select RealECD,Count(*) as OffsetTime
			from D_PurchReq 
			Group by RealECD) as dpc on dpc.RealECD = db.RealECD
			Inner Join(Select RealECD,Sum(IntroAmounts) as IntroAmount
			from D_IntroReq 
			Group by RealECD) as din on din.RealECD = db.RealECD
			where db.InvoiceNo =@InvoiceNo

		---F3
			Update dpr
			Set dpr.InvoiceNo = dbd.InvoiceNo,
			dpr.InvoiceSEQ = dbd.InvoiceSEQ,
			dpr.UpdateOperator = Null,
			dpr.UpdateDateTime = @TargetDate,
			dpr.UpdateIPAddress =@IPAddress
			from D_BillingDetail dbd 
			Inner Join D_PurchReq dpr on dpr.PurchReqID = dbd.PurchReqID 
			where dbd.InvoiceNo = @InvoiceNo
			and dbd.InvoiceKBN = 5

		---D_Billing G		
			Update db
			Set db.TaxAmount =(IsNull(db.BaseAmount,0) * 0.1),
			db.BillingAmount =(IsNull((IsNull(db.BaseAmount,0) * 0.1),0) * 1.1),
			db.TotaAmount = ISNULL(db.TotaAmount,0) + ISNULL((IsNull((IsNull(db.BaseAmount,0) * 0.1),0) * 1.1),0)
			From D_Billing db
			where db.InvoiceNo = @InvoiceNo

   
   FETCH NEXT FROM d_cursor INTO @RealECD
   END 
   ---cursorclosed for RealECD
   CLOSE d_cursor  
   DEALLOCATE d_cursor;

---M_Monthly	
	Update M_Monthly
	Set BillingYYYYMM = @YYYYMM,
	BillingUpdateDateTime = @TargetDate
	where DataKey = 1

---Insert L_Log
	EXEC pr_Tenfold_Insert_L_Log
			 @LoginKBN      = 3
			,@LoginID       = Null
			,@RealECD       = Null
			,@LoginName     = Null
			,@IPAddress     = @IPAddress
			,@Page          = 'tenfold_billing_process'
			,@Processing    = 'Update'
			,@Remarks       = N'請求処理'

END

GO
