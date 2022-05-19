IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_list_get_m_pref')
BEGIN
    DROP PROCEDURE [pr_t_mansion_list_get_m_pref]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_list_get_m_pref]
AS	
BEGIN
	select PrefName,PrefCD from M_Pref
	where RegionCD is Not NULL
	order by DisplayOrder
END
