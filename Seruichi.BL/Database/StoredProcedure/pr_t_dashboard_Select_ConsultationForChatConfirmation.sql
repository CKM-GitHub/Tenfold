IF EXISTS (select * from sys.objects where name = 'pr_t_dashboard_Select_ConsultationForChatConfirmation')
BEGIN
    DROP PROCEDURE [pr_t_dashboard_Select_ConsultationForChatConfirmation]
END
GO
CREATE PROCEDURE [dbo].[pr_t_dashboard_Select_ConsultationForChatConfirmation]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FORMAT(count(ConsultID),'N0') as Chat_Confirmation_Waiting_Count																			
	from D_Consultation A													
	left outer join D_PurchReq B																						
	on	B.PurchReqID =	A.PurchReqID						
	left outer join M_SellerMansion C																						
	on	C.SellerMansionID =	B.SellerMansionID						
	Where A.DeleteDateTime is Null										
	and	A.ConsultDateTime is not Null										
	and	A.SolutionDateTime is Null										
	and	B.DeleteDateTime is Null										
	and	C.DeleteDateTime is Null	
END
