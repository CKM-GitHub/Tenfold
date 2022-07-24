IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Select_M_RECondManOpt_by_ConditionSEQ')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Select_M_RECondManOpt_by_ConditionSEQ]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Select_M_RECondManOpt_by_ConditionSEQ]
(
    @RealECD   varchar(10)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT
         OptionKBN
        ,OptionSEQ
        ,CategoryKBN
        ,Value1
        ,HandlingKBN1
        ,NotApplicableFLG
        ,FORMAT(IncDecRate, '#0.00') AS IncDecRate

    FROM M_RECondManOpt

    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

    ORDER BY OptionKBN, OptionSEQ

END