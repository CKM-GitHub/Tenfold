IF EXISTS (select * from sys.objects where name = 'fn_GetMaxBuiltYearMonth')
BEGIN
    DROP FUNCTION [fn_GetMaxBuiltYearMonth]
END
GO

CREATE FUNCTION [dbo].[fn_GetMaxBuiltYearMonth](
     @BuildingAge    As int
)
RETURNS int
BEGIN

    DECLARE @Today datetime = GETDATE()
    DECLARE @ResultDate datetime

    SET @ResultDate = DATEADD(year,  (@BuildingAge - 1) * -1, @Today)

    RETURN CAST(FORMAT(@ResultDate, 'yyyyMM') AS int)

END
