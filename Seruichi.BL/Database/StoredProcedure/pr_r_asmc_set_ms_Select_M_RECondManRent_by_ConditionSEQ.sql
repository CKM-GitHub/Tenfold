IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Select_M_RECondManRent_by_ConditionSEQ')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Select_M_RECondManRent_by_ConditionSEQ]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Select_M_RECondManRent_by_ConditionSEQ]
(
    @RealECD   varchar(10)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT
         RentLow
        ,RentHigh
    FROM M_RECondManRent
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

END