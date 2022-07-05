IF EXISTS (select * from sys.objects where name = 'pr_r_asmc_ms_map_add_Select_Towns_by_Cities')
BEGIN
    DROP PROCEDURE [pr_r_asmc_ms_map_add_Select_Towns_by_Cities]
END
GO

CREATE PROCEDURE [dbo].[pr_r_asmc_ms_map_add_Select_Towns_by_Cities]
(
    @CitycdCsv  varchar(max)
    ,@RealECD   varchar(10)
)
AS
BEGIN

    DECLARE @SysDate date = GETDATE()

    SELECT DISTINCT
         ADR.PrefCD
        ,ADR.PrefName
        ,ADR.CityCD
        ,ADR.CityName
        ,ADR.TownCD
        ,ADR.TownName
        ,ADR.TownKana
        ,ISNULL(MAN.Kensu,0)        AS MansionCount
        ,ISNULL(RES.Kensu,0)        AS RealEstateCount
        ,ADR.DisplayOrder

        ,ISNULL(MyRES.ValidFLG, 0)  AS ValidFLG
        --ValidFLG��NULL��ExistsFlg���P�̏ꍇ�A����f�[�^�����ׂĖ������A�����؂�Ƃ�������
        ,CASE WHEN MyRES.ValidFLG IS NULL THEN MyRES2.ExistsFlg ELSE 0 END AS ExpirationFlag

    FROM M_Address   ADR          ---�Z���}�X�^ 
    ----------------------------------------------------------------------
    -- �o�^�}���V���������̎Q��
    -- �@�}���V�����}�X�^
    --   ����R�[�h�ŃO���[�v�����ēo�^�}���V���������擾
    ----------------------------------------------------------------------
    CROSS APPLY (
                SELECT TownCD, Count(MansionCD) AS Kensu
                FROM M_Mansion  
                WHERE TownCD = ADR.TownCD
                AND   NoDisplayFLG = 0
                GROUP BY TownCD 
                ) AS MAN 
    ----------------------------------------------------------------------
    -- �o�^��А��̎Q��
    --   �G���A��������n��}�X�^ 
    --   ����R�[�h�ŃO���[�v�����Ď��ƎҌ������擾
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT M.TownCD, Count(DISTINCT t1.RealECD) AS Kensu
                FROM M_RECondMan t1 
                INNER JOIN M_Mansion M ON M.MansionCD = t1.MansionCD
                WHERE TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND  (t1.ExpDate IS NULL OR t1.ExpDate >= @SysDate)
                AND   t1.DisabledFlg = 0
                AND   M.NoDisplayFLG = 0
                GROUP BY TownCD
                ) AS RES
    ----------------------------------------------------------------------
    -- ���Џ��
    ----------------------------------------------------------------------
    OUTER APPLY (
                SELECT 
                     MIN(t1.ValidFLG) + 1   AS ValidFLG
                FROM M_RECondMan t1
                INNER JOIN M_Mansion M ON M.MansionCD = t1.MansionCD
                WHERE t1.RealECD = @RealECD
                AND   M.TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND  (t1.ExpDate IS NULL OR t1.ExpDate >= @SysDate)
                AND   t1.DisabledFlg = 0
                AND   M.NoDisplayFLG = 0
                GROUP BY CityCD 
                ) AS   MyRES

    OUTER APPLY (
                SELECT TOP 1
                     1 AS ExistsFlg
                FROM M_RECondMan t1
                INNER JOIN M_Mansion M ON M.MansionCD = t1.MansionCD
                WHERE t1.RealECD = @RealECD
                AND   M.TownCD = ADR.TownCD
                AND   t1.DeleteDateTime IS NULL
                AND   M.NoDisplayFLG = 0
                ) AS   MyRES2

    WHERE ADR.NoDisplayFLG = 0  --�\���ΏۊO�͏���
      AND ADR.AddressLevel = 2  --AddressLevel=1 �͏���
      AND ADR.CityCD IN (SELECT value FROM string_split(@CitycdCsv, ','))
  
    ORDER BY ADR.PrefCD, ADR.CityCD, ADR.TownKana

END
