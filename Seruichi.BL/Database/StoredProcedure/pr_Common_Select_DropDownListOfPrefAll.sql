IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_DropDownListOfPrefAll')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfPrefAll]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfPrefAll]
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     PrefCD         AS [Value]
    ,PrefName       AS DisplayText
    ,DisplayOrder   AS DisplayOrder
    FROM M_Pref
    ORDER BY DisplayOrder

END