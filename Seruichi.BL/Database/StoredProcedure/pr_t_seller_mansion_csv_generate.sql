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
	--IF @EndDate IS NULL set @EndDate='2099/12/31'

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
				ISNULL(A.SellerMansionID,'') As '物件CD',
				ISNULL(A.MansionName,'') As 'マンション名',
				ISNULL(A.PrefName + A.CityName + A.TownName + A.[Address],'') As '住所',
				ISNULL(A.RoomNumber,'0') As '部屋',
				ISNULL(A.LocationFloor,'0') As '階数',
				ISNULL(A.RoomArea,'0') As '面積',
				ISNULL(A.SellerCD,'') as N'売主CD',
				ISNULL(B.SellerName,'') As '売主名',
				ISNULL(FORMAT(A.InsertDateTime, 'yyyy/MM/dd HH:mm:ss'),'') As '登録日時',
				ISNULL(FORMAT(D.DeepAssDateTime, 'yyyy/MM/dd HH:mm:ss'),'') As '詳細査定日時',
				ISNULL(FORMAT(D.PurchReqDateTime, 'yyyy/MM/dd HH:mm:ss'),'') As '買取依頼日時',
				ISNULL(G.RealECD,'') as N'マンション Top1業者CD',
				ISNULL(H.REName,'') As 'マンションTop1',
				ISNULL(G.AssessAmount,'0')  As 'マンション金額',
				ISNULL(E.RealECD,'') as N'エリア Top1業者CD',
				ISNULL(F.REName,'') As 'エリアTop1',
				ISNULL(E.AssessAmount,'0')  As 'エリア金額',
				ISNULL(I.RealECD,'') as N'買取依頼会社CD',
				ISNULL(J.REName,'') As '買取依頼会社',
				ISNULL(I.AssessAmount,'0')  As '買取依頼金額',
				ISNULL(FORMAT(D.IntroDateTime, 'yyyy/MM/dd HH:mm:ss'),'') As '送客日時',
				ISNULL(CASE WHEN D.EndStatus =1 THEN
						CASE WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.SellerTermDateTime <= D.BuyerTermDateTime  THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss')  
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.BuyerTermDateTime <= D.SellerTermDateTime  THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss')   
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime  IS NULL THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NOT NULL THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NULL THEN NULL 
						END
				END,'') As '成約日時',
				ISNULL(CASE WHEN D.EndStatus =2 OR D.EndStatus = 3 THEN  
						CASE WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.SellerTermDateTime <= D.BuyerTermDateTime  THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss')  
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime IS NOT NULL AND D.BuyerTermDateTime <= D.SellerTermDateTime  THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss')   
							 WHEN D.SellerTermDateTime IS NOT NULL AND D.BuyerTermDateTime  IS NULL THEN FORMAT(D.SellerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NOT NULL THEN FORMAT(D.BuyerTermDateTime, 'yyyy/MM/dd HH:mm:ss') 
							 WHEN D.SellerTermDateTime IS NULL AND D.BuyerTermDateTime  IS NULL THEN NULL 
						END
				END,'') As '辞退日時'
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
	where ((@MansionName  IS NULL OR (A.MansionName like '%'+ @MansionName +'%')) and A.DeleteDateTime IS NULL)
	and ((@Range = '登録日'		and (@StartDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, A.InsertDateTime) <= @EndDate))
	OR (@Range = '詳細査定日'	and (@StartDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) <= @EndDate)) 
	OR (@Range	= '買取依頼日'	and (@StartDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) >= @StartDate) and (@EndDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) <= @EndDate))
	OR (@Range	= '送客日'		and (@StartDate IS NULL OR CONVERT(DATE, D.InsertDateTime) >= @StartDate)   and	(@EndDate IS NULL OR CONVERT(DATE, D.InsertDateTime)  <= @EndDate))
	OR (@Range	= '成約日'		and 
					((D.EndStatus =1 AND  @StartDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime) >= @StartDate)   and (@EndDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime)  <= @EndDate)) 
					OR  ((@StartDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime) >= @StartDate)   and (@EndDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime)  <= @EndDate)))
	OR (@Range	= '辞退日'		and 
					((D.EndStatus = 2 OR D.EndStatus = 3 AND  @StartDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime) >= @StartDate)   and (@EndDate IS NULL OR CONVERT(DATE, D.SellerTermDateTime)  <= @EndDate)) 
					OR  ((@StartDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime) >= @StartDate)   and (@EndDate IS NULL OR CONVERT(DATE, D.BuyerTermDateTime)  <= @EndDate))))
	 and ((@Chk_Mi = 1 and D.AssReqID IS NULL)
	OR (@Chk_Kan  = 1 and D.EasyAssDateTime IS NOT NULL and D.DeepAssDateTime IS NULL)
	OR (@Chk_Satei = 1 and D.DeepAssDateTime IS NOT NULL and D.PurchReqDateTime IS NULL)
	OR (@Chk_Kaitori = 1 and D.PurchReqDateTime IS NOT NULL and D.ConfirmDateTime IS NULL) 
	OR (@Chk_Kakunin = 1 and D.ConfirmDateTime IS NOT NULL and D.IntroDateTime IS NULL)
	OR (@Chk_Kosho =1 and D.IntroDateTime IS NOT NULL and (D.SellerTermDateTime IS NULL and D.BuyerTermDateTime IS NULL))
	OR (@Chk_Seiyaku = 1 and D.EndStatus = 1 and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	OR (@Chk_Urinushi = 1 and D.EndStatus = 2 and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	OR (@Chk_Kainushi = 1 and D.EndStatus = 3 and (D.SellerTermDateTime IS NOT NULL OR D.BuyerTermDateTime IS NOT NULL))
	) 
	Order by A.SellerMansionID
END













