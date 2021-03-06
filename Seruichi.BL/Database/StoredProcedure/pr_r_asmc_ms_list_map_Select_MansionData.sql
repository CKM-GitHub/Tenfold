IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_ms_list_map_Select_MansionData')
BEGIN
    DROP PROCEDURE [pr_r_asmc_ms_list_map_Select_MansionData]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_ms_list_map_Select_MansionData]
(
     @RealECD           varchar(10)
    ,@MansionName       varchar(100)
    ,@CityCDCsv         varchar(max)
    ,@TownCDCsv         varchar(max)
    ,@YearFrom          int
    ,@YearTo            int
    ,@DistanceFrom      int
    ,@DistanceTo        int
    ,@RoomsFrom         int
    ,@RoomsTo           int
    ,@Unregistered      tinyint
    ,@Priority          tinyint
)
AS    
BEGIN

    SET NOCOUNT ON

    DECLARE @SysDate date = GETDATE()

    SELECT DISTINCT
         mm.MansionCD
        ,mm.MansionName
        ,CASE vrm.Priority 
            WHEN 1 THEN '★☆☆☆☆'
            WHEN 2 THEN '★★☆☆☆'
            WHEN 3 THEN '★★★☆☆'
            WHEN 4 THEN '★★★★☆'
            WHEN 5 THEN '★★★★★'
            ELSE '☆☆☆☆☆' END AS Priority
        ,ISNULL(vrm.ValidFLG,9) AS ValidFLG
        ,CASE vrm.ValidFLG 
            WHEN 0 THEN '(未公開)' 
            WHEN 1 THEN '(公開済)' 
            ELSE '' END AS ValidFLGText
        ,ISNULL(FORMAT(mm.ConstYYYYMM, '0000/00'),'') AS ConstYYYYMM
        ,dbo.fn_GetBuildingAge(mm.ConstYYYYMM) AS BuildingAge
        ,ms.Distance
        ,mm.Rooms
        ,ISNULL(FORMAT(ISNULL(vrm.UpdateDateTime, vrm.InsertDateTime), 'yyyy/MM/dd'),'') AS UpdateDateTime
        ,ISNULL(FORMAT(vrm.ExpDate, 'yyyy/MM/dd'),'') AS ExpDate
        ,CASE WHEN vrm.[期限切れ] = 1 THEN '期限切れ' WHEN vrm.[期限切れ間近] = 1 THEN '期限切れ間近' ELSE '' END AS ExpDateText
        ,ISNULL(mm.PrefName,'') AS PrefName
        ,ISNULL(mm.CityName,'') AS CityName
        ,ISNULL(mm.TownName,'') AS TownName
        ,ISNULL(mm.Address,'')  AS Address
        ,CASE WHEN vrm.MansionCD IS NULL THEN 0 ELSE 1 END AS RegisteredFLG
        ,CASE WHEN vrm.MansionCD IS NULL THEN '未登録' ELSE '登録済' END AS RegisteredText
        ,mm.Latitude
        ,mm.Longitude
    FROM M_Mansion mm
    LEFT OUTER JOIN V_RECondMan vrm
       ON vrm.MansionCD = mm.MansionCD
      AND vrm.RealECD = @RealECD 
    OUTER APPLY (
        SELECT TOP 1 Distance FROM M_MansionStation WHERE MansionCD = mm.MansionCD ORDER BY Distance
    ) ms
    WHERE mm.NoDisplayFLG = 0
      AND mm.DeleteDateTime IS NULL
      AND mm.Latitude IS NOT NULL
      AND mm.Longitude IS NOT NULL
      AND (@YearTo       IS NULL OR mm.ConstYYYYMM >= dbo.fn_GetMinBuiltYearMonth(@YearTo))
      AND (@YearFrom     IS NULL OR mm.ConstYYYYMM <= dbo.fn_GetMaxBuiltYearMonth(@YearFrom))
      AND (@DistanceFrom IS NULL OR ms.Distance >= @DistanceFrom)
      AND (@DistanceTo   IS NULL OR ms.Distance <= @DistanceTo)
      AND (@RoomsFrom    IS NULL OR mm.Rooms >= @RoomsFrom)
      AND (@RoomsTo      IS NULL OR mm.Rooms <= @RoomsTo)
      AND (@MansionName  IS NULL OR (mm.MansionCD IN (SELECT MansionCD From M_MansionWord WHERE MansionWord LIKE '%' + @MansionName + '%')))  
      AND (@CityCDCsv    IS NULL OR mm.CityCD IN (SELECT value FROM string_split(@CityCDCsv, ',')))
      AND (@TownCDCsv    IS NULL OR mm.TownCD IN (SELECT value FROM string_split(@TownCDCsv, ',')))
      AND (@Unregistered = 1 OR vrm.MansionCD IS NOT NULL) 
      AND (@Priority = 0 OR vrm.Priority >= @Priority)

END