IF EXISTS (select * from sys.objects where name = 'pr_M_Counter_Select_Exists')
BEGIN
    DROP PROCEDURE [pr_M_Counter_Select_Exists]
END
GO

CREATE PROCEDURE [dbo].[pr_M_Counter_Select_Exists]
(
    @CounterKey int
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT TOP 1 CounterKey
    FROM M_Counter
    WHERE CounterKey = @CounterKey

END