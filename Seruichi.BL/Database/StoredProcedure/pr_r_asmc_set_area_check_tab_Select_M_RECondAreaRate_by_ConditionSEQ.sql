IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_M_RECondAreaRate_by_ConditionSEQ')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_M_RECondAreaRate_by_ConditionSEQ]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_M_RECondAreaRate_by_ConditionSEQ]
(
    @RealECD   varchar(10)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT
        ColNo
        ,RowNo
        ,DistanceFrom
        ,DistanceTo
        ,AgeFrom
        ,AgeTo
        ,Rate
    FROM M_RECondAreaRate
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ
    ORDER BY RowNO, ColNO

END