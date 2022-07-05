IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_railway_Select_Lines_by_RegionCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_railway_Select_Lines_by_RegionCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_railway_Select_Lines_by_RegionCD]
(
    @RegionCD   varchar(2)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = GETDATE()

    SELECT DISTINCT 
        PRF.PrefCD
        ,PRF.PrefName
        ,LIN.CompanyCD          AS CityGroupCD
        ,RCP.CompanyName        AS CityGroupName
        ,STN.LineCD             AS CityCD
        ,LIN.LineName           AS CityName
        ,ISNULL(MAN.Kensu,0)    AS MansionCount
        ,ISNULL(RES.Kensu,0)    AS RealEstateCount
        ,LIN.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 0)  AS ValidFLG
        ,CASE WHEN MyRES2.ExpExistsFlg = 1 OR  MyRES3.AllDisabledFlg = 1 THEN 1 ELSE 0 END AS ExpirationFlag

    FROM M_Pref PRF
    INNER JOIN M_Station STN ON PRF.PrefCD = STN.PrefCD
    INNER JOIN M_Line LIN ON STN.LineCD = LIN.LineCD
    INNER JOIN M_RailCompany RCP ON LIN.CompanyCD = RCP.CompanyCD
    ----------------------------------------------------------------------
    -- �o�^�}���V���������̎Q��
    -- �@�}���V�����Ŋ�w�}�X�^
    --   �Ŋ�w�R�[�h�ŉw�}�X�^���������A�H��CD���擾
    --   �擾�����H���R�[�h�ŃO���[�v�����ēo�^�}���V���������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT t2.LineCD
                        , Count(DISTINCT t1.MansionCD) AS Kensu
                    FROM M_MansionStation t1 
                    INNER JOIN M_Station t2 ON t1.StationCD = t2.StationCD
                    INNER JOIN M_Mansion M ON M.MansionCD = t1.MansionCD
                    WHERE t2.LineCD = LIN.LineCD
                    AND   M.NoDisplayFLG = 0
                    GROUP BY t2.LineCD                    
                ) AS MAN 
    ----------------------------------------------------------------------
    -- �o�^��А��̎Q��
    --   �H���w��������w�}�X�^  
    --   �H���R�[�h�ŃO���[�v�����Ď��ƎҌ������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT t1.LineCD
                        , Count(DISTINCT t1.RealECD) AS Kensu
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.LineCD = LIN.LineCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY LineCD
                ) RES

    ----------------------------------------------------------------------
    -- ���Џ��
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT 
                        MIN(t2.ValidFLG) + 1   AS ValidFLG
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.LineCD = LIN.LineCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY LineCD
                ) AS   MyRES

    --�����؂�̗L���f�[�^
    OUTER APPLY (
                    SELECT TOP 1
                        1 AS ExpExistsFlg 
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.LineCD = LIN.LineCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND   t2.ExpDate < @SysDate
                    AND   t1.DisabledFlg = 0
                ) AS   MyRES2

    --���ׂĖ���
    OUTER APPLY (
                    SELECT
                        MIN(DisabledFlg) AS AllDisabledFlg
                    FROM M_RECondLineSta t1
                    INNER JOIN M_RECondLine t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.LineCD = LIN.LineCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                ) AS   MyRES3

        WHERE STN.NoDisplayFLG = 0  --�\���ΏۊO�͏���
          AND PRF.RegionCD = @RegionCD
  
        ORDER BY PrefCD, CityGroupCD, CityGroupName, DisplayOrder

END