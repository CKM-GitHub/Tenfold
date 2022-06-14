IF EXISTS (select * from sys.objects where name = 'fn_GetCondOptValueText')
BEGIN
    DROP FUNCTION [fn_GetCondOptValueText]
END
GO

CREATE FUNCTION [dbo].[fn_GetCondOptValueText](
     @OptionKBN         AS int
     ,@CategoryKBN      AS tinyint
     ,@Value1           AS tinyint
     ,@HandlingKBN1     AS tinyint
)
RETURNS varchar(20)
BEGIN

    DECLARE @result varchar(20)

    --IF @OptionKBN = 1
    --    SET @result = '総戸数' + CAST(@Value1 AS varchar) + '戸以下'

    IF @OptionKBN = 2
        SET @result = '1階' 

    ELSE IF @OptionKBN = 3
        SET @result = '最上階'

    --ELSE IF @OptionKBN = 4
    --    SET @result = CAST(@Value1 AS varchar) + '㎡' + CASE @HandlingKBN1 WHEN 1 THEN '以内' WHEN 4 THEN '～' ELSE '' END

    ELSE IF @OptionKBN = 5
        SET @result = 'バルコニーなし'

    ELSE IF @OptionKBN = 6
        SET @result = '北向き'

    ELSE IF @OptionKBN = 7
        SET @result = '角部屋'

    --ELSE IF @OptionKBN = 8
    --    SET @result = CAST(@Value1 AS varchar) + '部屋' 

    ELSE IF @OptionKBN = 9
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN 'セパレート'
                        WHEN 2 THEN 'ユニット'
                        WHEN 3 THEN 'シャワーブース'
                        ELSE '' END

    ELSE IF @OptionKBN = 10
        SET @result = '借地権'

    ELSE IF @OptionKBN = 11
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '空室'
                        WHEN 2 THEN '賃貸'
                        WHEN 3 THEN 'サブリース'
                        ELSE '' END

    ELSE IF @OptionKBN = 12
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '自主管理'
                        WHEN 2 THEN '管理委託'
                        ELSE '' END

    ELSE IF @OptionKBN = 13
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '2週間以上'
                        WHEN 2 THEN '1ヶ月以上'
                        WHEN 3 THEN '3ヶ月以上'
                        WHEN 4 THEN '6ヶ月以上'
                        WHEN 5 THEN '1年以上'
                        WHEN 6 THEN 'その他'
                        ELSE '' END

    ELSE
        SET @result = CAST(@Value1 AS varchar)

    RETURN @result

END
