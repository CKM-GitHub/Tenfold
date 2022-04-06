IF EXISTS (select * from sys.objects where name = 'pr_t_seller_list_Select_DropDownlistofPref')
BEGIN
    DROP PROCEDURE [pr_t_seller_list_Select_DropDownlistofPref]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_list_Select_DropDownlistofPref]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT PrefName AS DisplayText,
	PrefCD AS [Value]
	from M_Pref 
	Order by DisplayOrder
END
