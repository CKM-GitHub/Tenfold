IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_address_Select_Towns_by_Cities')
BEGIN
    DROP PROCEDURE [pr_r_asmc_address_Select_Towns_by_Cities]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_address_Select_Towns_by_Cities]
(
    @CitycdCsv  varchar(max)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    DECLARE @SysDate datetime = GETDATE()

    SELECT DISTINCT
         ADR.PrefCD
        ,ADR.PrefName
        ,ADR.CityCD
        ,ADR.CityName
        ,ADR.TownCD
        ,ADR.TownName
        ,ADR.TownKana
        ,ISNULL(MAN.Kensu,0)        AS MansionCount
        ,ISNULL(RES.Kensu,0)        AS RealEstateCount
        ,ADR.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 0)  AS ValidFLG
        --ValidFLGがNULLでExistsFlgが１の場合、査定データがすべて無効か、期限切れということ
        ,CASE WHEN MyRES.ValidFLG IS NULL THEN MyRES2.ExistsFlg ELSE 0 END AS ExpirationFlag

    FROM M_Address   ADR          ---住所マスタ 
    ----------------------------------------------------------------------
    -- 登録マンション件数の参照
    -- 　マンションマスタ
    --   町域コードでグループ化して登録マンション数を取得
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT TownCD, Count(MansionCD) AS Kensu
                FROM M_Mansion  
                WHERE TownCD = ADR.TownCD
                AND   NoDisplayFLG = 0
                GROUP BY TownCD 
                ) AS MAN 
    ----------------------------------------------------------------------
    -- 登録会社数の参照
    --   エリア査定条件地区マスタ 
    --   町域コードでグループ化して事業者件数を取得
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT t1.TownCD, Count(DISTINCT t1.RealECD) AS Kensu
                FROM M_RECondAreaSec t1
                INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                AND   t2.ExpDate > @SysDate
                AND   t1.DisabledFlg = 0
                GROUP BY TownCD
                ) AS RES
    ----------------------------------------------------------------------
    -- 自社情報
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT 
                     MIN(t2.ValidFLG) + 1   AS ValidFLG
                FROM M_RECondAreaSec t1
                INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE t1.RealECD = @RealECD
                AND   t1.TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                AND   t2.ExpDate > @SysDate
                AND   t1.DisabledFlg = 0
                GROUP BY CityCD 
                ) AS   MyRES

    OUTER APPLY (
                SELECT TOP 1
                     1 AS ExistsFlg
                FROM M_RECondAreaSec t1
                INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE t1.RealECD = @RealECD
                AND   t1.TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                ) AS   MyRES2

    WHERE ADR.NoDisplayFLG = 0  --表示対象外は除く
      AND ADR.AddressLevel = 2  --AddressLevel=1 は除く
      AND ADR.CityCD IN (SELECT value FROM string_split(@CitycdCsv, ','))
  
    ORDER BY ADR.PrefCD, ADR.CityCD, ADR.TownKana

END