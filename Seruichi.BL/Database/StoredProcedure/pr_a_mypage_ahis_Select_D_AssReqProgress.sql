IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_ahis_Select_D_AssReqProgress')
BEGIN
    DROP PROCEDURE [pr_a_mypage_ahis_Select_D_AssReqProgress]
END
GO
Create PROCEDURE [dbo].[pr_a_mypage_ahis_Select_D_AssReqProgress]
	
	@SellerCD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
								A.SellerCD,
								A.SellerName,
							Cast(ROW_NUMBER( ) OVER(order by A.InsertDateTime desc) as int) as No,
							Cast(A.AssReqID as varchar(10)) as ID,

						    (
							Case When A.EasyAssDateTime is not null and A.DeepAssDateTime is null and D.DeleteDateTime is null then  N'簡易査定'  
							  When A.DeepAssDateTime is not null and A.PurchReqDateTime is null and D.DeleteDateTime is null then  N'詳細査定' 
							  When A.PurchReqDateTime is not null and A.ConfirmDateTime is null and D.DeleteDateTime is null then  N'買取依頼' 
							  When A.ConfirmDateTime is not null and A.IntroDateTime is null and D.DeleteDateTime is null then  N'確認中'  
							  When A.IntroDateTime is not null and A.SellerTermDateTime is null and A.BuyerTermDateTime is null and D.DeleteDateTime is null then  N'交渉中'  
							  When A.EndStatus =1 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then N'成約'  
							  When A.EndStatus =2 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then N'売主辞退' else
							--Case When A.EndStatus =3 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then
							 N'買主辞退'   
							end
							) as [Status], 
								  (
							Case When A.EasyAssDateTime is not null and A.DeepAssDateTime is null and D.DeleteDateTime is null then  '01'  
							  When A.DeepAssDateTime is not null and A.PurchReqDateTime is null and D.DeleteDateTime is null then  '02' 
							  When A.PurchReqDateTime is not null and A.ConfirmDateTime is null and D.DeleteDateTime is null then  '03' 
							  When A.ConfirmDateTime is not null and A.IntroDateTime is null and D.DeleteDateTime is null then  '04'  
							  When A.IntroDateTime is not null and A.SellerTermDateTime is null and A.BuyerTermDateTime is null and D.DeleteDateTime is null then  '05'  
							  When A.EndStatus =1 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then '06'  
							  When A.EndStatus =2 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then '07' else
							--Case When A.EndStatus =3 and (A.SellerTermDateTime is not null or  A.BuyerTermDateTime is not null) and D.DeleteDateTime is null then
							 '08'   
							end
							) as [StatusIndex], 
							(Cast (D.MansionName as varchar(30) ) + ' '+ Cast(Isnull(D.RoomNumber,'') as varchar(30))) as MansionName,
							Cast((D.PrefName + D.CityName + D.TownName + D.Address) as varchar(160)) as Address,
							Cast(Isnull(C.REName,'') as varchar(30)) as REName ,
							ISNULL(FORMAT(CONVERT(MONEY, A.AssessAmount), '###,###'),'0') +' '+N'円'as AssessAmount,
							ISNULL(FORMAT(A.DeepAssDateTime, 'yyyy/MM/dd HH:mm:ss'),'') DeepAssDateTime,
							--Isnull(A.DeepAssDateTime,'') as DeepAssDateTime, 
							A.AssReqID as AssReqID, 
							A.SellerMansionID as SellerMansionID ,
							ISNULL(FORMAT(Isnull(A.InsertDateTime,''), 'yyyy/MM/dd HH:mm:ss'),'') as IDT
							 
							from D_AssReqProgress A
							left outer join D_AssReq B on A.AssReqID= B.AssReqID
							left outer join M_RealEstate C on C.RealECD = A.RealECD
							left outer join M_SellerMansion D on D.SellerMansionID = A.SellerMansionID 
							
							where ((@SellerCD is null ) or A.SellerCD= @SellerCD) order by A.InsertDateTime desc

END