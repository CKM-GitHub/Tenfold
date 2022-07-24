IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_set_ms_Select_V_RECondMan_by_MansionCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_set_ms_Select_V_RECondMan_by_MansionCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_set_ms_Select_V_RECondMan_by_MansionCD]
(
    @RealECD   varchar(10)
    ,@MansionCD varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = getdate()

    SELECT TOP 1
         Man.MansionCD
        ,VCon.RealECD
        ,VCon.ConditionSEQ
        ,VCon.PrecedenceFlg
        ,VCon.NotApplicableFlg
        ,VCon.ValidFLG
        ,FORMAT(VCon.ExpDate, 'yyyy-MM-dd') AS ExpDate
        ,VCon.[ŠúŒÀØ‚ê] AS Expired
        ,VCon.[ŠúŒÀØ‚êŠÔ‹ß] AS ExpiredSoon
        ,VCon.Priority
        ,VCon.Remark
        ,VCon.REStaffCD
        ,VCon.REStaffName

        ,Man.MansionName
        ,ISNULL(Man.PrefName,'') + ISNULL(Man.CityName,'') + ISNULL(Man.TownName,'') + ISNULL(Man.Address,'') AS Address 
        ,FORMAT(ISNULL(RES.Kensu, 0),'#0') AS RealEstateCount

    FROM M_Mansion Man

    LEFT OUTER JOIN V_RECondMan VCon
    ON  VCon.RealECD = @RealECD
    AND VCon.MansionCD = Man.MansionCD

    OUTER APPLY (
                    SELECT t1.MansionCD ,Count(DISTINCT t1.RealECD) AS Kensu
                    FROM M_RECondMan t1 
                    WHERE t1.MansionCD = VCon.MansionCD
                    AND   t1.DeleteDateTime IS NULL
                    AND  (t1.ExpDate IS NULL OR t1.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY t1.MansionCD 
                ) AS   RES

    WHERE Man.MansionCD = @MansionCD

END
