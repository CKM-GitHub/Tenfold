IF EXISTS (select * from sys.objects where name = 'pr_M_Mail_Select_by_DataKey')
BEGIN
    DROP PROCEDURE [pr_M_Mail_Select_by_DataKey]
END
GO

CREATE PROCEDURE [dbo].[pr_M_Mail_Select_by_DataKey]
(
    @DataKey    int
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     SenderAddress
    ,SenderServer
    ,SenderAccount
    ,SenderPassword
    FROM M_Mail
    WHERE DataKey = @DataKey

END