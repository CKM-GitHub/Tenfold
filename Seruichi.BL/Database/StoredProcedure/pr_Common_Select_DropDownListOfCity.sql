IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfCity')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfCity]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfCity]
(
    @PrefCD varchar(2)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     CityCD         AS [Value]
    ,CityName       AS DisplayText
    ,DisplayOrder   AS DisplayOrder
    FROM M_Address
    WHERE PrefCD = @PrefCD
    AND   AddressLevel = 1
    AND   NoDisplayFLG = 0
    ORDER BY DisplayOrder

END