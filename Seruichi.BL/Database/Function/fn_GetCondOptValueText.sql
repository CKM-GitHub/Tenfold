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
    --    SET @result = '���ː�' + CAST(@Value1 AS varchar) + '�ˈȉ�'

    IF @OptionKBN = 2
        SET @result = '1�K' 

    ELSE IF @OptionKBN = 3
        SET @result = '�ŏ�K'

    --ELSE IF @OptionKBN = 4
    --    SET @result = CAST(@Value1 AS varchar) + '�u' + CASE @HandlingKBN1 WHEN 1 THEN '�ȓ�' WHEN 4 THEN '�`' ELSE '' END

    ELSE IF @OptionKBN = 5
        SET @result = '�o���R�j�[�Ȃ�'

    ELSE IF @OptionKBN = 6
        SET @result = '�k����'

    ELSE IF @OptionKBN = 7
        SET @result = '�p����'

    --ELSE IF @OptionKBN = 8
    --    SET @result = CAST(@Value1 AS varchar) + '����' 

    ELSE IF @OptionKBN = 9
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '�Z�p���[�g'
                        WHEN 2 THEN '���j�b�g'
                        WHEN 3 THEN '�V�����[�u�[�X'
                        ELSE '' END

    ELSE IF @OptionKBN = 10
        SET @result = '�ؒn��'

    ELSE IF @OptionKBN = 11
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '��'
                        WHEN 2 THEN '����'
                        WHEN 3 THEN '�T�u���[�X'
                        ELSE '' END

    ELSE IF @OptionKBN = 12
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '����Ǘ�'
                        WHEN 2 THEN '�Ǘ��ϑ�'
                        ELSE '' END

    ELSE IF @OptionKBN = 13
        SET @result = CASE @CategoryKBN 
                        WHEN 1 THEN '2�T�Ԉȏ�'
                        WHEN 2 THEN '1�����ȏ�'
                        WHEN 3 THEN '3�����ȏ�'
                        WHEN 4 THEN '6�����ȏ�'
                        WHEN 5 THEN '1�N�ȏ�'
                        WHEN 6 THEN '���̑�'
                        ELSE '' END

    ELSE
        SET @result = CAST(@Value1 AS varchar)

    RETURN @result

END
