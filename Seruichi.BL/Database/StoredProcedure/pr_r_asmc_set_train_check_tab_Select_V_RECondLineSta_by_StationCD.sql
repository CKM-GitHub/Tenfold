IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta_by_StationCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta_by_StationCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_train_check_tab_Select_V_RECondLineSta_by_StationCD]
(
    @RealECD   varchar(10)
    ,@StationCD varchar(10)
)
AS
BEGIN

    SELECT
         t1.RealECD
        ,t1.StationCD
        ,t1.ConditionSEQ
        ,LIN.LineName + ' ' + STN.StationName AS StationName

        ,t1.ValidFLG
        ,CASE t1.ValidFLG WHEN 1 THEN '(公開済)' ELSE '(未公開)' END AS ValidFLGText
        ,t1.[期限切れ間近] AS Expired
        ,CASE WHEN t1.[期限切れ] = 1    THEN '有効期限切れ'
              WHEN t1.[期限切れ間近] = 1 THEN '有効期限切れ間近'
              ELSE ISNULL(FORMAT(t1.ExpDate, 'yyyy年MM月dd日迄'),'') END AS ExpDateText

        ,t1.REStaffName
        ,ISNULL(FORMAT(t1.ExpDate, 'yyyy/MM/dd'),'') AS ExpDate
        ,t1.Priority
        ,t1.PrecedenceFlg
        ,t1.Remark

    FROM V_RECondLineSta t1
    INNER JOIN M_Station STN ON t1.StationCD = STN.StationCD
    LEFT OUTER JOIN M_Line LIN ON LIN.LineCD = STN.LineCD
    WHERE t1.RealECD = @RealECD
    AND   t1.StationCD = @StationCD

END