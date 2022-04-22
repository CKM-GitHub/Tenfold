IF EXISTS (select * from sys.objects where name = 'pr_t_dashboard_Select_AssReqProgressForNewRequest')
BEGIN
    DROP PROCEDURE [pr_t_dashboard_Select_AssReqProgressForNewRequest]
END
GO
CREATE PROCEDURE [dbo].[pr_t_dashboard_Select_AssReqProgressForNewRequest] 
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FORMAT(count(AssReqID),'N0') as New_Request_Cases_Count																						
	from D_AssReqProgress A														
	left outer join M_SellerMansion B																							
	on	B.SellerMansionID =	A.SellerMansionID							
	Where A.DeepAssDateTime	 is not	Null											
	and A.IntroDateTime is Null											
	and A.DeleteDateTime is	Null											
	and B.DeleteDateTime is	Null	
END
