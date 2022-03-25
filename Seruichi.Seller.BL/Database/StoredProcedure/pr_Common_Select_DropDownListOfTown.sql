IF EXISTS (select * from sys.objects where name = 'pr_M_Address_Select_DropDownListOfTown')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfTown]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfTown]
(
    @PrefCD varchar(2)
    ,@CityCD varchar(13)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     TownCD         AS [Value]
    ,TownName       AS DisplayText
    ,DisplayOrder   AS DisplayOrder
    FROM M_Address
    WHERE PrefCD = @PrefCD
    AND   CityCD = @CityCD
    AND   AddressLevel = 2
    AND   NoDisplayFLG = 0
    ORDER BY DisplayOrder

END