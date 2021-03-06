IF EXISTS (select * from sys.objects where name = 'pr_r_contact_Insert_D_Contact')
BEGIN
    DROP PROCEDURE [pr_r_contact_Insert_D_Contact]
END
GO

CREATE PROCEDURE [dbo].[pr_r_contact_Insert_D_Contact]
	@ContactTime            datetime
    ,@ContactName           varchar(300)
    ,@ContactKana           varchar(300)
    ,@ContactAddress        varchar(300)
    ,@ContactPhone          varchar(100)
    ,@ContactType           varchar(50)
    ,@ContactAssID          varchar(10)
    ,@ContactSubject        varchar(50)
    ,@ContactIssue          varchar(1000)
    ,@Operator              varchar(10)
	,@RealECD				varchar(10)
    ,@IPAddress             varchar(20)
    ,@LoginName             varchar(50)
AS
BEGIN
  Declare @SellerCD	varchar(10)  = NULL
  set  @SellerCD = (select SellerCD from D_AssReq where  D_AssReq.AssReqID = @ContactAssID)

	 INSERT INTO D_Contact
    (
        ContactTime
        ,LoginKBN
        ,LoginID
        ,RealECD
        ,ContactName
        ,ContactKana
        ,ContactAddress
        ,ContactPhone
        ,ContactType
        ,ContactAssID
        ,ContactSubject
        ,ContactIssue
        ,SellerCD
        ,InsertIPAddress
    ) VALUES (
        @ContactTime
        ,2
        ,@Operator
        ,@RealECD
        ,@ContactName
        ,@ContactKana
        ,@ContactAddress
        ,@ContactPhone
        ,@ContactType
        ,@ContactAssID
        ,@ContactSubject
        ,@ContactIssue
        ,@SellerCD
        ,@IPAddress
    )

    EXEC pr_L_Log_Insert
     @LogDateTime   = @ContactTime
    ,@LoginKBN      = 2
    ,@LoginID       = @Operator
    ,@RealECD       = @RealECD
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'r_contact'
    ,@ProcessKBN    = 4
    ,@Remarks       = @ContactSubject
END
