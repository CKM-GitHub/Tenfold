IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Update_M_RealEstate')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Update_M_RealEstate]
END
GO

CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Update_M_RealEstate]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @SystemDateTime datetime =GETDATE()
    -- Insert statements for procedure here
	Update	M_RealEstate
	SET		PenaltyFLG=0,
			PenaltyMemo= Null,
			PenaltyStartDate= Null,
			PenaltyEndDate =Null

	Update	M_RealEstate  
	  SET   PenaltyFLG =(Select Case When C.RealECD = D.RealECD Then 1 Else 0 End  ),
	        PenaltyMemo= C.PenaltyMemo,
			PenaltyStartDate =C.PenaltyStartDate,
			PenaltyEndDate =C.PenaltyEndDate,
			UpdateOperator =Null,
			UpdateDateTime = @SystemDateTime,
			UpdateIPAddress= @IPAddress
	FROM M_RealEstate As D
	Left outer join L_REPenalty AS C on D.RealECD=C.RealECD 
								and C.PenaltySEQ= (SELECT 
												  MAX(B.PenaltySEQ)
												  FROM M_RealEstate AS A 
												  Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
												  and	B.PenaltyStartDate <= @SystemDateTime 
												  and   B.PenaltyEndDate >= @SystemDateTime
												  and   B.DeleteDateTime is  null 
												) 
	AND D.RealECD is not Null  

	   EXEC pr_L_Log_Insert
     @LogDateTime   = @SystemDateTime
    ,@LoginKBN      = 3
    ,@LoginID       = NULL
    ,@RealECD       = NULL
    ,@LoginName     = NULL
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'tenfold_daily_process'
    ,@ProcessKBN    = 2
    ,@Remarks       = 'ペナルティーの更新'
END