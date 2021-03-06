IF EXISTS (select * from sys.objects where name = 'pr_r_temp_mes_Update_M_REMessage')
BEGIN
    DROP PROCEDURE [pr_r_temp_mes_Update_M_REMessage]
END
GO

CREATE PROCEDURE [dbo].[pr_r_temp_mes_Update_M_REMessage]
     @RealECD    varchar(10)
    ,@MessageSEQ int
	,@MessageTitle varchar(50)
	,@MessageText  varchar(1000)
	,@Operator varchar(10)
	,@LoginName varchar(50)
	,@IPAddress varchar(20)
AS
BEGIN

DECLARE @SysDatetime datetime = GETDATE()
DECLARE @Remarks varchar(100)
set @Remarks = 'MessageSEQ:' + CAST(@MessageSEQ AS VARCHAR(10))

	UPDATE M_REMessage
		set MessageTitle = @MessageTitle					
		,MessageTEXT =@MessageText				
		,UpdateOperator=@Operator					
		,UpdateDateTime=@SysDatetime				
		,UpdateIPAddress=@IPAddress				
		Where RealECD=@RealECD					
        and MessageSEQ=@MessageSEQ
		
		EXEC pr_L_Log_Insert
			@LogDateTime   = @SysDatetime
			,@LoginKBN      = 2
			,@LoginID       = @Operator
			,@RealECD       = @RealECD
			,@LoginName     = @LoginName
			,@IPAddress     = @IPAddress
			,@PageID        = 'r_temp_mes'
			,@ProcessKBN    = 2
			,@Remarks       = @Remarks


END