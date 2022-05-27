IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_LineChange')
BEGIN
    DROP PROCEDURE [pr_Common_Select_LineChange]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_LineChange]
(
    @MansionStationNameTable    T_MansionStationName READONLY
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DISTINCT
     W.RowNo
    ,LIN.LineCD
    ,STN.StationCD
    ,CAST(CEILING(ISNULL(W.Distance,0) / 80.0) AS varchar) AS Distance
    FROM @MansionStationNameTable W
	INNER JOIN M_LineChange LIN ON LIN.APILineName = W.LineName
    INNER JOIN M_Station STN ON STN.LineCD = LIN.LineCD AND STN.StationName = W.StationName
    ORDER BY W.RowNo

END
