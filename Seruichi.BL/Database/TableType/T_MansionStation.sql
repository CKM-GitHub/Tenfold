IF EXISTS (select * from sys.table_types where user_type_id = Type_id(N'T_MansionStation'))
BEGIN
    IF EXISTS (select * from sys.objects where name = 'pr_a_index_Insert_SellerMansionData')
        DROP PROCEDURE [pr_a_index_Insert_SellerMansionData]

    DROP TYPE [T_MansionStation]
END
GO

CREATE TYPE T_MansionStation AS TABLE
(
    RowNo               int
    ,LineCD             varchar(10)
    ,StationCD          varchar(10)
    ,Distance           int
)
GO
