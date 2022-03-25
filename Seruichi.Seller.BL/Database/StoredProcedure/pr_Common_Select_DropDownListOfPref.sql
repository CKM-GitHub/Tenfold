IF EXISTS (select * from sys.objects where name = 'pr_M_Pref_Select_DropDownListOfPref')
BEGIN
    DROP PROCEDURE [pr_Common_Select_DropDownListOfPref]
END
GO

CREATE PROCEDURE [dbo].[pr_Common_Select_DropDownListOfPref]
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     PrefCD         AS [Value]
    ,PrefName       AS DisplayText
    ,DisplayOrder   AS DisplayOrder
    FROM M_Pref
    WHERE RegionCD IS NOT NULL
    ORDER BY DisplayOrder

END