IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_uinfo_Select_M_Address_by_ZipCode')
BEGIN
    DROP PROCEDURE [pr_a_mypage_uinfo_Select_M_Address_by_ZipCode]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_uinfo_Select_M_Address_by_ZipCode]
(
    @ZipCode1 varchar(3)
    ,@ZipCode2 varchar(4)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT ADR.PrefCD, ADR.CityName, ADR.TownName
      FROM M_ZipCode ZIP
     INNER JOIN M_Address ADR ON ADR.TownCD = ZIP.TownCD
     WHERE ZIP.ZipCode1 = @ZipCode1
       AND ZIP.ZipCode1 = @ZipCode1

END