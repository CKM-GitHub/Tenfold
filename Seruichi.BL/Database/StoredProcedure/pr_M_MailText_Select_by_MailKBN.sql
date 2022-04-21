IF EXISTS (select * from sys.objects where name = 'pr_M_MailText_Select_by_MailKBN')
BEGIN
    DROP PROCEDURE [pr_M_MailText_Select_by_MailKBN]
END
GO

CREATE PROCEDURE [dbo].[pr_M_MailText_Select_by_MailKBN]
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