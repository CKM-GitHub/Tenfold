IF EXISTS (select * from sys.objects where name = 'pr_a_index_Select_Prefectures_by_ZipCode')
BEGIN
    DROP PROCEDURE [pr_a_index_Select_Prefectures_by_ZipCode]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Select_Prefectures_by_ZipCode]
(
    @ZipCode1 varchar(3)
    ,@ZipCode2 varchar(4)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT PRF.PrefCD, PRF.PrefName, PRF.RegionCD

    FROM M_ZipCode ZIP
    INNER JOIN M_Address ADR ON ADR.TownCD = ZIP.TownCD
    INNER JOIN M_Pref PRF ON PRF.PrefCD = ADR.PrefCD

    WHERE ZipCode1 = @ZipCode1
    AND (@ZipCode2 IS NULL OR ZipCode2 = @ZipCode2)

END