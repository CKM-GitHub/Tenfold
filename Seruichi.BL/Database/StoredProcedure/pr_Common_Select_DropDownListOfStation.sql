IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfStation')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfStation]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfStation]
(
    @LineCD varchar(10)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     StationCD      AS [Value]
    ,StationName    AS DisplayText
    ,DisplayOrder   As DisplayOrder
    FROM M_Station
    WHERE LineCD = @LineCD
    AND   NoDisplayFLG = 0
    ORDER BY DisplayOrder

END