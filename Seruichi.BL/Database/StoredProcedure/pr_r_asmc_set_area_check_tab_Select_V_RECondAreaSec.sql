IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec]
(
    @RealECD   varchar(10)
    ,@TownCD varchar(13)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT TOP 1
         REStaffName
        ,ISNULL(FORMAT(ExpDate, 'yyyy/MM/dd'),'') AS ExpDate
        ,Priority
        ,PrecedenceFlg
        ,Remark

    FROM V_RECondAreaSec

    WHERE RealECD = @RealECD
    AND   TownCD = @TownCD
    AND   ConditionSEQ = @ConditionSEQ

END
