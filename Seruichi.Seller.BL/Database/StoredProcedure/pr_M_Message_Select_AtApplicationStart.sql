IF EXISTS (select * from sys.objects where name = 'pr_M_Message_Select_AtApplicationStart')
BEGIN
    DROP PROCEDURE [pr_M_Message_Select_AtApplicationStart]
END
GO

CREATE PROCEDURE [dbo].[pr_M_Message_Select_AtApplicationStart]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
     MessageID
    ,MessageText1
    ,MessageText2
    ,MessageText3
    ,MessageText4
    ,MessageButton
    ,MessageMark
    ,ButtonValues
    FROM M_Message
    --WHERE MessageID IN ()
END