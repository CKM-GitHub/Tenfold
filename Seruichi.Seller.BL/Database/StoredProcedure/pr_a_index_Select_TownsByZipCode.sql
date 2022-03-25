IF EXISTS (select * from sys.objects where name = 'pr_a_index_Select_TownsByZipCode')
BEGIN
    DROP PROCEDURE [pr_a_index_Select_TownsByZipCode]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Select_TownsByZipCode]
(
    @ZipCode1 varchar(3)
    ,@ZipCode2 varchar(4)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DISTINCT TWN.TownCD, TWN.TownName

    FROM M_ZipCode ZIP
    INNER JOIN M_Address TWN ON TWN.TownCD = ZIP.TownCD

    WHERE ZIP.ZipCode1 = @ZipCode1
    AND (@ZipCode2 IS NULL OR ZIP.ZipCode2 = @ZipCode2)
    AND TWN.AddressLevel = 2
    AND TWN.NoDisplayFLG = 0

END