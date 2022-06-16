IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_Select_M_Region_for_Map')
BEGIN
    DROP PROCEDURE [pr_r_asmc_Select_M_Region_for_Map]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_Select_M_Region_for_Map]
AS
BEGIN

    --SELECT
    --     MPR.RegionCD
    --    ,MRG.RegionName
    --    ,CASE MRG.RegionCD 
    --        WHEN '03' THEN 'kanto'
    --        WHEN '04' THEN 'tokai'
    --        WHEN '06' THEN 'kansai'
    --        WHEN '09' THEN 'kyushu'
    --        ELSE '' END AS CssName
    --    ,CASE MPR.PrefName WHEN '�����s' THEN '����' ELSE REPLACE(REPLACE(MPR.PrefName, '�{', ''), '��', '') END AS PrefName
    --FROM M_Pref MPR
    --INNER JOIN M_Region MRG ON MRG.RegionCD = MPR.RegionCD
    --WHERE MPR.RegionCD IS NOT NULL
    --ORDER BY MPR.RegionCD, MPR.DisplayOrder

    SELECT DISTINCT
         MPR.RegionCD
        ,MRG.RegionName
        ,CASE MRG.RegionCD 
            WHEN '01' THEN 'hokkaido'
            WHEN '02' THEN 'tohoku'
            WHEN '03' THEN 'kanto'
            WHEN '04' THEN 'tokai'
            WHEN '05' THEN 'hokuriku'
            WHEN '06' THEN 'kansai'
            WHEN '07' THEN 'chugoku'
            WHEN '08' THEN 'shikoku'
            WHEN '09' THEN 'kyushu'
            WHEN '10' THEN 'okinawa'
            ELSE '' END AS CssName
        ,CASE MRG.RegionCD 
            WHEN '01' THEN '�k�C��'
            WHEN '02' THEN '�X|���|�{��' + CHAR(13) + '�H�c|�R�`|����'
            WHEN '03' THEN '����|�_�ސ�' + CHAR(13) + '��t|���'
            WHEN '04' THEN '���m'
            WHEN '05' THEN '�V��|�x�R' + CHAR(13) + '�ΐ�|����'
            WHEN '06' THEN '���|����|���s'
            WHEN '07' THEN '����|����|���R' + CHAR(13) + '�L��|�R��'
            WHEN '08' THEN '����|����' + CHAR(13) + '���Q|���m'
            WHEN '09' THEN '����'
            WHEN '10' THEN '����'
            ELSE '' END AS PrefName
    FROM M_Pref MPR
    INNER JOIN M_Region MRG ON MRG.RegionCD = MPR.RegionCD
    WHERE MPR.RegionCD IS NOT NULL

END