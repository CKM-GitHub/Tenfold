IF EXISTS (select * from sys.objects where name = 'pr_a_login_Select_M_Mail')
BEGIN
    DROP PROCEDURE [pr_a_login_Select_M_Mail]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Select_M_Mail]
AS
BEGIN

    SET NOCOUNT ON

    SELECT
     SenderAddress
    ,SenderServer
    ,SenderAccount
    ,SenderPassword

    FROM M_Mail
    WHERE DataKey = 1

END