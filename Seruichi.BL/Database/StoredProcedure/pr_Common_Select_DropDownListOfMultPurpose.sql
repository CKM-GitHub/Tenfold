IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfMultPurpose')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfMultPurpose]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfMultPurpose]
    @DataID int
AS
BEGIN

    SET NOCOUNT ON

    IF @DataID = 104
    BEGIN
        SELECT
         DataKey        AS [Value]
        ,Char1          AS DisplayText
        ,Num2           AS DisplayOrder
        ,Num1           AS HiddenItem
        FROM M_MultPurpose
        WHERE DataID = @DataID
        ORDER BY Num2, DataKey
    END
    ELSE
    BEGIN
        SELECT
         DataKey        AS [Value]
        ,Char1          AS DisplayText
        ,Num1           AS DisplayOrder
        FROM M_MultPurpose
        WHERE DataID = @DataID
        ORDER BY Num1, DataKey
    END
END
