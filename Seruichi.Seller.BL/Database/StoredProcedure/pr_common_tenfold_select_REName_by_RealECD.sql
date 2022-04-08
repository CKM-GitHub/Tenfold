IF EXISTS (select * from sys.objects where name = 'pr_common_tenfold_select_REName_by_RealECD')
BEGIN
    DROP PROCEDURE [pr_common_tenfold_select_REName_by_RealECD]
END
GO
CREATE PROCEDURE [dbo].[pr_common_tenfold_select_REName_by_RealECD]	
	@RealECD as varchar(10)
AS
BEGIN
	select REName from M_RealEstate where RealECD=@RealECD
END
