IF EXISTS (select * from sys.table_types where user_type_id = Type_id(N'T_MansionStationName'))
BEGIN
    IF EXISTS (select * from sys.objects where name = 'pr_Common_Select_LineChange')
        DROP PROCEDURE [pr_Common_Select_LineChange]

    DROP TYPE [T_MansionStationName]
END
GO

CREATE TYPE T_MansionStationName AS TABLE
(
    RowNo               int
    ,LineName           varchar(max)
    ,StationName        varchar(max)
    ,Distance           int
)
GO
