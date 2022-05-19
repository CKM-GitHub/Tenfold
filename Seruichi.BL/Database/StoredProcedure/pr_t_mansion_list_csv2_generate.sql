IF EXISTS (select * from sys.objects where name = 'pr_t_mansion_list_csv2_generate')
BEGIN
    DROP PROCEDURE [pr_t_mansion_list_csv2_generate]
END
GO

CREATE PROCEDURE [dbo].[pr_t_mansion_list_csv2_generate]
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
	SELECT A.MansionCD,
	A.MansionName,
	D.StationSEQ,
	E.LineCD,
	F.LineName,
	D.StationCD,
	E.StationName,
	D.Distance
	From M_Mansion A
	Left Outer Join M_MansionStation D on D.MansionCD = A.MansionCD
	Left Outer Join M_Address B on B.TownCD = A.TownCD
	Left Outer Join M_MansionWord C on C.MansionCD = A.MansionCD
	Left Outer Join M_Station E on E.StationCD = D.StationCD
	Left Outer Join M_Line F on F.LineCD = E.LineCD
	Where (@StartAge is null or A.ConstYYYYMM >= @StartAge)
	and (@EndAge is null or A.ConstYYYYMM <= @EndAge)
	and (@StartUnit is null or A.Rooms >= @StartUnit)
	and (@EndUnit is null or A.Rooms <= @EndUnit)
	and A.NoDisplayFLG = 0
	and (@Apartment is null or(C.MansionWord Like  '%' + @Apartment + '%'))
	and ((A.CityCD in (SELECT value FROM string_split(@CityCD, ',')))
		or (B.CityGPCD in (SELECT value FROM string_split(@CityGPCD, ','))))
	order by A.CityCD,A.MansionName,A.MansionCD,D.StationSEQ
END
