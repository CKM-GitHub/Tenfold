IF EXISTS (select * from sys.objects where name = 'pr_M_Counter_GetNewID')
BEGIN
    DROP PROCEDURE [pr_M_Counter_GetNewID]
END
GO

CREATE PROCEDURE [dbo].[pr_M_Counter_GetNewID]
(
    @CounterKey     int,
    @NewID          varchar(10) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Prefix varchar(3) = ''
    DECLARE @Counter int = 0

    UPDATE M_Counter SET 
        @Prefix = ISNULL(Prefix, ''),
        @Counter = [Counter] = ISNULL([Counter],0) + 1
    WHERE CounterKey = @CounterKey

    SET @NewID = @Prefix + RIGHT(FORMAT(@Counter, '0000000000'), 10 - LEN(@Prefix))

END
