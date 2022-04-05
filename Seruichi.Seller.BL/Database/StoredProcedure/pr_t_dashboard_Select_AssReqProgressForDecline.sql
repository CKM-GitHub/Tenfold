IF EXISTS (select * from sys.objects where name = 'pr_t_dashboard_Select_AssReqProgressForDecline')
BEGIN
    DROP PROCEDURE [pr_t_dashboard_Select_AssReqProgressForDecline]
END
GO
CREATE PROCEDURE [dbo].[pr_t_dashboard_Select_AssReqProgressForDecline] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FORMAT(count(AssReqID),'N0') as Decline_Cases_Count																										
	from D_AssReqProgress A																			
	left outer join M_SellerMansion B																												
	on	B.SellerMansionID =	A.SellerMansionID												
	Where A.EndStatus =	2 or A.EndStatus =3														
	and (A.SellerTermDateTime is not Null or A.BuyerTermDateTime is not	Null)	
	and A.DeleteDateTime is	Null																
	and B.DeleteDateTime is	Null	
END
