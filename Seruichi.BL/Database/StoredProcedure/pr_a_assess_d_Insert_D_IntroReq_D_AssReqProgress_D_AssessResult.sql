IF EXISTS (select * from sys.objects where name = 'pr_a_assess_d_Insert_D_IntroReq_D_AssReqProgress_D_AssessResult')
BEGIN
    DROP PROCEDURE [pr_a_assess_d_Insert_D_IntroReq_D_AssReqProgress_D_AssessResult]
END
GO
Create PROCEDURE [dbo].[pr_a_assess_d_Insert_D_IntroReq_D_AssReqProgress_D_AssessResult]
	 
			@AssID		as varchar(100),					
			@AssSEQ		as varchar(100),
			@AssReqID	as varchar(100),				
			@ProgressKBN	as varchar(100),						
			@RealECD		as varchar(100),					
			@SellerCD	as varchar(100),						
			@SellerMansionID	as varchar(100),						
			@AssessAmount		as varchar(100),		
			@SellerName as varchar(200),
			@IpAddress as varchar(100),
			@AssessType1 as varchar(1),
			@AssessType2 as varchar(1),
			@ConditionSEQ as varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
			declare @IntroReqID as varchar(10), @Nowdate as datetime= getdate();
		     EXEC pr_M_Counter_GetNewID
             @CounterKey = 5, 
            @NewID = @IntroReqID OUTPUT 
			--select @IntroReqID

			--D_IntroReq
			insert into [D_IntroReq]	
			(
			IntroReqID,
			AssID,						
			AssSEQ,							
			ProgressKBN	,						
			RealECD,							
			SellerCD	,						
			SellerMansionID	,						
			AssessAmount	,						
			IntroReqDateTime	,						
			NameConfDateTime,							
			AddressConfDateTime,						
			TelConfDateTime,							
			MailConfDateTime	,						
			RegistConfDateTime,						
			IntroPlanDate		,					
			TenStaffCD		,					
			Remark			,				
			IntroDateTime		,					
			IntroYYYYMM		,					
			IntroAmounts		,					
			IntroKBN			,				
			InvoiceNo		,					
			InvoiceSEQ		,					
			InsertOperator	,						
			InsertOperatorName,							
			InsertDateTime	,						
			InsertIPAddress	,						
			UpdateOperator	,						
			UpdateOperatorName,							
			UpdateDateTime	,						
			UpdateIPAddress	,						
			DeleteOperator	,						
			DeleteOperatorName,							
			DeleteDateTime	,						
			DeleteIPAddress	,						
			InsertSellerCD	,						
			InsertREStaffCD	,						
			InsertTenStaffCD	,						
			UpdateSellerCD	,						
			UpdateREStaffCD	,						
			UpdateTenStaffCD	,						
			DeleteSellerCD	,						
			DeleteREStaffCD	,						
			DeleteTenStaffCD							

			)
			values
			(
				@IntroReqID,
				@AssID,						
				@AssSEQ	,						
				1		,					
				@RealECD	,						
				@SellerCD	,						
				@SellerMansionID,							
				@AssessAmount,							
				@Nowdate		,					
				null	,						
				null,							
				null	,						
				null		,					
				null	,						
				null	,						
				null	,						
				null			,				
				null	,						
				0		,					
				0		,					
				0	,						
				null	,						
				0	,						
				@SellerCD	,						
				@SellerName	,						
				@Nowdate	,						
				@IPAddress	,						
				@SellerCD		,					
				@SellerName	,						
				@Nowdate	,						
				@IPAddress	,						
				null	,						
				null	,						
				null	,						
				null		,					
				@SellerCD	,						
				null	,						
				null	,						
				@SellerCD	,						
				null	,						
				null	,						
				null	,						
				null	,						
				null							

			)
  
			--D_AssReqProgress 
			Update
 								D_AssReqProgress set
								PurchReqDateTime = @Nowdate, 						
								ConfirmDateTime	=null,						
								IntroDateTime		=null,					
								RealECD			=@RealECD,				
								REConfirmDateTime	=null,						
								AssessType1		=@AssessType1	,				
								AssessType2		=@AssessType2,				
								ConditionSEQ		=@ConditionSEQ,					
								AssessAmount		=@AssessAmount,					
								AssID			=@AssID,				
								AssSEQ			=@AssSEQ	,			
								UpdateOperator	=@SellerCD	,					
								UpdateOperatorName=@SellerName	,						
								UpdateDateTime	=@Nowdate,						
								UpdateIPAddress	=@IPAddress,						
								UpdateSellerCD	=@SellerCD	 
								where AssReqID = @AssReqID

			--D_AssessResult
			Update D_AssessResult
								set  
								IntroFLG		=1,					
								UpdateOperator	=@SellerCD,						
								UpdateOperatorName=@SellerName,							
								UpdateDateTime	=@Nowdate,						
								UpdateIPAddress	=@IpAddress,						
								UpdateSellerCD	=@SellerCD	 	
								where 	AssID	=@AssID and AssSEQ=@AssSEQ			

			
END