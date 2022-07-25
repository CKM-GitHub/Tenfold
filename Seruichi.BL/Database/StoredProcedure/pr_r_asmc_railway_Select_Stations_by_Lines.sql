IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_railway_Select_Stations_by_Lines')
BEGIN
    DROP PROCEDURE [pr_r_asmc_railway_Select_Stations_by_Lines]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_railway_Select_Stations_by_Lines]
(
    @LinecdCsv  varchar(max)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = GETDATE()

    SELECT DISTINCT
         PRF.PrefCD
        ,PRF.PrefName
        ,STN.LineCD                 AS CityCD
        ,LIN.LineName               AS CityName
        ,STN.StationCD              AS TownCD
        ,STN.StationName            AS TownName
        ,ISNULL(MAN.Kensu,0)        AS MansionCount
        ,ISNULL(RES.Kensu,0)        AS RealEstateCount
        ,STN.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 9)  AS ValidFLG
        ,ISNULL(MyRES2.Expired, 0)  AS Expired

    FROM M_Pref PRF
    INNER JOIN M_Station STN ON PRF.PrefCD = STN.PrefCD
    INNER JOIN M_Line LIN ON STN.LineCD = LIN.LineCD
    --INNER JOIN M_RailCompany RCP ON LIN.CompanyCD = RCP.CompanyCD
    ----------------------------------------------------------------------
    -- �o�^�}���V���������̎Q��
    -- �@�}���V�����Ŋ�w�}�X�^
    --   �Ŋ�w�R�[�h�ŉw�}�X�^���������A�wCD���擾
    --   �擾�����w�R�[�h�ŃO���[�v�����ēo�^�}���V���������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT  t1.StationCD
                            ,Count(DISTINCT t1.MansionCD) AS Kensu
                    FROM M_MansionStation t1
                    INNER JOIN M_Mansion M ON M.MansionCD = t1.MansionCD
                    WHERE t1.StationCD = STN.StationCD
                    AND   M.NoDisplayFLG = 0
                    GROUP BY t1.StationCD
                ) MAN 
    ----------------------------------------------------------------------
    -- �o�^��А��̎Q��
    --   �H���w��������w�}�X�^  
    --   �w�R�[�h�ŃO���[�v�����Ď��ƎҌ������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT t1.StationCD
                        ,Count(DISTINCT t1.RealECD) AS Kensu
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.StationCD = STN.StationCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY StationCD
                ) RES
    ----------------------------------------------------------------------
    -- ���Џ��
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT 
                     MIN(t2.ValidFLG) AS ValidFLG
                FROM M_RECondLineSta t1
                INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE t1.RealECD = @RealECD
                AND   t1.StationCD = STN.StationCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                AND  (t2.ExpDate IS NULL OR t2.ExpDate >= DATEADD(d, 7, @SysDate))
                AND   t1.DisabledFlg = 0
                GROUP BY StationCD 
                ) AS   MyRES

    OUTER APPLY (
                SELECT TOP 1
                     1 AS Expired
                FROM M_RECondLineSta t1
                INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE t1.RealECD = @RealECD
                AND   t1.StationCD = STN.StationCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                AND   t2.ExpDate < DATEADD(d, 7, @SysDate)
                AND   t1.DisabledFlg = 0
                ) AS   MyRES2

    WHERE STN.NoDisplayFLG = 0  --�\���ΏۊO�͏���
      AND STN.LineCD IN (SELECT value FROM string_split(@LinecdCsv, ','))
  
    ORDER BY PrefCD, CityCD, DisplayOrder

END
