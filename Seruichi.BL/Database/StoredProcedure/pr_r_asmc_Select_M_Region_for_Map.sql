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
    --    ,CASE MPR.PrefName WHEN '東京都' THEN '東京' ELSE REPLACE(REPLACE(MPR.PrefName, '府', ''), '県', '') END AS PrefName
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
            WHEN '01' THEN '北海道'
            WHEN '02' THEN '青森|岩手|宮城' + CHAR(13) + '秋田|山形|福島'
            WHEN '03' THEN '東京|神奈川' + CHAR(13) + '千葉|埼玉'
            WHEN '04' THEN '愛知'
            WHEN '05' THEN '新潟|富山' + CHAR(13) + '石川|福井'
            WHEN '06' THEN '大阪|兵庫|京都'
            WHEN '07' THEN '鳥取|島根|岡山' + CHAR(13) + '広島|山口'
            WHEN '08' THEN '徳島|香川' + CHAR(13) + '愛媛|高知'
            WHEN '09' THEN '福岡'
            WHEN '10' THEN '沖縄'
            ELSE '' END AS PrefName
    FROM M_Pref MPR
    INNER JOIN M_Region MRG ON MRG.RegionCD = MPR.RegionCD
    WHERE MPR.RegionCD IS NOT NULL

END