IF EXISTS (select * from sys.objects where name = 'pr_a_index_Insert_SellerMansionData')
BEGIN
    DROP PROCEDURE [pr_a_index_Insert_SellerMansionData]
END
GO

CREATE PROCEDURE [dbo].[pr_a_index_Insert_SellerMansionData]
(
    @SellerMansionID            varchar(10)
    ,@SellerCD                  varchar(10)
    ,@MansionName               varchar(100)
    ,@MansionCD                 varchar(10)
    ,@LatestRequestDate         date            = NULL
    ,@HoldingStatus             tinyint         = 0
    ,@ZipCode1                  varchar(3)
    ,@ZipCode2                  varchar(4)
    ,@PrefCD                    varchar(2)
    ,@PrefName                  varchar(10)
    ,@CityCD                    varchar(13)
    ,@CityName                  varchar(50)
    ,@TownCD                    varchar(13)
    ,@TownName                  varchar(50)
    ,@Address                   varchar(50)
    ,@StructuralKBN             tinyint
    ,@Floors                    int
    ,@ConstYYYYMM               int
    ,@Rooms                     int
    ,@LocationFloor             int
    ,@RoomNumber                varchar(5)
    ,@RoomArea                  decimal(5,2)
    ,@BalconyKBN                tinyint
    ,@BalconyArea               decimal(5,2)
    ,@Direction                 tinyint
    ,@FloorType                 int
    ,@FloorType1                int
    ,@FloorType2                tinyint
    ,@BathKBN                   tinyint
    ,@RightKBN                  tinyint
    ,@CurrentKBN                tinyint
    ,@ManagementKBN             tinyint
    ,@RentFee                   money
    ,@ManagementFee             money
    ,@RepairFee                 money
    ,@ExtraFee                  money
    ,@PropertyTax               money
    ,@DesiredTime               tinyint
    ,@Remark                    varchar(1000)
    ,@Longitude                 decimal(9,6)
    ,@Latitude                  decimal(9,6)
    ,@Operator                  varchar(10)
    ,@IPAddress                 varchar(20)
    ,@LoginName                 varchar(50)
    ,@MansionStationTable       T_MansionStation READONLY

)
AS
BEGIN

    DECLARE @SysDatetime datetime = GETDATE()

    --ChangeFLG
    DECLARE @AddressChangeFLG       tinyint = 0
           ,@StructuralKBNChangeFLG tinyint = 0
           ,@FloorsChangeFLG        tinyint = 0
           ,@ConstYYYYMMChangeFLG   tinyint = 0
           ,@RoomsChangeFLG         tinyint = 0
           ,@RightKBNChangeFLG      tinyint = 0

    SELECT @AddressChangeFLG        = CASE WHEN [Address] = @Address            THEN 0 ELSE 1 END
           ,@StructuralKBNChangeFLG = CASE WHEN StructuralKBN = @StructuralKBN  THEN 0 ELSE 1 END
           ,@FloorsChangeFLG        = CASE WHEN Floors = @Floors                THEN 0 ELSE 1 END
           ,@ConstYYYYMMChangeFLG   = CASE WHEN ConstYYYYMM = @ConstYYYYMM      THEN 0 ELSE 1 END
           ,@RoomsChangeFLG         = CASE WHEN Rooms = @Rooms                  THEN 0 ELSE 1 END
           ,@RightKBNChangeFLG      = CASE WHEN RightKBN = @RightKBN            THEN 0 ELSE 1 END
    FROM M_Mansion
    WHERE MansionCD = @MansionCD

    -- <<< Declaring Variables


    EXEC pr_M_Counter_GetNewID 
        @CounterKey = 1, 
        @NewID = @SellerMansionID OUTPUT


    INSERT INTO M_SellerMansion
    (
        SellerMansionID
        ,SellerCD
        ,MansionName
        ,MansionCD
        ,LatestRequestDate
        ,HoldingStatus
        ,ZipCode1
        ,ZipCode2
        ,PrefCD
        ,PrefName
        ,CityCD
        ,CityName
        ,TownCD
        ,TownName
        ,[Address]
        ,AddressChangeFLG
        ,StructuralKBN
        ,StructuralKBNChangeFLG
        ,Floors
        ,FloorsChangeFLG
        ,ConstYYYYMM
        ,ConstYYYYMMChangeFLG
        ,Rooms
        ,RoomsChangeFLG
        ,LocationFloor
        ,RoomNumber
        ,RoomArea
        ,BalconyKBN
        ,BalconyArea
        ,Direction
        ,FloorType
        ,FloorType1
        ,FloorType2
        ,BathKBN
        ,RightKBN
        ,RightKBNChangeFLG
        ,CurrentKBN
        ,ManagementKBN
        ,RentFee
        ,ManagementFee
        ,RepairFee
        ,ExtraFee
        ,PropertyTax
        ,DesiredTime
        ,Remark
        ,Longitude
        ,Latitude
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    )
    VALUES
    (
        @SellerMansionID
        ,@SellerCD
        ,@MansionName
        ,@MansionCD
        ,@LatestRequestDate
        ,@HoldingStatus
        ,@ZipCode1
        ,@ZipCode2
        ,@PrefCD
        ,@PrefName
        ,@CityCD
        ,@CityName
        ,@TownCD
        ,@TownName
        ,@Address
        ,@AddressChangeFLG
        ,@StructuralKBN
        ,@StructuralKBNChangeFLG
        ,@Floors
        ,@FloorsChangeFLG
        ,@ConstYYYYMM
        ,@ConstYYYYMMChangeFLG
        ,@Rooms
        ,@RoomsChangeFLG
        ,@LocationFloor
        ,@RoomNumber
        ,@RoomArea
        ,@BalconyKBN
        ,@BalconyArea
        ,@Direction
        ,@FloorType
        ,@FloorType1
        ,@FloorType2
        ,@BathKBN
        ,@RightKBN
        ,@RightKBNChangeFLG
        ,@CurrentKBN
        ,@ManagementKBN
        ,@RentFee
        ,@ManagementFee
        ,@RepairFee
        ,@ExtraFee
        ,@PropertyTax
        ,@DesiredTime
        ,@Remark
        ,@Longitude
        ,@Latitude
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


    INSERT INTO M_SellerMansionStation
    (
         SellerMansionID
        ,StationSEQ
        ,StationCD
        ,StationCDChangeFLG
        ,Distance
        ,DistanceChangeFLG
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    )
    SELECT
         @SellerMansionID
        ,ROW_NUMBER() OVER(ORDER BY A.RowNo)
        ,A.StationCD
        ,CASE WHEN B.StationCD IS NULL OR B.StationCD = A.StationCD THEN 0 ELSE 1 END
        ,A.Distance
        ,CASE WHEN B.StationCD IS NULL OR B.Distance = A.Distance THEN 0 ELSE 1 END
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,NULL
        ,NULL
        ,NULL
    FROM @MansionStationTable AS A
    OUTER APPLY (
        SELECT TOP 1 StationCD, Distance FROM M_MansionStation B WHERE MansionCD = @MansionCD AND B.StationCD = A.StationCD
    ) AS B 


    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @Operator
    --,@RealECD       
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_index'
    ,@ProcessKBN    = 1
    ,@Remarks       = @MansionName

END
