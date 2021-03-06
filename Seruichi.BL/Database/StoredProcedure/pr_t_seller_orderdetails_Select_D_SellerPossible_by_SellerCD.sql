IF EXISTS (select * from sys.objects where name = 'pr_t_seller_orderdetails_Select_D_SellerPossible_by_SellerCD')
BEGIN
    DROP PROCEDURE [pr_t_seller_orderdetails_Select_D_SellerPossible_by_SellerCD]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_orderdetails_Select_D_SellerPossible_by_SellerCD]
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
   Case When PaymentFlg=2 or 
			 PaymentFlg=0 Then '' 
		When PaymentFlg=1 and 
			 ChangeFee !=0 Then FORMAT(CONVERT(MONEY, ChangeFee), '###,###')
		When ChangeFee= 0 Then ''
		End as ChangeFee,
   Case When PaymentFlg =2 Then '失敗'
		When PaymentFlg =1 Then '完了'
		End As PaymentFlg
		
  FROM	D_SellerPossible
  WHERE SellerCD = @SellerCD
  Order By InsertDateTime Desc

END