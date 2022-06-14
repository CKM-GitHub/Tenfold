IF EXISTS(select * from sys.objects where name = 'pr_t_reale_account_InsertUpdateData')
BEGIN
DROP PROCEDURE[pr_t_reale_account_InsertUpdateData]
END
GO 
 Create Procedure [dbo].[pr_t_reale_account_InsertUpdateData]
  @PFlg as tinyint ,--=1,
 @TFlg as tinyint,-- =1,
 @StartDate as date,
 @EndDate as date,
 @Memo as varchar(500),
 @RealECD as varchar(10),--='RE04',
 @TenStaffCD as varchar(10),
 @IP as varchar(50),--='::';
 @Isfake as tinyint
 As

 --declare
 --@PFlg as tinyint =1,
 --@TFlg as tinyint =1,
 --@StartDate as date,
 --@EndDate as date,
 --@Memo as varchar(500),
 --@RealECD as varchar(10)='RE04',
 --@TenStaffCD as varchar(10),
 --@IP as varchar(50)='::';
 


 declare @Date as datetime = getdate();
 update M_RealEstate set 
 PenaltyFLG = @PFlg,
 TestFLG =@TFlg,
 PenaltyStartDate = @StartDate,
 PenaltyEndDate =@EndDate,
 PenaltyMemo = @Memo ,
 UpdateOperator = @TenStaffCD,
 UpdateDateTime=@Date,
 UpdateIPAddress = @IP
 where RealECD = @RealECD and DeleteDateTime is null

 if @PFlg=1 and @Isfake=1
 Begin
 insert L_REPenalty (
 RealECD,PenaltyStartDate,PenaltyEndDate,PenaltyMemo,InsertOperator,InsertDateTime,InsertIPAddress,UpdateOperator,UpdateDateTime,UpdateIPAddress
 )
 values (
 @RealECD,
 @StartDate,
 @EndDate,
 @Memo,
 @TenStaffCD,
 @Date,
 @IP, 
 @TenStaffCD,
 @Date,
 @IP

 )

 End
