IF EXISTS (select * from sys.objects where name = 'fn_GetCondOptOptionKBNName')
BEGIN
    DROP FUNCTION [fn_GetCondOptOptionKBNName]
END
GO

CREATE FUNCTION [dbo].[fn_GetCondOptOptionKBNName](
     @OptionKBN    As int
)
RETURNS varchar(20)
BEGIN

    DECLARE @result varchar(20)

    SET @result = CASE @OptionKBN 
        WHEN 1 THEN '総戸数'　　　　
        WHEN 2 THEN '所在階'
        WHEN 3 THEN '所在階'
        WHEN 4 THEN '専有面積'
        WHEN 5 THEN 'バルコニー'
        WHEN 6 THEN '主採光'
        WHEN 7 THEN '角部屋'
        WHEN 8 THEN '部屋数'
        WHEN 9 THEN 'バストイレ'
        WHEN 10 THEN '土地権利'
        WHEN 11 THEN '賃貸状況'
        WHEN 12 THEN '管理状況'
        WHEN 13 THEN '売却希望時期'
        ELSE '' END

    RETURN @result

END
