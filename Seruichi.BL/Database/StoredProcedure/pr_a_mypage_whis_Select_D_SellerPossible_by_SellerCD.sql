IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_whis_Select_D_SellerPossible_by_SellerCD')
BEGIN
    DROP PROCEDURE [pr_a_mypage_whis_Select_D_SellerPossible_by_SellerCD]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_whis_Select_D_SellerPossible_by_SellerCD]
(
    @SellerCD               varchar(10)
)
AS
BEGIN
   SELECT
		ROW_NUMBER( ) OVER(order by InsertDateTime desc) as No,
		PossibleID,
		ISNULL(FORMAT(ChangeDateTime, 'yyyy/MM/dd HH:mm:ss'),'')as ChangeDateTime,
		ChangeCount,
   Case When ChangeFee=0 
		Then '' 
		Else FORMAT(CONVERT(MONEY, ChangeFee), '###,###')+' '+N'円' 
		End as ChangeFee,
    Case When PaymentFlg =2 Then 'NG'
		When PaymentFlg =1 Then	'OK' 
		When PaymentFlg =0 Then '' End As PaymentFlg
		
  FROM	D_SellerPossible
  WHERE SellerCD = @SellerCD
  Order By InsertDateTime Desc

END