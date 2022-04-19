IF EXISTS (select * from sys.objects where name = 'pr_a_register_Select_M_Address_ByZipCode')
BEGIN
    DROP PROCEDURE [pr_a_register_Select_M_Address_ByZipCode]
END
GO

CREATE PROCEDURE [dbo].[pr_a_register_Select_M_Address_ByZipCode]
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