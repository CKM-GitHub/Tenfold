IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_list_get_m_pref_and_citygpcd_and_citycd')
BEGIN
    DROP PROCEDURE [pr_t_mansion_list_get_m_pref_and_citygpcd_and_citycd]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_list_get_m_pref_and_citygpcd_and_citycd]
AS	
BEGIN
	;WITH CTECompany
AS
(
	select T1.PrefCD,T1.PrefName,T1.CityGPCD,T1.CityGPName,T1.CityCD,T1.CityName,T1.DisplayOrder from 
	(select ma.PrefCD,ma.PrefName,CityGPCD,CityGPName,CityCD,CityName,ma.DisplayOrder
	from M_Address  ma
	inner join M_Pref mp on mp.PrefCD = ma.PrefCD and mp.RegionCD is NoT NULL and AddressLevel=1  and NoDisplayFLG = 0
	and ma.DeleteDateTime is null
	) T1 

	inner join M_Address ma on ma.PrefCD = t1.PrefCD and ma.CityGPCD = t1.CityGPCD and AddressLevel=1  
	and NoDisplayFLG = 0
	and ma.DeleteDateTime is null 
	where 
	ma.CityName <> ( T1.CityGPName)
	
)

	-- Working Example
	SELECT 
		PrefCD,PrefName,CityGPCD, CityGPName,CityCD,CityName
	FROM CTECompany
	Group by PrefCD,PrefName,CityGPCD, CityGPName,CityCD,CityName,DisplayOrder
	ORDER BY DisplayOrder
END
