IF EXISTS (select * from sys.objects where name = 'pr_r_temp_mes_Insert_M_REMessage')
BEGIN
    DROP PROCEDURE [pr_r_temp_mes_Insert_M_REMessage]
END
GO

CREATE PROCEDURE [dbo].[pr_r_temp_mes_Insert_M_REMessage]
     @RealECD    varchar(10)
	,@MessageTitle varchar(50)
	,@MessageText  varchar(1000)
	,@Operator varchar(10)
	,@LoginName varchar(50)
	,@IPAddress varchar(20)
AS
BEGIN

DECLARE @SysDatetime datetime = GETDATE()
DECLARE @MessageSEQ int
set @MessageSEQ = (select Max(MessageSEQ)+1 FROM M_REMessage WHERE RealECD=@RealECD)
DECLARE @Remarks varchar(100)
set @Remarks = 'MessageSEQ:' + CAST(@MessageSEQ AS VARCHAR(10))

	INSERT INTO M_REMessage
			(
			 RealECD						
			,MessageSEQ						
			,MessageKBN						
			,MessageTitle						
			,MessageText						
			,ValidFLG						
			,InsertOperator						
			,InsertDateTime						
			,InsertIPAddress						
			,UpdateOperator						
			,UpdateDateTime						
			,UpdateIPAddress						
			,DeleteOperator						
			,DeleteDateTime						
			,DeleteIPAddress						
						
	
		)
		VALUES
		(
			 @RealECD
			,@MessageSEQ
			,1
			,@MessageTitle
			,@MessageText
			,0
	        ,@Operator
	        ,@SysDatetime
			,@IPAddress
			,null
			,null
			,null
			,null
			,null
			,null
		)
		
		EXEC pr_L_Log_Insert
			@LogDateTime   = @SysDatetime
			,@LoginKBN      = 2
			,@LoginID       = @Operator
			,@RealECD       = @RealECD
			,@LoginName     = @LoginName
			,@IPAddress     = @IPAddress
			,@PageID        = 'r_temp_mes'
			,@ProcessKBN    = 1
			,@Remarks       = @Remarks


END