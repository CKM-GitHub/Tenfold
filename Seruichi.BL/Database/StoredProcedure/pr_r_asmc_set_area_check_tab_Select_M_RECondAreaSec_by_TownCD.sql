IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_M_RECondAreaSec_by_TownCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_M_RECondAreaSec_by_TownCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_M_RECondAreaSec_by_TownCD]
(
    @RealECD   varchar(10)
    ,@TownCD varchar(13)
)
AS
BEGIN

    DECLARE @SysDate datetime = GETDATE()

    SELECT TOP 1 
         t1.RealECD
        ,t1.TownCD
        ,t1.ConditionSEQ
        ,ADR.CityName + ' ' + ADR.TownName AS TownName
        ,t2.ValidFLG
        ,CASE t2.ValidFLG WHEN 1 THEN '(公開済)' ELSE '(未公開)' END AS ValidFLGText
        ,CASE WHEN t2.ExpDate < @SysDate THEN 1 ELSE 0 END AS ExpDateFLG
        ,CASE WHEN t2.ExpDate < @SysDate THEN '有効期限切れ' ELSE FORMAT(t2.ExpDate, 'yyyy年MM月dd日迄') END AS ExpDateText

    FROM M_RECondAreaSec t1
    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ
    INNER JOIN M_Address ADR ON t1.TownCD = ADR.TownCD
    WHERE t1.RealECD = @RealECD
    AND   t1.TownCD = @TownCD
    AND   t1.DeleteDateTime IS NULL
    AND   t2.DeleteDateTime IS NULL
    ORDER BY t1.DisabledFlg ASC, t1.ConditionSEQ DESC 

END