IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_train_check_tab_Select_M_RECondLineSta_by_StationCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_train_check_tab_Select_M_RECondLineSta_by_StationCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_train_check_tab_Select_M_RECondLineSta_by_StationCD]
(
    @RealECD   varchar(10)
    ,@StationCD varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = GETDATE()

    SELECT TOP 1 
         t1.RealECD
        ,t1.StationCD
        ,t1.ConditionSEQ
        ,LIN.LineName + ' ' + STN.StationName AS StationName
        ,t2.ValidFLG
        ,CASE t2.ValidFLG WHEN 1 THEN '(公開済)' ELSE '(未公開)' END AS ValidFLGText
        ,CASE WHEN t2.ExpDate < @SysDate THEN 1 ELSE 0 END AS ExpDateFLG
        ,CASE WHEN t2.ExpDate < @SysDate THEN '有効期限切れ' ELSE ISNULL(FORMAT(t2.ExpDate, 'yyyy年MM月dd日迄'),'') END AS ExpDateText

    FROM M_RECondLineSta t1
    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ
    INNER JOIN M_Station STN ON t1.StationCD = STN.StationCD
    LEFT OUTER JOIN M_Line LIN ON LIN.LineCD = STN.LineCD
    WHERE t1.RealECD = @RealECD
    AND   t1.StationCD = @StationCD
    AND   t1.DeleteDateTime IS NULL
    AND   t2.DeleteDateTime IS NULL
    ORDER BY t1.DisabledFlg ASC, t1.ConditionSEQ DESC 

END