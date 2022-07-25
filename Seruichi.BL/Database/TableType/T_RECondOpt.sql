IF EXISTS (select * from sys.table_types where user_type_id = Type_id(N'T_RECondOpt'))
BEGIN
    IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Insert_RECondMan_All')
        DROP PROCEDURE [pr_r_asmc_set_ms_Insert_RECondMan_All]

    IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Insert_Template')
        DROP PROCEDURE [pr_r_asmc_set_ms_Insert_Template]
		
    DROP TYPE [T_RECondOpt]
END
GO

CREATE TYPE T_RECondOpt AS TABLE
(
    OptionKBN           int
    ,OptionSEQ          int
    ,CategoryKBN        tinyint
    ,NotApplicableFLG   tinyint
    ,Value1             int
    ,HandlingKBN1       tinyint
    ,Value2             int
    ,HandlingKBN2       tinyint
    ,IncDecRate         decimal(5,2)
)
GO
