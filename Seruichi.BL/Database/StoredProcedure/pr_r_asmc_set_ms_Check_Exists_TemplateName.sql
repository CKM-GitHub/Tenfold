IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Check_Exists_TemplateName')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Check_Exists_TemplateName]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Check_Exists_TemplateName]
(
    @RealECD            varchar(10)
    ,@TemplateName      varchar(50)
)
AS
BEGIN

    SELECT TOP 1 1 AS ExistsFLG
    FROM M_Template 
    WHERE RealECD = @RealECD 
    AND   TemplateName = @TemplateName

END
