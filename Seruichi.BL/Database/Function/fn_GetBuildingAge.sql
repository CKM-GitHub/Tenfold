IF EXISTS (select * from sys.objects where name = 'fn_GetBuildingAge')
BEGIN
    DROP FUNCTION [fn_GetBuildingAge]
END
GO

CREATE FUNCTION [dbo].[fn_GetBuildingAge](
     @StartYearMonth    As int
)
RETURNS int
BEGIN
    IF @StartYearMonth IS NULL RETURN 0

    DECLARE @ReturnValue int, @diffMonth int
    DECLARE @StartDate varchar(10) = FORMAT(@StartYearMonth, '0000/00') + '/01'
    DECLARE @EndDate datetime = GETDATE()

    IF @StartDate > @EndDate RETURN 0;

    SET @diffMonth = CASE WHEN ISDATE(@StartDate) = 1 AND ISDATE(@EndDate) = 1
        THEN DATEDIFF(m, @StartDate, @EndDate) ELSE 0 END

    --SET @ReturnValue = CASE WHEN @diffMonth % 12 > 0 THEN @diffMonth / 12 + 1 ELSE @diffMonth / 12 END
    SET @ReturnValue = @diffMonth / 12 + 1
    RETURN @ReturnValue
END
