IF EXISTS (select * from sys.objects where name = 'pr_t_reale_account_InsertUpdateData_new')
BEGIN
    DROP PROCEDURE [pr_t_reale_account_InsertUpdateData_new]
END
GO
Create  Procedure [dbo].[pr_t_reale_account_InsertUpdateData_new]
  @PFlg as tinyint ,--=1,
 @TFlg as tinyint,-- =1,
 @StartDate as date,
 @EndDate as date,
 @Memo as varchar(500),
 @RealECD as varchar(10),--='RE04',
 @TenStaffCD as varchar(10),
 @IP as varchar(50),--='::';
 @Isfake as tinyint,
 --@IsNew as tinyint,
 @SEQPenalty as int
 As

  declare @Date as datetime = getdate();
  --  SELECT 
	 --B.*
	 -- FROM M_RealEstate AS A 
	 -- Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
	 -- where
	 --     B.PenaltyStartDate <= @Date 
	 -- and B.PenaltyEndDate >= @Date
	 -- and B.DeleteDatetime is  not null 
	 -- order by B.PenaltySEQ desc



--M_RE  < Main/Sub
 Update	 M_RealEstate	
Set			PenaltyFLG						=	0,						
			PenaltyStartDate					=	null	,					
			PenaltyEndDate					=   null	,					
			PenaltyMemo						=	Null						
Where			RealECD						=	@RealECD	

if @SEQPenalty = -1


Begin
--LRE   <   Main
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
 End 
else 
Begin
--L_RE  <	Sub

--Update
  if @PFlg = 1 
  Begin
   --select * from L_REPenalty
	
	update L_REPenalty
	set 
	PenaltyStartDate =@StartDate ,
	PenaltyEndDate=@EndDate ,
	PenaltyMemo=@Memo,
	UpdateOperator = @TenStaffCD,
	UpdateDateTime = @Date,
	UpdateIPAddress = @IP

	        where PenaltySEQ =@SEQPenalty

   End
   else
   Begin
     
	 	update L_REPenalty
	set 
	PenaltyStartDate =@StartDate ,
	PenaltyEndDate=@EndDate ,
	PenaltyMemo=@Memo,
	UpdateOperator = @TenStaffCD,
	UpdateDateTime = @Date,
	UpdateIPAddress = @IP,
	DeleteOperator= @TenStaffCD,
	DeleteDateTime= @Date,
	DeleteIPAddress= @IP 
	        where PenaltySEQ =@SEQPenalty


   End 
 End   
--M_RE   < Main/Sub
 if exists(
 
  SELECT 
	top 1  B. PenaltyStartDate, B. PenaltyEndDate,B. PenaltyMemo
	  FROM M_RealEstate AS A 
	  Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
	  where
	      B.PenaltyStartDate <= @Date 
	  and B.PenaltyEndDate >= @Date
	  and B.DeleteDatetime is   null 
	  order by B.PenaltySEQ desc
 )
 Begin 
  update M_RealEstate set 

 PenaltyFLG = 1,


 PenaltyStartDate = (SELECT 
	top 1  B. PenaltyStartDate 
	  FROM M_RealEstate AS A 
	  Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
	  where
	      B.PenaltyStartDate <= @Date 
	  and B.PenaltyEndDate >= @Date
	  and B.DeleteDatetime is    null 
	  order by B.PenaltySEQ desc),


 PenaltyEndDate =(SELECT 
	top 1    B. PenaltyEndDate 
	  FROM M_RealEstate AS A 
	  Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
	  where
	      B.PenaltyStartDate <= @Date 
	  and B.PenaltyEndDate >= @Date
	  and B.DeleteDatetime is    null 
	  order by B.PenaltySEQ desc),


 PenaltyMemo =(SELECT 
	top 1   B. PenaltyMemo
	  FROM M_RealEstate AS A 
	  Left Outer join L_REPenalty AS B ON  B.RealECD= A.RealECD
	  where
	      B.PenaltyStartDate <= @Date 
	  and B.PenaltyEndDate >= @Date
	  and B.DeleteDatetime is    null 
	  order by B.PenaltySEQ desc),



 TestFLG =@TFlg, 
 UpdateOperator = @TenStaffCD,
 UpdateDateTime=@Date,
 UpdateIPAddress = @IP
 where RealECD = @RealECD-- and DeleteDateTime is null

 End
 else
 Begin
  update M_RealEstate set  

 TestFLG =@TFlg, 
 UpdateOperator = @TenStaffCD,
 UpdateDateTime=@Date,
 UpdateIPAddress = @IP
 where RealECD = @RealECD --and DeleteDateTime is null
 End




  




