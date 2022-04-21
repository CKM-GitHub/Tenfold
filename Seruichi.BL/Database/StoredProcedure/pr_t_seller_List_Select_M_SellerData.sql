IF EXISTS (select * from sys.objects where name = 'pr_t_seller_List_Select_M_SellerData')
BEGIN
    DROP PROCEDURE [pr_t_seller_List_Select_M_SellerData]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_List_Select_M_SellerData]
	-- Add the parameters for the stored procedure here
	@ValidCheck as tinyint,
	@InValidCheck as tinyint,
	--@SellerCD as varchar(10),
	@SellerName as varchar(50),
	@PrefNameSelect as varchar(10),
	@RangeSelect as tinyint,
	@StartDate as Date,
	@EndDate as Date,
	@expectedCheck as tinyint,
	@negtiatioinsCheck as tinyint,
	@endCheck as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   --IF @EndDate IS NULL set @EndDate='2099/12/31'

   DECLARE @g INT
		IF EXISTS (SELECT 1 FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND (B.HoldingStatus = 2 or B.HoldingStatus = 3))			SELECT @g = Count(SellerMansionID) FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND (B.HoldingStatus = 2 or B.HoldingStatus = 3) GROUP BY SellerMansionID 		ELSE			SELECT @g = 0

   DECLARE @e INT
		IF EXISTS (SELECT 1 FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND B.HoldingStatus != 5)			SELECT @e = Count(SellerMansionID) FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND B.HoldingStatus != 5 GROUP BY SellerMansionID 		ELSE			SELECT @e = 0

  DECLARE @f INT
		IF EXISTS (SELECT 1 FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND B.HoldingStatus = 4)			SELECT @f = Count(SellerMansionID) FROM M_Seller A OUTER APPLY M_SellerMansion B WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null AND B.HoldingStatus = 4 GROUP BY SellerMansionID 		ELSE			SELECT @f = 0

	SELECT 
		   Row_Number() Over (Order By A.SellerCD) As [NO],
		   ISNULL(CASE   
				WHEN G.c > 0 THEN N'交渉中' 
				WHEN ((G.c = 0) and (E.a != F.b)) THEN N'見込'
				WHEN ((G.c = 0) and (E.a = F.b)) THEN N'終了'
		   END,'') as N'ステータス',
		   CASE   
				WHEN A.InvalidFLG = 0 THEN ''
				ELSE N'✓'
		   END as N'無効会員',
		   ISNULL(A.SellerCD,'') as N'売主CD',
		   ISNULL(A.SellerName,'') as N'売主名',
		   ISNULL(A.PrefName,'')as N'居住地',
		   
		   ISNULL(FORMAT (A.InsertDateTime, 'yyyy/MM/dd hh:mm:ss'),'') as N'登録日時',
		   ISNULL(FORMAT (D.DeepAssDateTime, 'yyyy/MM/dd hh:mm:ss'),'') as N'査定依頼日時',
		   ISNULL(FORMAT (D.PurchReqDateTime, 'yyyy/MM/dd hh:mm:ss'),'') as N'買取依頼日時',
		   ISNULL(E.a,'') as N'登録数',
		   ISNULL(F.b,'') as N'成約数'
		   
		From M_Seller A 
		outer apply (select top 1 C.DeepAssDateTime as DeepAssDateTime ,C.PurchReqDateTime as PurchReqDateTime
						from D_AssReqProgress C
						where C.SellerCD = A.SellerCD  
						and C.DeepAssDateTime is not null  
						and C.DeleteDateTime is null 
						Order by C.InsertDateTime desc) D
		outer apply (select @e AS a ) E
		outer apply (select @f As b ) F
		outer apply (SELECT @g AS c) G

    where ((@ValidCheck='1' and A.InvalidFLG = '0') or (@InValidCheck = '1' and A.InvalidFLG = '1')) 
	AND (@SellerName is null or ((A.SellerName Like '%'+@SellerName+'%') or (A.SellerCD Like '%'+@SellerName+'%')))
	AND ((@PrefNameSelect = N'全国' and (A.PrefName is not Null)) 
		or (@PrefNameSelect != N'全国' and (A.PrefName = @PrefNameSelect )))
	AND ((@RangeSelect = '0'	and (@StartDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, A.InsertDateTime) <= @EndDate))
		OR (@RangeSelect ='1'	and (@StartDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) <= @EndDate)) 
		OR (@RangeSelect = '2'	and (@StartDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) >= @StartDate) and (@EndDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) <= @EndDate)))
	AND ((@negtiatioinsCheck ='1' and G.c > 0 ) 
		or (@expectedCheck = '1' and (G.c = 0 and E.a != F.b)) 
		or (@endCheck = '1' and (G.c = 0 and E.a = F.b)))

		Order by A.SellerCD,A.SellerKana
END
