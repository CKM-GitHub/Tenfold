IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_M_RECondAreaOpt_by_ConditionSEQ')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_M_RECondAreaOpt_by_ConditionSEQ]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_M_RECondAreaOpt_by_ConditionSEQ]
(
    @RealECD   varchar(10)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT
         OptionKBN
        ,OptionSEQ
        ,dbo.fn_GetCondOptOptionKBNName(OptionKBN) AS OptionKBNName
        ,dbo.fn_GetCondOptValueText(OptionKBN, CategoryKBN, Value1, HandlingKBN1) AS ValueText
        ,CASE HandlingKBN1 WHEN 1 THEN 'à»ì‡' WHEN 4 THEN 'Å`' ELSE '' END AS HandlingKBNText
        ,NotApplicableFLG
        ,CASE NotApplicableFLG WHEN 1 THEN 'ç∏íËÅEîÉéÊëŒè€äO' ELSE '' END AS NotApplicableFLGText
        ,FORMAT(IncDecRate, '#0.00') AS IncDecRate

    FROM M_RECondAreaOpt

    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

    ORDER BY OptionKBN, OptionSEQ

END