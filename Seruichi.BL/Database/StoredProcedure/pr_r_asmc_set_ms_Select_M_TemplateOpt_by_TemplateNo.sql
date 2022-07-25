IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Select_M_TemplateOpt_by_TemplateNo')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Select_M_TemplateOpt_by_TemplateNo]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Select_M_TemplateOpt_by_TemplateNo]
(
    @RealECD   varchar(10)
    ,@TemplateNo int
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

    FROM M_TemplateOpt

    WHERE RealECD = @RealECD
    AND   TemplateNo = @TemplateNo

    ORDER BY OptionKBN, OptionSEQ

END