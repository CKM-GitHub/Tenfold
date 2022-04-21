IF EXISTS (select * from sys.objects where name = 'pr_common_tenfold_select_MansionName_by_MansionCD')
BEGIN
    DROP PROCEDURE [pr_common_tenfold_select_MansionName_by_MansionCD]
END
GO
CREATE PROCEDURE [dbo].[pr_common_tenfold_select_MansionName_by_MansionCD]
	-- Add the parameters for the stored procedure here
	 @MansionCD varchar(10)
AS
BEGIN
	select MansionName from M_Mansion where MansionCD=@MansionCD
END
