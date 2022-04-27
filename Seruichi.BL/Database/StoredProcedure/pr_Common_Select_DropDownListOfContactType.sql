IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfContactType')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfContactType]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfContactType]
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     DataKey        AS [Value]
    ,Char1          AS DisplayText
    ,Num1           AS DisplayOrder
    ,Char2          AS HiddenItem
    FROM M_MultPurpose
    WHERE DataID = 103
    ORDER BY Num1

END