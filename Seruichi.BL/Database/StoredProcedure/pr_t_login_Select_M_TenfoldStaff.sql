IF EXISTS (select * from sys.objects where name = 'pr_t_login_Select_M_TenfoldStaff')
BEGIN
    DROP PROCEDURE [pr_t_login_Select_M_TenfoldStaff]
END
GO

CREATE PROCEDURE [dbo].[pr_t_login_Select_M_TenfoldStaff]
	-- Add the parameters for the stored procedure here
	 @TenStaffCD varchar(10)    ,@TenStaffPW varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
   select * from  M_TenfoldStaff    where TenStaffCD = @TenStaffCD COLLATE SQL_Latin1_General_CP1_CS_AS 
   and TenStaffPW = @TenStaffPW COLLATE SQL_Latin1_General_CP1_CS_AS

END
