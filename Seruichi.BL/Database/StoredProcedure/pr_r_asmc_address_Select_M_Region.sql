IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_address_Select_M_Region')
BEGIN
    DROP PROCEDURE [pr_r_asmc_address_Select_M_Region]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_address_Select_M_Region]
AS
BEGIN

    SELECT DISTINCT
         MPR.RegionCD
        ,MRG.RegionName
        ,MRG.DisplayOrder
    FROM M_Pref MPR
    INNER JOIN M_Region MRG ON MRG.RegionCD = MPR.RegionCD
    WHERE MPR.RegionCD IS NOT NULL
    ORDER BY MRG.DisplayOrder

END