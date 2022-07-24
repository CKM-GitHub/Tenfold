IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_TownCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_TownCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_TownCD]
(
    @RealECD   varchar(10)
    ,@TownCD varchar(13)
)
AS
BEGIN

    SELECT 
         t1.RealECD
        ,t1.CityCD
        ,t1.TownCD
        ,t1.ConditionSEQ
        ,ADR.CityName + ' ' + ADR.TownName AS TownName

        ,t1.ValidFLG
        ,CASE t1.ValidFLG WHEN 1 THEN '(公開済)' ELSE '(未公開)' END AS ValidFLGText
        ,t1.[期限切れ間近] AS Expired
        ,CASE WHEN t1.[期限切れ] = 1    THEN '有効期限切れ'
              WHEN t1.[期限切れ間近] = 1 THEN '有効期限切れ間近'
              ELSE ISNULL(FORMAT(t1.ExpDate, 'yyyy年MM月dd日迄'),'') END AS ExpDateText

        ,t1.REStaffName
        ,ISNULL(FORMAT(t1.ExpDate, 'yyyy/MM/dd'),'') AS ExpDate
        ,t1.Priority
        ,t1.PrecedenceFlg
        ,t1.Remark

    FROM V_RECondAreaSec t1
    INNER JOIN M_Address ADR ON t1.TownCD = ADR.TownCD
    WHERE t1.RealECD = @RealECD
    AND   t1.TownCD = @TownCD

END