IF EXISTS (select * from sys.objects where name = 'pr_a_mypage_uinfo_Update_M_Seller')
BEGIN
    DROP PROCEDURE [pr_a_mypage_uinfo_Update_M_Seller]
END
GO

CREATE PROCEDURE [dbo].[pr_a_mypage_uinfo_Update_M_Seller]
(
    @SellerCD               varchar(10)
    ,@MailAddress           varchar(300)
    ,@Password              varchar(300)
    ,@SellerName            varchar(300)
    ,@SellerKana            varchar(300)
    ,@Birthday              varchar(100)
    ,@ZipCode1              varchar(3)
    ,@ZipCode2              varchar(4)
    ,@PrefName              varchar(10)
    ,@CityName              varchar(300)
    ,@TownName              varchar(300)
    ,@Address1              varchar(300)
    ,@Address2              varchar(300)
    ,@HandyPhone            varchar(100)
    ,@HousePhone            varchar(100)
    ,@Fax                   varchar(100)
    ,@PossibleTimes         int             = 0
    ,@LeaveDateTime         datetime        = NULL
    ,@TestFLG               tinyint         = 0
    ,@InvalidFLG            tinyint         = 0
    ,@InvalidDateTime       datetime        = NULL
    ,@Remark                varchar(1000)   = NULL
    ,@Operator              varchar(10)
    ,@IPAddress             varchar(20)
    ,@LoginName             varchar(50)
)
AS
BEGIN

    DECLARE @SysDatetime datetime = GETDATE()

    UPDATE M_Seller SET 
         MailAddress        = @MailAddress
        ,[Password]         = @Password
        ,SellerName         = @SellerName
        ,SellerKana         = @SellerKana
        ,Birthday           = @Birthday
        ,ZipCode1           = @ZipCode1
        ,ZipCode2           = @ZipCode2
        ,PrefName           = @PrefName
        ,CityName           = @CityName
        ,TownName           = @TownName
        ,Address1           = @Address1
        ,Address2           = @Address2
        ,HandyPhone         = @HandyPhone
        ,HousePhone         = @HousePhone
        ,Fax                = @Fax
        ,UpdateOperator     = @Operator
        ,UpdateDateTime     = @SysDatetime
        ,UpdateIPAddress    = @IPAddress
    WHERE SellerCD = @SellerCD

    EXEC pr_L_Seller_Insert
     @SellerCD      = @SellerCD

    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @SellerCD
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_mypage_uinfo'
    ,@ProcessKBN    = 2
    ,@Remarks       = @LoginName

END