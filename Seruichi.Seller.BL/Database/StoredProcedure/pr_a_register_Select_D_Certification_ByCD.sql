IF EXISTS (select * from sys.objects where name = 'pr_a_register_Select_D_Certification_ByCD')
BEGIN
    DROP PROCEDURE [pr_a_register_Select_D_Certification_ByCD]
END
GO

CREATE PROCEDURE [dbo].[pr_a_register_Select_D_Certification_ByCD]
(
    @CertificationCD            varchar(12)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DataSEQ, CreationDateTime, EffectiveDateTime, AccessDateTime, MailAddress
    FROM D_Certification
    WHERE CertificationCD = @CertificationCD

END