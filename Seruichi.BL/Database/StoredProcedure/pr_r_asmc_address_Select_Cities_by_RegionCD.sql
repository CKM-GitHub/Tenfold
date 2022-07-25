IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_address_Select_Cities_by_RegionCD')
BEGIN
    DROP PROCEDURE [pr_r_asmc_address_Select_Cities_by_RegionCD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_address_Select_Cities_by_RegionCD]
(
    @RegionCD   varchar(2)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = GETDATE()

    SELECT DISTINCT 
         ADR.PrefCD
        ,ADR.PrefName
        ,ADR.CityGPCD               AS CityGroupCD
        ,ADR.CityGPName             AS CityGroupName
        ,ADR.CityCD
        ,ADR.CityName
        ,ISNULL(MAN.Kensu,0)        AS MansionCount
        ,ISNULL(RES.Kensu,0)        AS RealEstateCount
        ,ADR.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 9)  AS ValidFLG
        ,ISNULL(MyRES2.Expired, 0)  AS Expired

        ,CAST(CASE WHEN VCon.ConditionSEQ IS NOT NULL THEN 1 ELSE 0 END  AS bit) AS IsShowCheckTab

    FROM M_Address    ADR --�Z���}�X�^
    INNER JOIN M_Pref PRF ON ADR.PrefCD = PRF.PrefCD   -- �n���}�X�^ 
    ----------------------------------------------------------------------
    -- �o�^�}���V���������̎Q��
    -- �@�}���V�����}�X�^
    --   �s�撬���R�[�h�ŃO���[�v�����ēo�^�}���V���������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT CityCD, Count(MansionCD) AS Kensu
                    FROM M_Mansion 
                    WHERE CityCD = ADR.CityCD
                    AND   NoDisplayFLG = 0
                    GROUP BY CityCD                    
                ) AS MAN 
    ----------------------------------------------------------------------
    -- �o�^��А��̎Q��
    --   �G���A��������n��}�X�^ 
    --   �s�撬���R�[�h�ŃO���[�v�����Ď��ƎҌ������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT t1.CityCD ,Count(DISTINCT t1.RealECD) AS Kensu
                    FROM M_RECondAreaSec t1 
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= @SysDate)
                    AND   t1.DisabledFlg = 0
                    GROUP BY CityCD 
                ) AS   RES
    ----------------------------------------------------------------------
    -- ���Џ��
    ----------------------------------------------------------------------
    OUTER APPLY (
                    SELECT 
                        MIN(t2.ValidFLG) AS ValidFLG
                    FROM M_RECondAreaSec t1
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND  (t2.ExpDate IS NULL OR t2.ExpDate >= DATEADD(d, 7, @SysDate))
                    AND   t1.DisabledFlg = 0
                    GROUP BY CityCD 
                ) AS   MyRES

    --�����؂�̗L���f�[�^
    OUTER APPLY (
                    SELECT TOP 1
                        1 AS Expired 
                    FROM M_RECondAreaSec t1
                    INNER JOIN M_RECondArea t2 ON t1.RealECD = t2.RealECD AND t1.ConditionSEQ = t2.ConditionSEQ 
                    WHERE t1.RealECD = @RealECD
                    AND   t1.CityCD = ADR.CityCD
                    AND   t1.DeleteDateTime IS NULL
                    AND   t2.DeleteDateTime IS NULL
                    AND   t2.ExpDate < DATEADD(d, 7, @SysDate)
                    AND   t1.DisabledFlg = 0
                ) AS   MyRES2

    ----------------------------------------------------------------------
    -- ���Џ��(City�ɑ΂��Đݒ肵���������)
    ----------------------------------------------------------------------
    LEFT OUTER JOIN V_RECondAreaSec VCon 
    �@ON  VCon.RealECD = @RealECD 
    �@AND VCon.PrefCD = ADR.PrefCD
   �@ AND VCon.CityCD = ADR.CityCD
   �@ AND VCon.TownCD = '' --View�ł�NULL��''

    WHERE ADR.NoDisplayFLG = 0  --�\���ΏۊO�͏���
      AND ADR.AddressLevel = 2  --AddressLevel=1 �͏���
      AND PRF.RegionCD = @RegionCD
  
    ORDER BY ADR.PrefCD, ADR.CityGPCD, ADR.CityCD, ADR.DisplayOrder

END