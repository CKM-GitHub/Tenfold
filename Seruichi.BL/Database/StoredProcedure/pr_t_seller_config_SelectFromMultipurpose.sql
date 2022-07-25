IF EXISTS (select * from sys.objects where name = 'pr_t_seller_config_SelectFromMultipurpose')
BEGIN
    DROP PROCEDURE [pr_t_seller_config_SelectFromMultipurpose]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_config_SelectFromMultipurpose]
	-- Add the parameters for the stored procedure here
	@DataID as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Num1 from M_MultPurpose where DataID = @DataID and DataKey='1'
END
