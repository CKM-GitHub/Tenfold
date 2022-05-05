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
							ROW_NUMBER( ) OVER(order by A.InsertDateTime desc) as No,
							A.AssReqID as ID,

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
							cast (D.MansionName as varchar(30) ) as MansionName,
							(D.PrefName + D.CityName + D.TownName + D.Address) as Address,
							Cast(C.REName as varchar(30)) as REName,
							(case when cast (Isnull(A.AssessAmount,0) as int ) = 0 then null else A.AssessAmount end) as AssessAmount,
							A.DeepAssDateTime as DeepAssDateTime, 
							A.AssReqID as AssReqID, 
							A.SellerMansionID as SellerMansionID 
							from D_AssReqProgress A
							left outer join D_AssReq B on A.AssReqID= B.AssReqID
							left outer join M_RealEstate C on C.RealECD = A.RealECD
							left outer join M_SellerMansion D on D.SellerMansionID = A.SellerMansionID 
							
							where ((@SellerCD is null ) or A.SellerCD= @SellerCD) order by A.InsertDateTime desc

END