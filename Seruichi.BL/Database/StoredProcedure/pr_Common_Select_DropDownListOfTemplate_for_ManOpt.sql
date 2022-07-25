IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfTemplate_for_ManOpt')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfTemplate_for_ManOpt]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfTemplate_for_ManOpt]
(
    @RealECD            varchar(10)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     TemplateNo     AS [Value]
    ,TemplateName   AS DisplayText
    ,ROW_NUMBER() OVER(ORDER BY TemplateNo) AS DisplayOrder
    FROM M_Template
    WHERE RealECD = @RealECD
    AND   TemplateKBN = 3 --オプション用
    AND   CreatePage = 3 --マンション用
    AND   DeleteDateTime IS NULL
    ORDER BY DisplayOrder

END