IF EXISTS (select * from sys.objects where name = 'pr_a_register_Select_D_Certification_by_Key')
BEGIN
    DROP PROCEDURE [pr_a_register_Select_D_Certification_by_Key]
END
GO

CREATE PROCEDURE [dbo].[pr_a_register_Select_D_Certification_by_Key]
(
    @CertificationCD            varchar(30)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DataSEQ, CreationDateTime, EffectiveDateTime, AccessDateTime, MailAddress
    FROM D_Certification
    WHERE CertificationCD = @CertificationCD

END