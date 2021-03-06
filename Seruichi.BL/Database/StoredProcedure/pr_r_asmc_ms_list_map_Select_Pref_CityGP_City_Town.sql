IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_ms_list_map_Select_Pref_CityGP_City_Town')
BEGIN
    DROP PROCEDURE [pr_r_asmc_ms_list_map_Select_Pref_CityGP_City_Town]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_ms_list_map_Select_Pref_CityGP_City_Town]
(
     @CityCDCsv varchar(max)
    ,@TownCDCsv varchar(max)
)
AS	
BEGIN

	SELECT DISTINCT
         ma.PrefCD
        ,mp.PrefName
        ,ma.CityGPCD
        ,ma.CityGPName
        ,ma.CityCD
        ,ma.CityName
        ,ma.TownCD
        ,ma.TownName
        ,mp.DisplayOrder AS DisplayOrder1
        ,ma.DisplayOrder AS DisplayOrder2
        ,CAST(ISNULL(w1.FLG,0) AS tinyint) AS CitySelected
        ,CAST(ISNULL(w2.FLG,0) AS tinyint) AS TownSelected
    FROM  M_Address ma
	INNER JOIN M_Pref mp 
       ON mp.PrefCD = ma.PrefCD
    INNER JOIN M_Mansion mm
       ON mm.TownCD = ma.TownCD
      AND mm.NoDisplayFLG = 0
    LEFT OUTER JOIN (SELECT 1 AS FLG, value AS CityCD FROM string_split(@CityCDCsv, ',')) AS w1
       ON w1.CityCD = ma.CityCD
    LEFT OUTER JOIN (SELECT 1 AS FLG, value AS TownCD FROM string_split(@TownCDCsv, ',')) AS w2
       ON w2.TownCD = ma.TownCD
    WHERE ma.AddressLevel = 2
      AND ma.NoDisplayFLG = 0
	  AND ma.DeleteDateTime IS NULL
      AND mp.RegionCD IS NOT NULL
	ORDER BY DisplayOrder1, PrefCD, DisplayOrder2, CityGPCD, CityCD, TownCD

END