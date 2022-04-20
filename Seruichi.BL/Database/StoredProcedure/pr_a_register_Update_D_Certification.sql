IF EXISTS (select * from sys.objects where name = 'pr_register_Update_D_Certification')
BEGIN
    DROP PROCEDURE [pr_register_Update_D_Certification]
END
GO

CREATE PROCEDURE [dbo].[pr_register_Update_D_Certification]
(
    @DataSEQ        decimal
)
AS
BEGIN

    DECLARE @SysDatetime datetime = GETDATE()

    UPDATE D_Certification
    SET AccessDateTime = @SysDatetime
    WHERE DataSEQ = @DataSEQ

END