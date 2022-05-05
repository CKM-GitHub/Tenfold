IF EXISTS (select * from sys.objects where name = 'pr_r_dashboard_SelectForOldestDate')
BEGIN
    DROP PROCEDURE [pr_r_dashboard_SelectForOldestDate]
END
GO

CREATE PROCEDURE [dbo].[pr_r_dashboard_SelectForOldestDate]
	-- Add the parameters for the stored procedure here
	@realECD as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FORMAT(Min(MinDT),'yyyy/MM/dd') AS MinDate
	From
	(
		Select  Min(ConfDateTime) AS MinDT
		From   M_RECondArea
		Where RealECD=@realECD
		and ExpDate  >=GetDate()
		and ValidFLG =1                 　
		and DeleteDateTime Is Null　　　　　
	UNION ALL
		Select  Min(ConfDateTime) AS MinDT
		From   M_RECondLine
		Where RealECD=@realECD
		and ExpDate >=GetDate()
		and ValidFLG=1                 　
		and DeleteDateTime Is Null　　　　　
	UNION ALL
		Select Min(ConfDateTime) AS MinDT
		From   M_RECondMan
		Where RealECD=@realECD
		and  ExpDate >=GetDate()
		and ValidFLG=1                 　
		and DeleteDateTime Is Null　　　　　
	) As Main

END
