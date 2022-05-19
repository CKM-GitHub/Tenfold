IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_list_Select_M_Mansion')
BEGIN
    DROP PROCEDURE [pr_t_mansion_list_Select_M_Mansion]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_list_Select_M_Mansion] 
	-- Add the parameters for the stored procedure here
	@StartAge as int,
	@EndAge as int,
	@StartUnit as int,
	@EndUnit as int,
	@Apartment as varchar(100),
	@CityCD as varchar(max),
	@CityGPCD as varchar(max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	ROW_NUMBER() OVER(ORDER BY A.CityCD) AS [No],
	A.MansionCD as N'マンションCD',
	A.MansionName as N'マンション名',
	A.PrefName+A.CityName+A.TownCD+A.[Address] as N'住所',
	ISNull( STUFF(A.ConstYYYYMM, 5, 0, '/'),0)as N'築年月',
	--A.ConstYYYYMM as N'築年月',
	DATEDIFF(year, (datefromparts(Left(ConstYYYYMM,4),RIGHT(ConstYYYYMM,2),'01')), GETDATE()) AS N'築年数',
	A.Rooms as N'総戸数'
	From M_Mansion A
	Left Outer Join M_Address B on B.TownCD = A.TownCD
	Left Outer Join M_MansionWord C on C.MansionCD = A.MansionCD
	Where (@StartAge is null or A.ConstYYYYMM >= @StartAge)
	and (@EndAge is null or A.ConstYYYYMM <= @EndAge)
	and (@StartUnit is null or A.Rooms >= @StartUnit)
	and (@EndUnit is null or A.Rooms <= @EndUnit)
	and A.NoDisplayFLG = 0
	and (@Apartment is null or (C.MansionWord Like  '%' + @Apartment + '%'))
	and ((A.CityCD in (SELECT value FROM string_split(@CityCD, ',')))
		or (B.CityGPCD in (SELECT value FROM string_split(@CityGPCD, ','))))
	order by A.CityCD,A.MansionName
END
