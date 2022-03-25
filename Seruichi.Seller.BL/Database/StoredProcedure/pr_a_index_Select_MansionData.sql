IF EXISTS (select * from sys.objects where name = 'pr_a_index_Select_MansionData')
BEGIN
    DROP PROCEDURE [pr_a_index_Select_MansionData]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Select_MansionData]
(
    @MansionCD varchar(10)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT 
     MAN.MansionCD
    ,MAN.MansionName
    ,MAN.ZipCode1
    ,MAN.ZipCode2
    ,MAN.PrefCD
    ,MAN.CityCD
    ,MAN.TownCD
    ,MAN.[Address]
    ,MAN.StructuralKBN
    ,MAN.Floors
    ,FORMAT(MAN.ConstYYYYMM, '0000-00') AS ConstYYYYMM
    ,CAST(dbo.fn_GetBuildingAge(MAN.ConstYYYYMM) AS varchar) AS BuildingAge
    ,MAN.Rooms
    ,MAN.RightKBN

    ,STA.LineCD
    ,MST.StationCD
    ,MST.Distance

    FROM M_Mansion MAN
    LEFT OUTER JOIN M_MansionStation MST ON MST.MansionCD = MAN.MansionCD
    LEFT OUTER JOIN M_Station STA ON STA.StationCD = MST.StationCD AND STA.NoDisplayFLG = 0
    --LEFT OUTER JOIN M_Line LIN ON LIN.LineCD = STA.LineCD AND STA.NoDisplayFLG = 0
    --LEFT OUTER JOIN M_Pref PRF ON PRF.PrefCD = MAN.PrefCD
    --LEFT OUTER JOIN M_Address ADR ON ADR.TownCD = MAN.TownCD 

    WHERE MAN.MansionCD = @MansionCD
    AND   MAN.NoDisplayFLG = 0

    ORDER BY MST.StationSEQ

END