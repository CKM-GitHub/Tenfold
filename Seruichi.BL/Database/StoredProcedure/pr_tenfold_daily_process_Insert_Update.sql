IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_Update')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_Update]
END
GO
CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_Update]
	-- Add the parameters for the stored procedure here
	@IPAddress varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @SystemDateTime datetime =GETDATE()
    -- Insert statements for procedure here

	   EXEC pr_tenfold_daily_process_Insert_M_REMonthlyCourse @IPAddress,@SystemDateTime

	   EXEC pr_tenfold_daily_process_Insert_M_REMoAreaPlan @IPAddress,@SystemDateTime

	   EXEC pr_tenfold_daily_process_Insert_M_REMoAreaPlanSec @IPAddress,@SystemDateTime

	   EXEC pr_tenfold_daily_process_Insert_M_REMoManPlan @IPAddress,@SystemDateTime

	   EXEC pr_tenfold_daily_process_Insert_M_REMoIntroAmount @IPAddress,@SystemDateTime
	   

	    UPDATE M_Monthly
		SET   MasterYYYYMM=(Select Format(@SystemDateTime,'yyyyMM')),
              MasterUpdateDateTime =@SystemDateTime
        WHERE  DataKey= 1




	 EXEC pr_L_Log_Insert
     @LogDateTime   = @SystemDateTime
    ,@LoginKBN      = 3
    ,@LoginID       = NULL
    ,@RealECD       = NULL
    ,@LoginName     = NULL
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'tenfold_daily_process'
    ,@ProcessKBN    = 2
    ,@Remarks       = '自動カウントアップ'
END
