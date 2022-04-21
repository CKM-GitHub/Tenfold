IF EXISTS (select * from sys.objects where name = 'pr_a_login_Insert_D_Certification')
BEGIN
    DROP PROCEDURE [pr_a_login_Insert_D_Certification]
END
GO

CREATE PROCEDURE [dbo].[pr_a_login_Insert_D_Certification]
(
    @CertificationCD        varchar(30)
    ,@MailAddress           varchar(300)
    ,@EffectiveDateTime     datetime OUTPUT
)
AS
BEGIN

    DECLARE @SysDatetime datetime = GETDATE()

    DECLARE @Num1 int 
    SELECT @Num1 = Num1 FROM M_MultPurpose WHERE DataID = 118 AND DataKey = 1

    SET @EffectiveDateTime = DATEADD(minute, ISNULL(@Num1,0), @SysDatetime)

    INSERT INTO D_Certification
    (
        CertificationCD
        ,CreationDateTime
        ,EffectiveDateTime
        ,AccessDateTime
        ,MailAddress
    ) VALUES (
        @CertificationCD
        ,@SysDatetime
        ,@EffectiveDateTime
        ,NULL
        ,@MailAddress
    )

END