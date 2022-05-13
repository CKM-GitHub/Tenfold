IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_address_Select_Cities_by_RegionCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_address_Select_Cities_by_RegionCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_address_Select_Cities_by_RegionCD]
(
    @RegionCD   varchar(2)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    SELECT DISTINCT 
         ADR.PrefCD
        ,ADR.PrefName
        ,ADR.CityGPCD               AS CityGroupCD
        ,ADR.CityGPName             AS CityGroupName
        ,ADR.CityCD
        ,ADR.CityName
        ,ISNULL(MAN.Kensu,0)        AS MansionCount
        ,ISNULL(RES.Kensu,0)        AS RealEstateCount
        ,ADR.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 9)  AS ValidFLG
        ,MyRES.ExpDate

    FROM M_Address    ADR --住所マスタ
    INNER JOIN M_Pref PRF ON ADR.PrefCD = PRF.PrefCD   -- 地方マスタ 
    ----------------------------------------------------------------------
    -- 登録マンション件数の参照
    -- 　マンションマスタ
    --   市区町村コードでグループ化して登録マンション数を取得
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT CityCD, Count(MansionCD) AS Kensu
                    FROM M_Mansion 
                    WHERE CityCD = ADR.CityCD
                    GROUP　BY CityCD                    
                ) AS MAN 
    ----------------------------------------------------------------------
    -- 登録会社数の参照
    --   エリア査定条件地区マスタ 
    --   市区町村コードでグループ化して事業者件数を取得
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT CityCD ,Count(RealECD) AS Kensu
                    FROM M_RECondAreaSec  
                    WHERE CityCD = ADR.CityCD
                    GROUP BY CityCD 
                ) AS   RES
    ----------------------------------------------------------------------
    -- 自社情報
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT 
                         MAX(t2.ValidFLG)   AS ValidFLG
                        ,MAX(t2.ExpDate)    AS ExpDate
                    FROM M_RECondAreaSec t1
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND   t2.ExpDate > GETDATE()
                    GROUP BY CityCD 
                ) AS   MyRES

    WHERE ADR.NoDisplayFLG = 0  --表示対象外は除く
      AND ADR.AddressLevel = 2  --AddressLevel=1 は除く
      AND PRF.RegionCD = @RegionCD
  
    ORDER BY ADR.PrefCD, ADR.CityGPCD, ADR.CityCD, ADR.DisplayOrder

END