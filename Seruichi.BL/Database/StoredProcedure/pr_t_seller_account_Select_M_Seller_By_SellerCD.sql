IF EXISTS (select * from sys.objects where name = 'pr_t_seller_account_Select_M_Seller_By_SellerCD')
BEGIN
    DROP PROCEDURE [pr_t_seller_account_Select_M_Seller_By_SellerCD] 
END
GO
CREATE PROCEDURE [dbo].[pr_t_seller_account_Select_M_Seller_By_SellerCD]
	-- Add the parameters for the stored procedure here
   @SellerCD	VARCHAR(10),
   @Type		TINYINT
AS
BEGIN
	SET NOCOUNT ON;

	IF(@Type = 0)
	BEGIN
		 
		SELECT  PossibleTimes   FROM M_Seller where SellerCD =@SellerCD
	END
	ELSE
	BEGIN
	SELECT DISTINCT IIF(IsNull(InvalidFLG,'0')='1', '無効会員','有効会員') AS 'InvalidFLG',
		s.TestFLG,
		ISNULL(s.SellerKana, '') AS N'SellerKana',
		ISNULL(s.SellerName, '') AS N'SellerName',
		ISNULL(s.SellerCD, '') AS N'SellerCD',
		ISNULL(s.PrefName, '') AS N'PrefName',
		ISNULL(s.CityName, '') AS N'CityName',
		ISNULL(s.TownName, '') AS N'TownName',
		ISNULL(s.Address1, '') AS N'Address1',
		ISNULL(s.Address2, '') AS N'Address2',
		ISNULL(s.HousePhone, '') AS N'HousePhone',
		ISNULL(s.HandyPhone, '') AS N'HandyPhone',
		ISNULL(s.MailAddress, '') AS N'MailAddress',
		ISNULL((SELECT FORMAT(CONVERT(MONEY, SUM(IntroAmounts)), '###,###') FROM D_IntroReq
			WHERE SellerCD = s.SellerCD AND IntroYYYYMM	= (SELECT LEFT(CONVERT(VARCHAR, GetDate(), 112), 6)) AND DeleteDateTime IS NULL																					
			GROUP BY SellerCD),'0') + '円' AS N'今月課金',
		(SELECT CAST(ISNULL(COUNT(AssReqID), 0) AS VARCHAR) + '件' FROM D_AssReqProgress
			WHERE SellerCD = s.SellerCD AND DeepAssDateTime IS NOT NULL AND DeleteDateTime IS NULL) AS N'査定数',
		(SELECT CAST(ISNULL(COUNT(AssReqID), 0) AS VARCHAR) + '件' FROM D_AssReqProgress
			WHERE SellerCD = s.SellerCD AND EndStatus = 1 AND (SellerTermDateTime IS NOT NULL OR BuyerTermDateTime IS NOT NULL) AND DeleteDateTime IS NULL) AS N'成約数',
		(SELECT CAST(ISNULL(COUNT(AssReqID), 0) AS VARCHAR) + '件' FROM D_AssReqProgress
			WHERE SellerCD = s.SellerCD AND EndStatus = 2 AND (SellerTermDateTime IS NOT NULL OR BuyerTermDateTime IS NOT NULL)) AS N'売主辞退数',
		(SELECT CAST(ISNULL(COUNT(AssReqID), 0) AS VARCHAR) + '件' FROM D_AssReqProgress
			WHERE SellerCD = s.SellerCD AND EndStatus = 3 AND (SellerTermDateTime IS NOT NULL OR BuyerTermDateTime IS NOT NULL)) AS N'買主辞退数',
		ISNULL(FORMAT(s.InsertDateTime, 'yyyy/MM/dd'),'') AS N'会員登録日',
		ISNULL(FORMAT(D.DeepAssDateTime, 'yyyy/MM/dd'),'') AS N'査定依頼日',
		ISNULL(FORMAT(P.PurchReqDateTime, 'yyyy/MM/dd'),'') AS N'買取依頼日',
		ISNULL(FORMAT(L.LoginDateTime, 'yyyy/MM/dd'),'') AS N'最終ログイン日'
	FROM M_Seller s	
	OUTER APPLY (SELECT TOP 1 DeepAssDateTime
					FROM D_AssReqProgress
					WHERE SellerCD = s.SellerCD
					AND DeleteDateTime IS NULL
					ORDER BY DeepAssDateTime) D	
	OUTER APPLY (SELECT TOP 1 PurchReqDateTime
					FROM D_AssReqProgress
					WHERE SellerCD = s.SellerCD
					AND DeleteDateTime IS NULL
					ORDER BY DeepAssDateTime) P
	OUTER APPLY (SELECT TOP 1 LoginDateTime
					FROM L_Login
					WHERE LoginID = s.SellerCD
					AND LoginKBN = 1
					ORDER BY LoginDateTime DESC) L
	WHERE s.SellerCD = @SellerCD
	END
END