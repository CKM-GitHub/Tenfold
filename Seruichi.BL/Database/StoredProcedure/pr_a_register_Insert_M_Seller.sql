IF EXISTS (select * from sys.objects where name = 'pr_a_register_Insert_M_Seller')
BEGIN
    DROP PROCEDURE [pr_a_register_Insert_M_Seller]
END
GO

CREATE PROCEDURE [dbo].[pr_a_register_Insert_M_Seller]
(
    @SellerCD               varchar(10)
    ,@MailAddress           varchar(300)
    ,@Password              varchar(300)
    ,@SellerName            varchar(300)
    ,@SellerKana            varchar(300)
    ,@Birthday              varchar(100)
    ,@ZipCode1              varchar(3)
    ,@ZipCode2              varchar(4)
    ,@PrefCD                varchar(2)
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

    EXEC pr_M_Counter_GetNewID
        @CounterKey = 2, 
        @NewID = @SellerCD OUTPUT

    SELECT @PossibleTimes = Num1 
        FROM M_MultPurpose 
        WHERE DataID = 112 AND DataKey = 1


    INSERT INTO M_Seller
    (
        SellerCD
        ,MailAddress
        ,[Password]
        ,SellerName
        ,SellerKana
        ,Birthday
        ,ZipCode1
        ,ZipCode2
        ,PrefCD
        ,PrefName
        ,CityName
        ,TownName
        ,Address1
        ,Address2
        ,HandyPhone
        ,HousePhone
        ,Fax
        ,PossibleTimes
        ,LeaveDateTime
        ,TestFLG
        ,InvalidFLG
        ,InvalidDateTime
        ,Remark
        ,InsertOperator
        ,InsertDateTime
        ,InsertIPAddress
        ,UpdateOperator
        ,UpdateDateTime
        ,UpdateIPAddress
        ,DeleteOperator
        ,DeleteDateTime
        ,DeleteIPAddress
    ) VALUES (
        @SellerCD
        ,@MailAddress
        ,@Password
        ,@SellerName
        ,@SellerKana
        ,@Birthday
        ,@ZipCode1
        ,@ZipCode2
        ,@PrefCD
        ,@PrefName
        ,@CityName
        ,@TownName
        ,@Address1
        ,@Address2
        ,@HandyPhone
        ,@HousePhone
        ,@Fax
        ,@PossibleTimes
        ,@LeaveDateTime
        ,@TestFLG
        ,@InvalidFLG
        ,@InvalidDateTime
        ,@Remark
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,@Operator
        ,@SysDatetime
        ,@IPAddress
        ,NULL
        ,NULL
        ,NULL
    )

    EXEC pr_L_Seller_Insert
     @SellerCD      = @SellerCD

    EXEC pr_L_Log_Insert
     @LogDateTime   = @SysDatetime
    ,@LoginKBN      = 1
    ,@LoginID       = @SellerCD
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_register'
    ,@ProcessKBN    = 1
    ,@Remarks       = @LoginName

END