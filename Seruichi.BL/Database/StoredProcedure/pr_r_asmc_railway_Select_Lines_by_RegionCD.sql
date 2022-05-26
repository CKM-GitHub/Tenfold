IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_railway_Select_Lines_by_RegionCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_railway_Select_Lines_by_RegionCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_railway_Select_Lines_by_RegionCD]
(
    @RegionCD   varchar(2)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    SELECT DISTINCT 
        PRF.PrefCD
        ,PRF.PrefName
        ,LIN.CompanyCD          AS CityGroupCD
        ,RCP.CompanyName        AS CityGroupName
        ,STN.LineCD
        ,LIN.LineName
        ,ISNULL(MAN.Kensu,0)    AS MansionCount
        ,ISNULL(RES.Kensu,0)    AS RealEstateCount
        ,LIN.DisplayOrder
    FROM M_Pref PRF
    INNER JOIN M_Station STN ON PRF.PrefCD = STN.PrefCD
    INNER JOIN M_Line LIN ON STN.LineCD = LIN.LineCD
    INNER JOIN M_RailCompany RCP ON LIN.CompanyCD = RCP.CompanyCD
    ----------------------------------------------------------------------
    -- 登録マンション件数の参照
    -- 　マンション最寄駅マスタ
    --   最寄駅コードで駅マスタを結合し、路線CDを取得
    --   取得した路線コードでグループ化して登録マンション数を取得
    ----------------------------------------------------------------------
    LEFT JOIN (
                    SELECT S2.LineCD 
                        ,Count(DISTINCT S1.MansionCD) AS Kensu
                    FROM M_MansionStation S1 
                    INNER JOIN M_Station S2 ON S1.StationCD = S2.StationCD   
                    GROUP BY S2.LineCD
        ) MAN 
        ON LIN.LineCD = MAN.LineCD
    ----------------------------------------------------------------------
    -- 登録会社数の参照
    --   路線駅査定条件駅マスタ  
    --   路線コードでグループ化して事業者件数を取得
    ----------------------------------------------------------------------
    LEFT JOIN (
                    SELECT LineCD
                        ,Count(DISTINCT RealECD) AS Kensu
                    FROM M_RECondLineSta
                    GROUP By LineCD
        ) RES
        ON LIN.LineCD = RES.LineCD

    ----------------------------------------------------------------------
    -- 自社情報
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT 
                        MAX(t2.ValidFLG)   AS ValidFLG
                        ,MAX(t2.ExpDate)    AS ExpDate
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.LineCD = LIN.LineCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND   t2.ExpDate > GETDATE()
                    GROUP BY LineCD
                ) AS   MyRES

        WHERE STN.NoDisplayFLG = 0  --表示対象外は除く
          AND PRF.RegionCD = @RegionCD
  
        ORDER BY PrefCD, CityGroupCD, CityGroupName, DisplayOrder

END