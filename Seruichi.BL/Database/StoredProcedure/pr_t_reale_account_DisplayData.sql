IF EXISTS(select * from sys.objects where name = 'pr_t_reale_account_DisplayData')
BEGIN
DROP PROCEDURE[pr_t_reale_account_DisplayData]
END
GO 


Create procedure [dbo].[pr_t_reale_account_DisplayData]

 @RealECD as varchar(10) 
	as 
	Begin
	
			--declare
			-- @RealECD as varchar(10)='RE04'

			select 
			TestFlg,
			PenaltyFlg,
			FORMAT (PenaltyStartDate, 'd', 'xh-ZA') PenaltyStartDate,
			FORMAT (PenaltyEndDate, 'd', 'xh-ZA')  PenaltyEndDate,
			Isnull(PenaltyMemo,'') as PenaltyMemo , 
			RealECD

			from M_realestate 
			where (@RealECD is null or (RealECD = @RealECD))
			and Deletedatetime is null

			select 
			FORMAT (p.PenaltyStartDate, 'd', 'xh-ZA') PenaltyStartDate,
			FORMAT (p.PenaltyEndDate, 'd', 'xh-ZA')  PenaltyEndDate,
			Isnull(p.PenaltyMemo,'') as PenaltyMemo ,
			Isnull(t.TenStaffName,'') as TenStaffName ,
			p.PenaltySEQ ,
		    (Case when p.PenaltyEndDate >= getdate() then 1 else 0 end ) ButtonEnable
			from L_REPenalty p
			left outer join M_TenfoldStaff t on t.TenstaffCD = p.InsertOperator
			where (@RealECD is null or (p.RealECD = @RealECD))
			and p.DeleteDateTime is  Null
			Order by p.InsertDateTime desc
			
			--select * from M_TenfoldStaff

			End