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
        WHEN 1 THEN '���ː�'�@�@�@�@
        WHEN 2 THEN '���݊K'
        WHEN 3 THEN '���݊K'
        WHEN 4 THEN '��L�ʐ�'
        WHEN 5 THEN '�o���R�j�['
        WHEN 6 THEN '��̌�'
        WHEN 7 THEN '�p����'
        WHEN 8 THEN '������'
        WHEN 9 THEN '�o�X�g�C��'
        WHEN 10 THEN '�y�n����'
        WHEN 11 THEN '���ݏ�'
        WHEN 12 THEN '�Ǘ���'
        WHEN 13 THEN '���p��]����'
        ELSE '' END

    RETURN @result

END
