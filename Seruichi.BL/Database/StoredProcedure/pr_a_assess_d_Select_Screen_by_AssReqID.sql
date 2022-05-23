
IF EXISTS (select * from sys.objects where name = 'pr_a_assess_d_Select_Screen_by_AssReqID')
BEGIN
    DROP PROCEDURE [pr_a_assess_d_Select_Screen_by_AssReqID]
END
GO
Create PROCEDURE [dbo].[pr_a_assess_d_Select_Screen_by_AssReqID]
	  @AssReqID as varchar(100), --= 'AR00000025',
	  @SellerCD as varchar(100)-- = 'SEL00000025'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
			
						 
						---ManInfo
						select 
						da.MansionName, 
						(Isnull(da.PrefName,'') + isnull(da.CityName,'') +Isnull(da.townname,'')+ Isnull(da.Address,'')) as Address,

						Isnull(ml.LineName,'') LineName,
						Isnull(ms.StationName,'') StationName,
						 (N'徒歩 '+　Cast(msm.Distance as varchar(20))　+N'分')  as Distance,

						 Isnull(da.SellerMansionID,'')SellerMansionID 
						 ,Isnull(msl.SellerName, '')SellerName
						 ,da.AssReqID
						 from
						 D_AssReq da
						left outer join M_SellerMansionStation msm on msm.SellerMansionID=da.SellerMansionID
						left outer join M_Station ms on ms.StationCD = msm.StationCD
						left outer join M_Line ml on ml.LineCD=ms.LineCD
						left outer join M_Seller msl on msl.SellerCD = @SellerCD
						where da.AssReqID= @AssReqID 
						order by msm.StationSEQ 
						

						--SpecInfo
						select 
				distinct 
			 ( Case 
			 when da.StructuralKBN = 0 then N'不明'
			 when da.StructuralKBN = 1 then N'SRC構造' 
			 when da.StructuralKBN = 2 then N'RC構造' 
			 when da.StructuralKBN = 3 then N'鉄骨造' 
			 when da.StructuralKBN = 4 then N'軽量鉄骨造' 
			 when da.StructuralKBN = 5 then N'重量鉄骨' 
			 when da.StructuralKBN = 6 then N'木造'  
			 else N'その他'    end 
			 ) as BuildingStructure,
			 (case when Cast(da.ConstYYYYMM as varchar(10)) = '0' or Isnull(da.ConstYYYYMM,'0')='0' then '0' else 
			 left (Cast (da.ConstYYYYMM as varchar(10)),4) +'/'+ Right (Cast (da.ConstYYYYMM as varchar(10)),2)
			 end) as ConstYYYYMM,
			 (Cast( da.RoomNumber  as varchar(20)) + N'号室') as RoomNumber,
			 (Cast(FORMAT(da.RoomArea, 'N2') as varchar(20)) + '㎡') as RoomArea,
			 (Cast(FORMAT(da.BalconyArea, 'N2') as varchar(20)) + '㎡') as BalconyArea,
			 (Case
			 when da.Direction =0 then N'不明' 
			 when da.Direction =1 then N'北向き' 
			 when da.Direction =2 then N'北東向き' 
			 when da.Direction =3 then N'東向き' 
			 when da.Direction =4 then N'南東向き' 
			 when da.Direction =5 then N'南向き' 
			 when da.Direction =6 then N'南西向き' 
			 when da.Direction =7 then N'西向き' 
			 else N'北西向き' End

			 ) as Direction,
			 (Cast(da.FloorType as varchar(10))  + N'部屋') as NumberofRoom,
			 (Case 
			 when da.BathKBN = 0 then N'不明'
			 when da.BathKBN = 1 then N'セパレート'
			 when da.BathKBN = 2 then N'ユニットバス'
			 else N'シャワーブース' End
			 ) as BathRoomandToilet,
			 (Case
			 when da.RightKBN = 0 then N'不明'
			 when da.RightKBN = 1 then N'所有権'
			 else N'借地権' End
			 ) as LandRight,
			 (Case
			 When da.CurrentKBN = 0 then N'不明'
			 When da.CurrentKBN = 1 then N'空室'
			 When da.CurrentKBN = 2 then N'賃貸'
			 else N'サブリース' end
			 ) as PresentState,
			 (Case
			 When da.ManagementKBN =0 then N'不明'
			 When da.ManagementKBN =1 then N'委託管理'
			 else N'自主管理' End 
			 ) as ManagementSystem,
			 (case
			 when da.DesiredTime = 0 then N'不明'
			 when da.DesiredTime = 0 then N'２週間以内'
			 when da.DesiredTime = 0 then N'１ヵ月以内'
			 when da.DesiredTime = 0 then N'３ヵ月以内'
			 else N'３ヵ月以降' End
			 ) as DesiredTimeSale, 
			 Case when (ISNULL(FORMAT(CONVERT(MONEY, da.RentFee), '###,###'),'0') +''+N'円') = N'円' then N'0円' else ISNULL(FORMAT(CONVERT(MONEY, da.RentFee), '###,###'),'0') + N'円' end as RentFee,
			  
			 Case when (ISNULL(FORMAT(CONVERT(MONEY, da.ManagementFee), '###,###'),'0') +''+N'円') = N'円' then N'0円' else ISNULL(FORMAT(CONVERT(MONEY, da.ManagementFee), '###,###'),'0') + N'円' end as ManagementFee,
			  
			 Case when (ISNULL(FORMAT(CONVERT(MONEY, da.RepairFee), '###,###'),'0') +''+N'円') = N'円' then N'0円' else ISNULL(FORMAT(CONVERT(MONEY, da.RepairFee), '###,###'),'0') + N'円' end as RepairFee,
			  
			 Case when (ISNULL(FORMAT(CONVERT(MONEY, da.PropertyTax), '###,###'),'0') +''+N'円') = N'円' then N'0円' else ISNULL(FORMAT(CONVERT(MONEY, da.PropertyTax), '###,###'),'0') + N'円' end as PropertyFee,

			 Case when (ISNULL(FORMAT(CONVERT(MONEY, da.ExtraFee), '###,###'),'0') +''+N'円') = N'円' then N'0円' else ISNULL(FORMAT(CONVERT(MONEY, da.ExtraFee), '###,###'),'0') + N'円' end as ExtraFee
			  
				from
						 D_AssReq da
						left outer join M_SellerMansionStation msm on msm.SellerMansionID=da.SellerMansionID
						left outer join M_Station ms on ms.StationCD = msm.StationCD
						left outer join M_Line ml on ml.LineCD=ms.LineCD
						left outer join M_Seller msl on msl.SellerCD = @SellerCD  
						where da.AssReqID= @AssReqID 


						--ManRank
						select 

			 mri.REFaceImage,
			 mre.Prefname,
			 mrs.ReStaffName,
			 mre.ContRateMansion,
			-- ISNULL(FORMAT(dar.AssessAmount, '#,#'),0) AssessAmount , 
			 ISNULL(FORMAT(CONVERT(MONEY, dar.AssessAmount), '###,###'),'0') AssessAmount,
			 dar.AssID,
			 dar.AssSEQ,
			 dar.AssReqID,
			 dar.RealECD,
			 dar.REStaffCD,
			 dar.AssessType1,
			 dar.AssessType2,
			 dar.ConditionSEQ,
			 dar.Rank,
			  			 ( Case When (select Num1 from M_MultPurpose where DataID=119) = 1 then 'none' else '' end ) as Flg

			 --select *
			  from D_AssessResult dar 
			 left outer join M_RealEstate mre on mre.RealECD= dar.RealECD
			 left outer join M_Restaff mrs on mrs.RealECD = dar.RealECD
									and mrs.REstaffCD=dar.ReStaffCD
			 left outer join M_REImage mri on mri.RealECD = dar.RealECD
									and mri.RestaffCD=dar.RestaffCD
			  where 
			  dar.AssReqID=@AssReqID  
			  and dar.AssessType1 = 3  
			  and dar.AssKBN= 2  
			  and dar.DeleteDatetime is NULL  
			  and dar.Rank <= 5
			  order by  dar.Rank, dar.RealECD


						--AreaRank
						select 

			 mri.REFaceImage,
			 mre.Prefname,
			 mrs.ReStaffName,
			 mre.ContRateMansion,
			 --ISNULL(FORMAT(dar.AssessAmount, '#,#'),0) AssessAmount , 
			 ISNULL(FORMAT(CONVERT(MONEY, dar.AssessAmount), '###,###'),'0') AssessAmount,
			 dar.AssID,
			 dar.AssSEQ,
			 dar.AssReqID,
			 dar.RealECD,
			 dar.REStaffCD,
			 dar.AssessType1,
			 dar.AssessType2,
			 dar.ConditionSEQ,
			 dar.Rank,
			 ( Case When (select Num1 from M_MultPurpose where DataID=119) = 1 then 'none' else '' end ) as Flg
			 --select *
			  from D_AssessResult dar 
			 left outer join M_RealEstate mre on mre.RealECD= dar.RealECD
			 left outer join M_Restaff mrs on mrs.RealECD = dar.RealECD
									and mrs.REstaffCD=dar.ReStaffCD
			 left outer join M_REImage mri on mri.RealECD = dar.RealECD
									and mri.RestaffCD=dar.RestaffCD
			  where 
			  dar.AssReqID=@AssReqID  
			  and dar.AssessType1 = 1 
			  and dar.AssKBN= 2  
			  and dar.DeleteDatetime is NULL  
			  and dar.Rank <= 5
			  order by  dar.Rank, dar.RealECD

END