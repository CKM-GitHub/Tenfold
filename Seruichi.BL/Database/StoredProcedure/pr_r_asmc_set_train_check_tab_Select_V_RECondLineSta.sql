IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta]
(
    @RealECD   varchar(10)
    ,@StationCD varchar(10)
    ,@ConditionSEQ int
)
AS
BEGIN

    SELECT TOP 1
         REStaffName
        ,FORMAT(ExpDate, 'yyyy/MM/dd') AS ExpDate
        ,Priority
        ,Remark

    FROM V_RECondLineSta

    WHERE RealECD = @RealECD
    AND   StationCD = @StationCD
    AND   ConditionSEQ = @ConditionSEQ

END
