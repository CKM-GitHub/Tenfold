IF EXISTS (select * from sys.objects where name = 'pr_t_seller_mansion_csv_generate')
BEGIN
    DROP PROCEDURE [pr_t_seller_mansion_csv_generate]
END
GO
CREATE PROCEDURE [dbo].[pr_t_seller_mansion_csv_generate]
	 @Chk_Mi			    tinyint         = 0
    ,@Chk_Kan				tinyint         = 0
	,@Chk_Satei				tinyint         = 0
	,@Chk_Kaitori			tinyint         = 0
	,@Chk_Kakunin			tinyint         = 0
	,@Chk_Kosho				tinyint         = 0
	,@Chk_Seiyaku			tinyint         = 0
	,@Chk_Urinushi			tinyint         = 0
	,@Chk_Kainushi			tinyint         = 0
	,@MansionName			varchar(50)
	,@Range					varchar(50)
	,@StartDate				date            = NULL
	,@EndDate				date            = NULL
AS
BEGIN
	IF @EndDate IS NULL set @EndDate='2099/12/31'

	select 
				Row_Number() Over (Order By A.SellerMansionID) As [NO],
				case when D.AssReqID IS NULL then N'未処理'
				when D.EasyAssDateTime IS NOT NULL AND D.DeepAssDateTime IS NULL then N'簡易査定'
				when D.DeepAssDateTime IS NOT NULL AND D.PurchReqDateTime IS NULL then N'査定依頼'
				when D.PurchReqDateTime IS NOT NULL AND D.ConfirmDateTime IS NULL then N'買取依頼'
				when D.ConfirmDateTime IS NOT NULL AND D.IntroDateTime IS NULL then N'確認中'
				when D.IntroDateTime IS NOT NULL AND D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime IS NULL then N'交渉中'
				when D.EndStatus=1 AND (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL) then N'成約'
				when D.EndStatus=2 AND (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL) then N'売主辞退'
				when D.EndStatus=3 AND (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL) then N'売主辞退'
				END as N'ステータス',
				A.SellerMansionID as N'物件CD',
				A.MansionName as N'マンション名',
				A.PrefCD + A.CityName + A.TownName + A.[Address] as N'住所',
				A.RoomNumber as N'部屋',
				A.LocationFloor as N'階数',
				A.RoomArea as N'面積',
				A.SellerCD as N'売主CD',
				B.SellerName as N'売主名',
				FORMAT(A.InsertDateTime, 'yyyy/MM/dd HH:mm:ss')  as N'登録日時',
				FORMAT(D.DeepAssDateTime, 'yyyy/MM/dd HH:mm:ss') as N'詳細査定日時',
				FORMAT(D.PurchReqDateTime, 'yyyy/MM/dd HH:mm:ss') as N'買取依頼日時',
				G.RealECD as N'マンション Top1業者CD',
				H.REName as N'マンション Top1',
				G.AssessAmount as N'マンション 金額',
				E.RealECD as N'エリア Top1業者CD',
				F.REName as N'エリア Top1',
				E.AssessAmount as N'エリア 金額',
				I.RealECD as N'買取依頼会社CD',
				J.REName as N'買取依頼会社',
				I.AssessAmount as N'買取依頼金額',
				FORMAT(D.IntroDateTime, 'yyyy/MM/dd HH:mm:ss') as  N'送客日時',
				CASE WHEN D.EndStatus =1 THEN
						CASE WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.SellerTermDateTime <= D.BuyerTermDateTime  THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss')  
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.BuyerTermDateTime <= D.SellerTermDateTime  THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss')   
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime  IS NULL THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NOT NULL THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NULL THEN NULL 
						END
					END As '成約日時',
					CASE WHEN D.EndStatus =2 OR D.EndStatus = 3 THEN  
						CASE WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.SellerTermDateTime <= D.BuyerTermDateTime  THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss')  
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.BuyerTermDateTime <= D.SellerTermDateTime  THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss')   
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime  IS NULL THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NOT NULL THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NULL THEN NULL 
						END
					END As '辞退日時'
	from M_SellerMansion A
	left outer join M_Seller B on B.SellerCD = A.SellerCD
	outer apply(
				select Top 1 *  from D_AssReqProgress C 
				where C.SellerMansionID = A.SellerMansionID
				and   C.DeleteDateTime  IS NULL
				order by C.InsertDateTime desc) D
	left outer join 
				D_AssessResult E 
				on E.AssReqID		= D.AssReqID
				and E.AssessType1	= 1
				and E.[Rank]		= 1
				and E.AssKBN		=2
	left outer join
				M_RealEstate F
				on F.RealECD = E.RealECD
	left outer join 
				D_AssessResult G 
				on G.AssReqID			 = D.AssReqID
				and G.AssessType1		 = 3
				and G.[AssDateTime]		= 1
				and G.AssKBN			= 2
	left outer join 
				M_RealEstate H 
				on H.RealECD = G.RealECD
	left outer join 
				D_AssessResult I
				on I.AssReqID = D.AssReqID
				and I.AssKBN = 2
				and I.IntroFLG = 1
	left outer join 
				M_RealEstate J 
				on J.RealECD = I.RealECD
    where ( A.MansionName like '%'+ @MansionName +'%') and A.DeleteDateTime IS NULL
	and (@Range = '登録日'		and (@StartDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  >= @StartDate)  and  ( CONVERT(DATE, A.InsertDateTime) <= @EndDate))
	OR (@Range = '詳細査定日'	and (@StartDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) >= @StartDate)  and  ( CONVERT(DATE, D.DeepAssDateTime) <= @EndDate)) 
	OR (@Range	= '買取依頼日'	and (@StartDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) >= @StartDate) and ( CONVERT(DATE, D.PurchReqDateTime) <= @EndDate))
	OR (@Range	= '送客日'		and (@StartDate IS NULL OR CONVERT(DATE, D.InsertDateTime) >= @StartDate)   and	( CONVERT(DATE, D.InsertDateTime)  <= @EndDate))
	OR (@Range	= '成約日'		and 
	   ((D.EndStatus =1 AND  @StartDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime) >= @StartDate)   and (CONVERT(DATE, D.SellerTermDateTime)  <= @EndDate)) 
	OR  ((@StartDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime) >= @StartDate)   and (CONVERT(DATE, D.BuyerTermDateTime)  <= @EndDate)))
	OR (@Range	= '辞退日'		and 
	   ((D.EndStatus = 2 OR D.EndStatus = 3 AND  @StartDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime) >= @StartDate)   and (CONVERT(DATE, D.SellerTermDateTime)  <= @EndDate)) 
	OR  ((@StartDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime) >= @StartDate)   and (CONVERT(DATE, D.BuyerTermDateTime)  <= @EndDate)))
	and (@Chk_Mi = 1 and D.AssReqID IS NULL)
	and ((@Chk_Kan  = 1 or D.EasyAssDateTime IS NOT NULL) and D.DeepAssDateTime IS NULL)
	and ((@Chk_Satei = 1 or D.DeepAssDateTime IS NOT NULL) and D.PurchReqDateTime IS NULL)
	and ((@Chk_Kaitori = 1 or D.PurchReqDateTime IS NOT NULL) and D.ConfirmDateTime IS NULL) 
	and ((@Chk_Kakunin = 1 or D.ConfirmDateTime IS NOT NULL) and D.IntroDateTime IS NULL)
	and ((@Chk_Kosho =1 or D.IntroDateTime IS NOT NULL) and D.SellerTermDateTime IS NULL and D.BuyerTermDateTime IS NULL)
	and ((@Chk_Seiyaku = 1 or D.EndStatus = 1) and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	and ((@Chk_Urinushi = 1 or D.EndStatus = 2) and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	and ((@Chk_Kainushi = 1 or D.EndStatus = 3) and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	Order by A.SellerMansionID
END







