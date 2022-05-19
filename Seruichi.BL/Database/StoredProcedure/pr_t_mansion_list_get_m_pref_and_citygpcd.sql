IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_list_get_m_pref_and_citygpcd')
BEGIN
    DROP PROCEDURE [pr_t_mansion_list_get_m_pref_and_citygpcd]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_list_get_m_pref_and_citygpcd]
AS	
BEGIN
	select ma.PrefCD,ma.PrefName,CityGPCD,CityGPName
	from M_Address  ma
	inner join M_Pref mp on mp.PrefCD = ma.PrefCD and mp.RegionCD is NoT NULL and AddressLevel=1  and NoDisplayFLG = 0
	and ma.DeleteDateTime is null
	group by ma.PrefCD,ma.PrefName,CityGPCD,CityGPName,ma.DisplayOrder
	order by ma.DisplayOrder
END
