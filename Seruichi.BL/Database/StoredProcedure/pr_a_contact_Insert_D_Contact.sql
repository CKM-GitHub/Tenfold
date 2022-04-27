IF EXISTS (select * from sys.objects where name = 'pr_a_contact_Insert_D_Contact')
BEGIN
    DROP PROCEDURE [pr_a_contact_Insert_D_Contact]
END
GO

CREATE PROCEDURE [dbo].[pr_a_contact_Insert_D_Contact]
(
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
    ,@IPAddress             varchar(20)
    ,@LoginName             varchar(50)
)
AS
BEGIN

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
        ,1
        ,@Operator
        ,NULL
        ,@ContactName
        ,@ContactKana
        ,@ContactAddress
        ,@ContactPhone
        ,@ContactType
        ,@ContactAssID
        ,@ContactSubject
        ,@ContactIssue
        ,@Operator
        ,@IPAddress
    )

    EXEC pr_L_Log_Insert
     @LogDateTime   = @ContactTime
    ,@LoginKBN      = 1
    ,@LoginID       = @Operator
    ,@RealECD       = NULL
    ,@LoginName     = @LoginName
    ,@IPAddress     = @IPAddress
    ,@PageID        = 'a_contact'
    ,@ProcessKBN    = 4
    ,@Remarks       = @LoginName

END