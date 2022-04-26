IF EXISTS (select * from sys.objects where name = 'pr_t_seller_List_csv_generate')
BEGIN
    DROP PROCEDURE [pr_t_seller_List_csv_generate]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_List_csv_generate]

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

    -- Insert statements for procedure here

	--IF @EndDate IS NULL set @EndDate='2099/12/31'

	SELECT 
		Row_Number() Over (Order By A.SellerCD) As [NO],
		CASE   
			WHEN ISNULL(G.c,0) > 0 THEN N'交渉中' 
			WHEN ((ISNULL(G.c,0) = 0) and (ISNULL(E.a,0) != ISNULL(F.b,0))) THEN N'見込'
			WHEN ((ISNULL(G.c,0) = 0) and (ISNULL(E.a,0) = ISNULL(F.b,0))) THEN N'終了'
		 END as N'ステータス',
		 CASE   
			WHEN A.InvalidFLG = 0 THEN ''
			ELSE N'無効'
		 END as N'無効会員',
		 ISNULL(A.SellerCD,'') as'売主CD',
		 ISNULL(A.SellerName,'') as '売主名',
		 ISNULL(A.SellerKana,'') as 'カナ名',
		 ISNULL(A.MailAddress,'') as 'メールアドレス'	,
		 ISNULL(A.Birthday,'') as '生年月日',
		 ISNULL(A.ZipCode1 +'-'+ A.ZipCode2,'') as '郵便番号',
		 ISNULL(A.PrefName,'') as '都道府県名',
		 ISNULL(A.CityName,'') as '市区町村名',
		 ISNULL(A.TownName,'') as '町域名',
		 ISNULL(A.Address1,'') as '番地',
		 ISNULL(A.Address2,'') as '建物名･部屋番号',
		 ISNULL(A.HandyPhone,'') as '携帯電話番号',
		 ISNULL(A.HousePhone,'') as '固定電話番号',
		 ISNULL(A.Fax,'') as 'FAX番号',
		 ISNULL(FORMAT (A.InsertDateTime, 'yyyy/MM/dd HH:mm:ss'),'') as '登録日時',
		 ISNULL(FORMAT (D.DeepAssDateTime, 'yyyy/MM/dd HH:mm:ss'),'') as '査定依頼日時',
		 ISNULL(FORMAT (D.PurchReqDateTime, 'yyyy/MM/dd HH:mm:ss'),'') as '買取依頼日時	',
		 ISNULL(E.a,'') as '登録数',
		 ISNULL(F.b,'') as'成約数'
		   
		From M_Seller A 
		outer apply (select top 1 C.DeepAssDateTime as DeepAssDateTime ,C.PurchReqDateTime as PurchReqDateTime
						from D_AssReqProgress C
						where C.SellerCD = A.SellerCD  
						and C.DeepAssDateTime is not null  
						and C.DeleteDateTime is null 
						Order by C.InsertDateTime desc) D
		outer apply (SELECT Count(SellerMansionID)as a
					FROM M_SellerMansion B 
					WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null 
					AND (B.HoldingStatus <> 5) GROUP BY B.SellerCD ) E
		outer apply (SELECT Count(SellerMansionID)as b
					FROM  M_SellerMansion B 
					WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null 
					AND (B.HoldingStatus =4) GROUP BY B.SellerCD ) F
		outer apply (Select Count(SellerMansionID) as c 					From M_SellerMansion B 					WHERE B.SellerCD = A.SellerCD AND B.DeleteDateTime is Null 					AND (B.HoldingStatus = 2 or B.HoldingStatus = 3)GROUP BY B.SellerCD ) G

    where ((@ValidCheck=1 and A.InvalidFLG = '0') or (@InValidCheck = 1 and A.InvalidFLG = '1')) 
	--AND (@SellerName is null or ((A.SellerName Like '%'+@SellerName+'%') or (A.SellerCD Like '%'+@SellerName+'%')))
	AND ((@PrefNameSelect = N'全国' and (A.PrefName is not Null)) 
		or (@PrefNameSelect != N'全国' and (A.PrefName = @PrefNameSelect )))
	AND ((@RangeSelect = 0	and (@StartDate IS NULL OR CONVERT(DATE, A.InsertDateTime)  >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, A.InsertDateTime) <= @EndDate))
		OR (@RangeSelect =1	and (@StartDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) >= @StartDate)  and  (@EndDate IS NULL OR CONVERT(DATE, D.DeepAssDateTime) <= @EndDate)) 
		OR (@RangeSelect = 2	and (@StartDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) >= @StartDate) and (@EndDate IS NULL OR CONVERT(DATE, D.PurchReqDateTime) <= @EndDate)))
	AND ((@negtiatioinsCheck =1 and ISNUll(G.c,0) > 0 ) 
		or (@expectedCheck = 1 and (ISNUll(G.c,0) = 0 and ISNULL(E.a,0) != ISNULL(F.b,0))) 
		or (@endCheck = 1 and (ISNUll(G.c,0) = 0 and ISNULL(E.a,0) = ISNULL(F.b,0))))

END
