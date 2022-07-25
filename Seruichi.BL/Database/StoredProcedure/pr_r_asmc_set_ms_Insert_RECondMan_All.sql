IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Insert_RECondMan_All')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Insert_RECondMan_All]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Insert_RECondMan_All]
(
    @RealECD            varchar(10)
    ,@MansionCD         varchar(10)

    --M_RECondMan
    ,@PrecedenceFlg	    tinyint
    ,@NotApplicableFlg	tinyint
    ,@ValidFLG	        tinyint
    ,@ExpDate	        date
    ,@Priority	        tinyint
    ,@Remark	        varchar(500)
    ,@REStaffCD	        varchar(10)

    --M_RECondManRate
    ,@Rate              decimal

    --M_RECondManRent
    ,@RentLow           money
    ,@RentHigh          money

    --M_RECondManOpt
    ,@RECondOptTable    T_RECondOpt READONLY

    ,@Operator          varchar(10)
    ,@IPAddress         varchar(20)
    ,@REStaffName       varchar(50)
)
AS
BEGIN

    DECLARE @SysDatetime datetime = getdate()
    DECLARE @ConditionSEQ int
    DECLARE @NewConditionSEQ int
    DECLARE @LogRemarks varchar(100)
    DECLARE @MaxDate date = '2029/12/31'

    --------------------
    --元のデータを無効に
    --------------------
    UPDATE M_RECondMan SET
        @ConditionSEQ = ConditionSEQ
        ,DisabledFlg = 1
        ,UpdateOperator = @Operator
        ,UpdateDateTime = @SysDatetime
        ,UpdateIPAddress = @IPAddress
    WHERE RealECD = @RealECD
    AND   MansionCD = @MansionCD
    AND   DisabledFlg = 0

    UPDATE M_RECondManRate SET
         UpdateOperator = @Operator
        ,UpdateDateTime = @SysDatetime
        ,UpdateIPAddress = @IPAddress
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

    UPDATE M_RECondManRent SET
         UpdateOperator = @Operator
        ,UpdateDateTime = @SysDatetime
        ,UpdateIPAddress = @IPAddress
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

    UPDATE M_RECondManOpt SET
         UpdateOperator = @Operator
        ,UpdateDateTime = @SysDatetime
        ,UpdateIPAddress = @IPAddress
    WHERE RealECD = @RealECD
    AND   ConditionSEQ = @ConditionSEQ

    --------------------
    --査定条件SEQを採番
    --------------------
    UPDATE M_RECounter SET 
        @NewConditionSEQ = SEQCounter = ISNULL(SEQCounter,0) + 1
    WHERE RealECD = @RealECD
    AND   SEQKbn = 1 --1:査定条件SEQ

    --------------------
    --新規レコード追加
    --------------------
    --M_RECondMan
    INSERT INTO M_RECondMan (
        RealECD
        ,ConditionSEQ
        ,MansionCD
        ,DisabledFlg
        ,PrecedenceFlg
        ,NotApplicableFlg
        ,ValidFLG
        ,ValidDateTime
        ,ConfDateTime
        ,ExpDate
        ,Priority
        ,Remark
        ,REStaffCD
        ,ExpStartDate
        ,ExpEndDate
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
        ,@NewConditionSEQ
        ,@MansionCD
        ,0 --DisabledFlg
        ,@PrecedenceFlg
        ,@NotApplicableFlg
        ,@ValidFLG
        ,CASE @ValidFLG WHEN 1 THEN @SysDatetime ELSE NULL END --ValidDateTime
        ,NULL --ConfDateTime
        ,@ExpDate
        ,@Priority
        ,@Remark
        ,@REStaffCD
        ,NULL --ExpStartDate
        ,NULL --ExpEndDate
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
    )

    IF @NotApplicableFlg = 0
    BEGIN

        --M_RECondManRate
        INSERT INTO M_RECondManRate (
            RealECD
            ,ConditionSEQ
            ,MansionCD
            ,Rate
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
            ,@NewConditionSEQ
            ,@MansionCD
            ,@Rate
            ,@Operator
            ,@SysDatetime
            ,@IPAddress
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
        )

        --M_RECondManRent
        INSERT INTO M_RECondManRent (
            RealECD
            ,ConditionSEQ
            ,MansionCD
            ,RentLow
            ,RentHigh
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
            ,@NewConditionSEQ
            ,@MansionCD
            ,@RentLow
            ,@RentHigh
            ,@Operator
            ,@SysDatetime
            ,@IPAddress
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
        )

        --M_RECondManOpt
        INSERT INTO M_RECondManOpt (
            RealECD
            ,ConditionSEQ
            ,MansionCD
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
            ,@NewConditionSEQ
            ,@MansionCD
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
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
        FROM @RECondOptTable t
    END

    --------------------
    --更新ログ
    --------------------
    SET @LogRemarks = @MansionCD + ' 査定条件SEQ:' + CAST(@NewConditionSEQ AS varchar)
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
