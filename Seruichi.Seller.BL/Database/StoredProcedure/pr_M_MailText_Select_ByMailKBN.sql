IF EXISTS (select * from sys.objects where name = 'pr_M_MailText_Select_ByMailKBN')
BEGIN
    DROP PROCEDURE [pr_M_MailText_Select_ByMailKBN]
END
GO

CREATE PROCEDURE [dbo].[pr_M_MailText_Select_ByMailKBN]
(
    @MailKBN    int
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT MailTitle, MailText01, MailText02, MailText03, MailText04, MailText05
    FROM M_MailText
    WHERE MailKBN = @MailKBN

END