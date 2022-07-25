IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Insert_Template')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Insert_Template]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Insert_Template]
(
    @RealECD            varchar(10)
    ,@TemplateName      varchar(50)
    ,@TemplateOptTable  T_RECondOpt READONLY

    ,@Operator          varchar(10)
    ,@IPAddress         varchar(20)
    ,@REStaffName       varchar(50)
)
AS
BEGIN

    DECLARE @SysDatetime datetime = getdate()
    DECLARE @ConditionSEQ int
    DECLARE @NewTemplateNo int
    DECLARE @LogRemarks varchar(100)
    DECLARE @MaxDate date = '2029/12/31'

    --------------------
    --テンプレートカウンタを採番
    --------------------
    UPDATE M_TemplateCount SET 
        @NewTemplateNo = TemplateNo = ISNULL(TemplateNo,0) + 1,
        UpdateOperator = @Operator,
        UpdateDateTime = @SysDatetime
    WHERE RealECD = @RealECD

    --------------------
    --新規レコード追加
    --------------------
    INSERT INTO M_Template(
        RealECD
        ,TemplateNo
        ,TemplateName
        ,TemplateKBN
        ,CreatePage
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    ) VALUES (
        @RealECD
        ,@NewTemplateNo
        ,@TemplateName
        ,3 --TemplateKBN
        ,3 --CreatePage
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,NULL
        ,NULL
        ,NULL
    )

    INSERT INTO M_TemplateOpt (
        RealECD
        ,TemplateNo
        ,OptionKBN
        ,OptionSEQ
        ,CategoryKBN
        ,NotApplicableFLG
        ,Value1
        ,HandlingKBN1
        ,Value2
        ,HandlingKBN2
        ,IncDecRate
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    ) SELECT
        @RealECD
        ,@NewTemplateNo
        ,t.OptionKBN
        ,ROW_NUMBER() OVER(PARTITION BY t.OptionKBN ORDER BY t.OptionSEQ)
        ,t.CategoryKBN
        ,t.NotApplicableFLG
        ,t.Value1
        ,t.HandlingKBN1
        ,t.Value2
        ,t.HandlingKBN2
        ,t.IncDecRate
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,NULL
        ,NULL
        ,NULL
    FROM @TemplateOptTable t

    --------------------
    --更新ログ
    --------------------
    SET @LogRemarks = 'テンプレート:' + CAST(@NewTemplateNo AS varchar)
    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 2
    ,@LoginID       = @Operator
    ,@RealECD       = @RealECD      
    ,@LoginName     = @REStaffName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'r_asmc_set_ms'
    ,@ProcessKBN    = 1
    ,@Remarks       = @LogRemarks

END
