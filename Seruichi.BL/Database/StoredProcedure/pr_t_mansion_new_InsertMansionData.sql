IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_new_InsertMansionData')
BEGIN
    DROP PROCEDURE [pr_t_mansion_new_InsertMansionData]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_new_InsertMansionData]
	-- Add the parameters for the stored procedure here
  @MansionCD varchar(10),
  @MansionName varchar(100),
  @ZipCode1 varchar(3),
  @ZipCode2 varchar(4),
  @PrefCD varchar(2),
  @PrefName varchar(10),
  @CityCD varchar(13),
  @CityName varchar(50),
  @TownCD varchar(13),
  @TownName varchar(50),
  @Address varchar(50),
  @StructuralKBN tinyint,
  @ConstYYYYMM int,
  @RightKBN int,
  @Rooms int,
  @Floors int,
  @Remark varchar(1000),
  @Longitude decimal(9,6),
  @Latitude decimal(9,6),
  @MansionWord varchar(100),
  @Operator                  varchar(10),
  @IPAddress                 varchar(20),
  @LoginName                 varchar(50),
  @MansionStationTable       T_MansionStation READONLY
 ,@MansionWordTable       T_MansionWord READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SysDatetime datetime = GETDATE()
    -- Insert statements for procedure here
	EXEC pr_M_Counter_GetNewID 
        @CounterKey = 7, 
        @NewID = @MansionCD OUTPUT

		 Insert Into M_Mansion
		 (
                MansionCD,
				MansionName,
				MansionShortName,
				ZipCode1,
				ZipCode2,
				PrefCD,
				PrefName,
				CityCD,
				CityName,
				TownCD,
				TownName,
				Address,
				StructuralKBN,
				ConstYYYYMM,
				Rooms,
				Floors,
				RightKBN,
				Remark,
				Longitude,
				Latitude,
				InsertOperator,
				InsertDateTime,
				InsertIPAddress,
				UpdateOperator,
				UpdateDateTime,
				UpdateIPAddress,
				DeleteDateTime,
				DeleteOperator,
				DeleteIPAddress
		)
		Values
		(
				@MansionCD,
				@MansionName,
				@MansionName,
				@ZipCode1,
				@ZipCode2,
				@PrefCD,
				@PrefName,
				@CityCD,
				@CityName,
				@TownCD,
				@TownName,
				@Address,
				@StructuralKBN,
				@ConstYYYYMM,
				@Rooms,
				@Floors,
				@RightKBN,
				@Remark,
				@Longitude,
				@Latitude,
				@Operator,
				@SysDatetime,
				@IPAddress,
				@Operator,
				@SysDatetime,
				@IPAddress,
				Null,
				Null,
				Null
		)


		Insert Into M_MansionStation
		(
					MansionCD,
					StationSEQ
					,StationCD
					,Distance,
					InsertOperator,
					InsertDateTime,
					InsertIPAddress,
					UpdateOperator,
					UpdateDateTime,
					UpdateIPAddress,
					DeleteOperator,
					DeleteDateTime,
					DeleteIPAddress
			)
			Select
			      
			        @MansionCD,
					 ROW_NUMBER() OVER(ORDER BY A.RowNo)
					,A.StationCD
					,A.Distance,
					@Operator,
					@SysDatetime,
					@IPAddress
					,@Operator,
					@SysDatetime,
					@IPAddress,
					Null,
					Null,
					Null
			 FROM @MansionStationTable AS A
			OUTER APPLY (
			SELECT TOP 1 StationCD, Distance FROM M_MansionStation B WHERE MansionCD = @MansionCD AND B.StationCD = A.StationCD
			) AS B 


		Insert Into M_MansionWord
		(
					MansionCD,
					WordSEQ,
					MansionWord,
					InsertOperator,
					InsertDateTime,
					InsertIPAddress,
					UpdateOperator
					,UpdateDateTime,
					UpdateIPAddress,
					DeleteDateTime,
					DeleteIPAddress,
					DeleteOperator
			)
			SELECT 
			        @MansionCD,
					W.WordSEQ,
					W.Word,
					@Operator,
					@SysDatetime,
					@IPAddress,
					@Operator,
					@SysDatetime,
					@IPAddress,
					Null,
					Null,
					Null
			 FROM @MansionWordTable AS  W
			
			

  EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @Operator
    --,@RealECD       
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 't_mansion_new'
    ,@ProcessKBN    = 1
    ,@Remarks       = @MansionCD
	
END

