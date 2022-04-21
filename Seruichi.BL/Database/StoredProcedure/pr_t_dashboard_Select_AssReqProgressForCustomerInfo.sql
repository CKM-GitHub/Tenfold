IF EXISTS (select * from sys.objects where name = 'pr_t_dashboard_Select_AssReqProgressForCustomerInfo')
BEGIN
    DROP PROCEDURE [pr_t_dashboard_Select_AssReqProgressForCustomerInfo]
END
GO
CREATE PROCEDURE [dbo].[pr_t_dashboard_Select_AssReqProgressForCustomerInfo]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select 	FORMAT(count(AssReqID),'N0')	as Customer_Information_Waiting_Count																										
	from D_AssReqProgress A																		
	left outer join M_SellerMansion B																											
	on	B.SellerMansionID =	A.SellerMansionID											
	Where (A.PurchReqDateTime is not Null and A.IntroDateTime is Null)
	and	A.DeleteDateTime is Null 															
	and	B.DeleteDateTime is Null	
END
