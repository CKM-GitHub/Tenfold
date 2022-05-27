IF EXISTS (select * from sys.objects where name = 'pr_a_index_Select_MansionList_by_MansionWord')
BEGIN
    DROP PROCEDURE [pr_a_index_Select_MansionList_by_MansionWord]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Select_MansionList_by_MansionWord]
(
    @PrefCD varchar(2)
    ,@MansionWord varchar(100)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DISTINCT TOP 2000
    MAN.MansionCD AS [Value], MAN.MansionName AS DisplayText, MAN.DisplayOrder

    FROM M_MansionWord WRD
    INNER JOIN M_Mansion MAN ON MAN.MansionCD = WRD.MansionCD

    WHERE WRD.MansionWord LIKE CONCAT('%', @MansionWord, '%')
    --AND   (@PrefCD IS NULL OR MAN.PrefCD = @PrefCD)
    AND   MAN.NoDisplayFLG = 0

    ORDER BY MAN.DisplayOrder, MAN.MansionName

END