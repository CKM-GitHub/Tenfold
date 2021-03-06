IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_ms_reged_list_Select_M_RECondMan')
BEGIN
    DROP PROCEDURE [pr_r_asmc_ms_reged_list_Select_M_RECondMan]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_ms_reged_list_Select_M_RECondMan]
	@RealECD varchar(10),
	@MansionName varchar(100),
	@CityCD varchar(max),
	@CityGPCD as varchar(max),
	@StartYear int,
	@EndYear int,
	@Rating int
	
AS
BEGIN
Declare @flag tinyint=0
if @CityCD is null AND @CityGPCD is null
set @flag=1

 SELECT Row_Number() Over (Order By REM.MansionCD) As [NO]
,MAN.MansionName AS N'マンション名'
,MAN.PrefName+MAN.CityName+MAN.TownName+MAN.[Address] AS N'住所'
,(case when REM.ValidFLG = '0' then N'未公開' else N'公開済' end )AS N'公開フラグ'
,ISNULL(FORMAT(REM.UpdateDateTime, 'yyyy/MM/dd HH:mm:ss'),FORMAT(REM.InsertDateTime, 'yyyy/MM/dd HH:mm:ss')) AS N'登録日'
,CASE WHEN REM.Priority=0 THEN '☆☆☆☆☆'
         WHEN REM.Priority=1 THEN '★☆☆☆☆'
         WHEN REM.Priority=2 THEN '★★☆☆☆'
         WHEN REM.Priority=3 THEN '★★★☆☆'
         WHEN REM.Priority=4 THEN '★★★★☆'
         WHEN REM.Priority=5 THEN '★★★★★'
 END AS N'優先度マーク'
,REM.MansionCD  AS N'マンションCD'
,ADR.TownKana AS '住所カナ'
,REM.Priority
From M_RECondMan REM
LEFT JOIN M_Mansion MAN ON REM.MansionCD = MAN.MansionCD 
LEFT JOIN M_Address ADR ON MAN.PrefCD = ADR.PrefCD
　　AND MAN.CityCD = ADR.CityCD
　　AND MAN.TownCD = ADR.TownCD
Where REM.RealECD = @RealECD
    and REM.DisabledFlg= 0
	and (@flag=1 OR 				((MAN.CityCD in (SELECT value FROM string_split(@CityCD, ',')))					OR (ADR.CityGPCD in (SELECT value FROM string_split(@CityGPCD, ',')))))
	--and (@CityCD is null or (MAN.CityCD in (SELECT value FROM string_split(@CityCD, ','))))
	and (@MansionName is null or (MAN.MansionCD IN (Select MMW.MansionCD From M_MansionWord MMW
　　　　　　　　　　　　　　　　　　　　　Where MMW.MansionWord Like '%' + @MansionName + '%')))
	--and (@MansionName is null or (MAN.MansionName Like  '%' + @MansionName + '%'))
    --and (@StartYear is null or (MAN.ConstYYYYMM  Between @StartYear and @EndYear)) 
	and (@StartYear is null or MAN.ConstYYYYMM >= @StartYear)
	and (@EndYear is null or MAN.ConstYYYYMM <= @EndYear)
    and (@Rating is null or (REM.Priority >=@Rating ))　　　　                  
 Order By REM.MansionCD

END