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
        ,MyRES.ExpDate

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
                    SELECT  S1.StationCD
                            ,Count(DISTINCT S1.MansionCD) AS Kensu
                    FROM M_MansionStation S1
                    WHERE S1.StationCD = STN.StationCD
                    GROUP BY S1.StationCD
                ) MAN 
    ----------------------------------------------------------------------
    -- �o�^��А��̎Q��
    --   �H���w��������w�}�X�^  
    --   �w�R�[�h�ŃO���[�v�����Ď��ƎҌ������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT S1.StationCD
                        ,Count(DISTINCT S1.RealECD) AS Kensu
                    FROM M_RECondLineSta S1
                    WHERE S1.StationCD = STN.StationCD
                    GROUP BY StationCD
                ) RES
    ----------------------------------------------------------------------
    -- ���Џ��
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT 
                     MAX(t2.ValidFLG)   AS ValidFLG
                    ,MAX(t2.ExpDate)    AS ExpDate
                FROM M_RECondLineSta t1
                INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                WHERE t1.RealECD = @RealECD
                AND   t1.StationCD = STN.StationCD
                AND   t1.DeleteDateTime IS NULL
                AND   t2.DeleteDateTime IS NULL
                AND   t2.ExpDate > GETDATE()
                GROUP BY StationCD 
                ) AS   MyRES

    WHERE STN.NoDisplayFLG = 0  --�\���ΏۊO�͏���
      AND STN.LineCD IN (SELECT value FROM string_split(@LinecdCsv, ','))
  
    ORDER BY PrefCD, CityCD, DisplayOrder

END
