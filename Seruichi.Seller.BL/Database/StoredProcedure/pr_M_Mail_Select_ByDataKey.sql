IF EXISTS (select * from sys.objects where name = 'pr_M_Mail_Select_ByDataKey')
BEGIN
    DROP PROCEDURE [pr_M_Mail_Select_ByDataKey]
END
GO

CREATE PROCEDURE [dbo].[pr_M_Mail_Select_ByDataKey]
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