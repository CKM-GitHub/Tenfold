IF EXISTS (select * from sys.objects where name = 'pr_M_Line_Select_DropDownListOfLine')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfLine]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfLine]
(
    @PrefCD varchar(2)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DISTINCT 
     LIN.LineCD         AS [Value]
    ,LIN.LineName       AS DisplayText
    ,LIN.DisplayOrder   AS DisplayOrder

    FROM M_Station STA
    INNER JOIN M_Line LIN ON LIN.LineCD = STA.LineCD

    WHERE STA.PrefCD = @PrefCD
    AND   STA.NoDisplayFLG = 0
    AND   LIN.NoDisplayFLG = 0

    ORDER BY LIN.DisplayOrder

END