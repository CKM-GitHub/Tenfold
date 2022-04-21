IF EXISTS (select * from sys.objects where name = 'pr_M_MailSendTo_Select_by_MailKBN')
BEGIN
    DROP PROCEDURE [pr_M_MailSendTo_Select_by_MailKBN]
END
GO

CREATE PROCEDURE [dbo].[pr_M_MailSendTo_Select_by_MailKBN]
(
    @MailKBN    int
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT SendType, SendToAddress
    FROM M_MailSendTo
    WHERE MailKBN = @MailKBN

END