IF EXISTS (select * from sys.objects where name = 'pr_t_reale_asmhis_get_DisplayData')
BEGIN
    DROP PROCEDURE [pr_t_reale_asmhis_get_DisplayData]
END
GO

Create  procedure [dbo].[pr_t_reale_asmhis_get_DisplayData]
								  
								   
							    @SellerCD   varchar(10),
							    @RealECD	varchar(10),
							    @Range      varchar(50),--=N'買取依頼日',
							    @StartDate  date = NULL,
							    @EndDate    date = NULL,
							    @Chk_Area	 tinyint,--         = 1,
							    @Chk_Mansion	 tinyint ,--        = 1,
							    @Chk_SendCustomer	 tinyint ,--        = 1,
							    @Chk_Top5	tinyint      ,--   = 1,
							    @Chk_Top5Out	tinyint        ,-- = 1,
							    @Chk_NonMemberSeller tinyint -- = 0
								as
								Begin
							
							select  
								Row_Number() Over (Order By D.DeepAssDateTime, D.AssReqID ) As [NO],
								(Case  When C.AssessType1 = 1 then N'エリア'  When C.AssessType1 = 3 then N'マンション' end ) as AssessType,
								(Case When C.IntroFlg  =1 then N'送客' else '' end ) as SendCustomer,

								D.AssReqID as AssessReqID,
								(Isnull(A.MansionName,'') + Isnull(A.RoomNumber,'')) as RoomNumber,
								Isnull(A.MansionName,'')  MansionName,
								Isnull(A.RoomNumber,'') RoomNo,
								B.SellerName as SellerName,
								ISNULL(FORMAT(A.InsertDateTime, 'yyyy/MM/dd HH:mm:ss'),'') AS InsertDate,
								ISNULL(FORMAT(D.EasyAssDateTime, 'yyyy/MM/dd HH:mm:ss'),'') AS EasyDate,
								ISNULL(FORMAT(D.DeepAssDateTime, 'yyyy/MM/dd HH:mm:ss'),'') AS DeepDate,
								ISNULL(FORMAT(D.PurchReqDateTime, 'yyyy/MM/dd HH:mm:ss'),'') AS PurchaseDate,
								ISNULL(FORMAT(D.IntroDateTime, 'yyyy/MM/dd HH:mm:ss'),'')AS  IntroDate,

								--(Case when C.AssessType1 = 1 then C.Rank else '' end) as AreaRank,
								--(Case when C.AssessType1 = 3 then C.Rank else '' end) as MansionRank,
								C.Rank,
								D.SellerMansionID,
								A.SellerCD,
								--Isnull(D.AssessAmount,0.0) as AssessAmount
								 ISNULL(FORMAT(CONVERT(MONEY, D.AssessAmount), '###,###'),'0') AssessAmount

								
								 from D_AssReqProgress D
								inner join D_AssessResult C on C.AssReqID =D.AssReqID and
														   C.AssKBN = 2 and
														   C.RealECD= D.RealECD and
														   C.DeleteDateTime is Null  	
								left outer join M_SellerMansion A on A.SellerMansionID= D.SellerMansionID
								left outer join M_Seller B on B.SellerCD =D.SellerCD
								where
								
								(@RealECD is null or (D.RealECD =@RealECD))
								and D.DeleteDateTime is null
								and 

								 ((@Range = N'登録日'		and (@StartDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  >= @StartDate)  and  ( @EndDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  <= @EndDate))
							     OR (@Range =  N'簡易査定日'	and (@StartDate IS NULL OR CONVERT(DATE, D.EasyAssDateTime) >= @StartDate)  and  ( @EndDate IS NULL OR CONVERT(DATE, D.EasyAssDateTime) <= @EndDate)) 
							     OR (@Range =  N'詳細査定日'	and (@StartDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) >= @StartDate)  and ( @EndDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime)  <= @EndDate))
							     OR (@Range	= N'買取依頼日'	and (@StartDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) >= @StartDate) and	( @EndDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) <= @EndDate))
							     OR (@Range	= N'送客日'		and (@StartDate IS NULL OR CONVERT(DATE, D.IntroDateTime) >= @StartDate) and	( @EndDate IS NULL OR CONVERT(DATE, D.IntroDateTime) <= @EndDate))	)
								and
									(
									 ( @Chk_Area = 1 and @Chk_Mansion  =1 and (C.AssessType1=3 or C.AssessType1=1  ))  or
									((Case 
									when @Chk_Area = 1 and @Chk_Mansion =0  then 1   
									when @Chk_Area = 0 and @Chk_Mansion =1  then 3   
									 end								  
									) = C.AssessType1 ) 
									)

								 and 
								  C.IntroFlg = @Chk_SendCustomer
								-- (@Chk_SendCustomer =0 or (@Chk_SendCustomer = 1 and  C.IntroFlg =1)  )
								 and    
										(( (@Chk_Top5 =1 and C.Rank <= 5 ))  or ( (@Chk_Top5Out =1 and C.Rank >5))) 
								 and    
								  ((@Chk_NonMemberSeller = 0 and  D.SellerCD is not null) or (@Chk_NonMemberSeller = 1 and  D.SellerCD is null))
								  --(( ( @Chk_NonMemberSeller = 0 ) or  (@Chk_NonMemberSeller = 1 and  D.SellerCD is null)    )) 
								 
								  
												Order by D.DeepAssDateTime, D.AssReqID 

												End