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

    DECLARE @SysDate date = GETDATE()

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
        ,ISNULL(MyRES2.Expired, 0)  AS Expired

        ,CAST(CASE WHEN VCon.ConditionSEQ IS NOT NULL THEN 1 ELSE 0 END  AS bit) AS IsShowCheckTab

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
                    AND   NoDisplayFLG = 0
                    GROUP BY CityCD                    
                ) AS MAN 
    ----------------------------------------------------------------------
    -- 登録会社数の参照
    --   エリア査定条件地区マスタ 
    --   市区町村コードでグループ化して事業者件数を取得
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT t1.CityCD ,Count(DISTINCT t1.RealECD) AS Kensu
                    FROM M_RECondAreaSec t1 
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY CityCD 
                ) AS   RES
    ----------------------------------------------------------------------
    -- 自社情報
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT 
                        MIN(t2.ValidFLG) AS ValidFLG
                    FROM M_RECondAreaSec t1
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= DATEADD(d, 7, @SysDate))
                    AND   t1.DisabledFlg = 0
                    GROUP BY CityCD 
                ) AS   MyRES

    --期限切れの有効データ
    OUTER APPLY (
                    SELECT TOP 1
                        1 AS Expired 
                    FROM M_RECondAreaSec t1
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND   t2.ExpDate < DATEADD(d, 7, @SysDate)
                    AND   t1.DisabledFlg = 0
                ) AS   MyRES2

    ----------------------------------------------------------------------
    -- 自社情報(Cityに対して設定した査定条件)
    ----------------------------------------------------------------------
    LEFT OUTER JOIN V_RECondAreaSec VCon 
    　ON  VCon.RealECD = @RealECD 
    　AND VCon.PrefCD = ADR.PrefCD
   　 AND VCon.CityCD = ADR.CityCD
   　 AND VCon.TownCD = '' --ViewではNULLは''

    WHERE ADR.NoDisplayFLG = 0  --表示対象外は除く
      AND ADR.AddressLevel = 2  --AddressLevel=1 は除く
      AND PRF.RegionCD = @RegionCD
  
    ORDER BY ADR.PrefCD, ADR.CityGPCD, ADR.CityCD, ADR.DisplayOrder

END