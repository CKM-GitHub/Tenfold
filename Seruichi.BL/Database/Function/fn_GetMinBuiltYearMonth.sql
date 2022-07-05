IF EXISTS (select * from sys.objects where name = 'fn_GetMinBuiltYearMonth')
BEGIN
    DROP FUNCTION [fn_GetMinBuiltYearMonth]
END
GO

CREATE FUNCTION [dbo].[fn_GetMinBuiltYearMonth](
     @BuildingAge    As int
)
RETURNS int
BEGIN

    DECLARE @Today date = GETDATE()
    DECLARE @ResultDate date

    SET @ResultDate = DATEADD(month, 1, DATEADD(year,  @BuildingAge * -1, @Today))

    RETURN CAST(FORMAT(@ResultDate, 'yyyyMM') AS int)

END
