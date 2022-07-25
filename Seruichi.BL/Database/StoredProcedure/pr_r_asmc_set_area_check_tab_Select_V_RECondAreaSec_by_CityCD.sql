IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_CityCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_CityCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec_by_CityCD]
(
    @RealECD   varchar(10)
    ,@CityCD varchar(13)
)
AS
BEGIN

    SELECT 
         t1.RealECD
        ,t1.CityCD
        ,NULL AS TownCD
        ,t1.ConditionSEQ
        ,ADR.CityName AS TownName

        ,t1.ValidFLG
        ,CASE t1.ValidFLG WHEN 1 THEN '(���J��)' ELSE '(�����J)' END AS ValidFLGText
        ,t1.[�����؂�ԋ�] AS Expired
        ,CASE WHEN t1.[�����؂�] = 1    THEN '�L�������؂�'
              WHEN t1.[�����؂�ԋ�] = 1 THEN '�L�������؂�ԋ�'
              ELSE ISNULL(FORMAT(t1.ExpDate, 'yyyy�NMM��dd����'),'') END AS ExpDateText

        ,t1.REStaffName
        ,ISNULL(FORMAT(t1.ExpDate, 'yyyy/MM/dd'),'') AS ExpDate
        ,t1.Priority
        ,t1.PrecedenceFlg
        ,t1.Remark

    FROM V_RECondAreaSec t1
    CROSS APPLY (SELECT TOP 1 * FROM M_Address ADR WHERE ADR.CityCD = t1.CityCD) AS ADR 
    WHERE t1.RealECD = @RealECD
    AND   t1.CityCD = @CityCD
    AND   t1.TownCD = '' --View�ł�NULL��''

END