IF EXISTS (select * from sys.objects where name = 'pr_a_register_Select_D_Certification_by_CertificationCD')
BEGIN
    DROP PROCEDURE [pr_a_register_Select_D_Certification_by_CertificationCD]
END
GO

CREATE PROCEDURE [dbo].[pr_a_register_Select_D_Certification_by_CertificationCD]
(
    @CertificationCD            varchar(30)
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT DataSEQ, CreationDateTime, EffectiveDateTime, AccessDateTime, MailAddress, SellerCD
    FROM D_Certification
    WHERE CertificationCD = @CertificationCD

END