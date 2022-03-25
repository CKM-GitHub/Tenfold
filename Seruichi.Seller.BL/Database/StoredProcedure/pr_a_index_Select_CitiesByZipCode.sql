IF EXISTS (select * from sys.objects where name = 'pr_a_index_Select_CitiesByZipCode')
BEGIN
    DROP PROCEDURE [pr_a_index_Select_CitiesByZipCode]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Select_CitiesByZipCode]
(
    @ZipCode1 varchar(3)
    ,@ZipCode2 varchar(4)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DISTINCT CTY.CityCD, CTY.CityName

    FROM M_ZipCode ZIP
    INNER JOIN M_Address TWN ON TWN.TownCD = ZIP.TownCD AND TWN.AddressLevel = 2
    INNER JOIN M_Address CTY ON CTY.CityCD = TWN.CityCD AND CTY.AddressLevel = 1

    WHERE ZIP.ZipCode1 = @ZipCode1
    AND (@ZipCode2 IS NULL OR ZIP.ZipCode2 = @ZipCode2)
    AND CTY.NoDisplayFLG = 0

END