IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_ms_reged_list_get_m_pref_and_citygpcd')
BEGIN
    DROP PROCEDURE [pr_r_asmc_ms_reged_list_get_m_pref_and_citygpcd]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_ms_reged_list_get_m_pref_and_citygpcd]
AS	
BEGIN
	--select ma.PrefCD,ma.PrefName,CityGPCD,CityGPName
	--from M_Address  ma
	--inner join M_Pref mp on mp.PrefCD = ma.PrefCD and mp.RegionCD is NoT NULL and AddressLevel=1  and NoDisplayFLG = 0
	--and ma.DeleteDateTime is null
	--group by ma.PrefCD,ma.PrefName,CityGPCD,CityGPName,ma.DisplayOrder
	--order by ma.DisplayOrder

	
	select 
	 T.CityGPCD
	,T.PrefCD
	,T.PrefName
	,T.CityGPName
	,T.DisplayOrder 
	,T.AddressLevel
	FROM (select Distinct(ma.CityGPCD) ,ma.PrefCD,ma.PrefName,ma.CityGPName,ma.DisplayOrder ,(case when  ma.CityGPCD <> ma.CityCD then '2' else '1' end) as 'AddressLevel'
	from M_Address  ma
	inner join M_Pref mp on mp.PrefCD = ma.PrefCD 
	and mp.RegionCD is NoT NULL 
	and ma.AddressLevel=1  
	and ma.NoDisplayFLG = 0
	and ma.DeleteDateTime is null
	group by ma.PrefCD
	,ma.PrefName
	,ma.CityGPCD
	,ma.CityGPName
	,ma.DisplayOrder
	,ma.CityCD) T
	order by DisplayOrder

END