IF EXISTS (select * from sys.objects where name = 'pr_GetDate')
BEGIN
    DROP PROCEDURE [pr_GetDate]
END
GO

CREATE PROCEDURE [dbo].[pr_GetDate]
AS
BEGIN

    SET NOCOUNT ON

    SELECT GETDATE() AS SystemDateTime

END