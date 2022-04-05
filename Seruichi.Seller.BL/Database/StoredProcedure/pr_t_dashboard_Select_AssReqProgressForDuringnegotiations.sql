IF EXISTS (select * from sys.objects where name = 'pr_t_dashboard_Select_AssReqProgressForDuringnegotiations')
BEGIN
    DROP PROCEDURE [pr_t_dashboard_Select_AssReqProgressForDuringnegotiations]
END
GO
CREATE PROCEDURE [dbo].[pr_t_dashboard_Select_AssReqProgressForDuringnegotiations]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FORMAT(count(AssReqID),'N0') as During_negotiations_Cases_Count																				
	from D_AssReqProgress A													
	left outer join M_SellerMansion B																						
	on	B.SellerMansionID =	A.SellerMansionID						
	Where A.IntroDateTime is not Null										
	and A.SellerTermDateTime is	Null										
	and A.BuyerTermDateTime is	Null										
	and A.DeleteDateTime is	Null										
	and B.DeleteDateTime is	Null	
END
