IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_train_check_tab_Select_M_RECondLineRent_by_ConditionSEQ')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_train_check_tab_Select_M_RECondLineRent_by_ConditionSEQ]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_train_check_tab_Select_M_RECondLineRent_by_ConditionSEQ]
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
        ,RentLow
        ,RentHigh
    FROM M_RECondLineRent
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ
    ORDER BY RowNO, ColNO

END