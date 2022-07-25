IF EXISTS (select * from sys.objects where name = 'pr_t_seller_config_InsertUpdateMultipurpose')
BEGIN
    DROP PROCEDURE [pr_t_seller_config_InsertUpdateMultipurpose]
END
GO

CREATE PROCEDURE [dbo].[pr_t_seller_config_InsertUpdateMultipurpose]
	-- Add the parameters for the stored procedure here
	@DataID as int,
	@Num1 as int,
	@Mode as int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

	If @Mode=1

		IF EXISTS (SELECT * FROM M_MultPurpose where DataID = @DataID and DataKey='1') 
			begin 
				Update M_MultPurpose
				Set Num1 = @Num1
				Where DataID = @DataID and DataKey = 1
			end
		Else
		   begin
			Insert Into M_MultPurpose
		   (DataID,
			DataKey,
			IDName,
			Char1,
			Char2,
			Char3,
			Char4,
			Char5,
			Num1,
			Num2,
			Num3,
			Num4,
			Num5,
			Date1,
			Date2,
			Date3)
		values (@DataID,
			1,
			Null,
			Null,
			Null,
			Null,
			Null,
			Null,
			@Num1,
			0,
			0,
			0,
			0,
			Null,
			Null,
			Null)
		   end

	else if @Mode=2

	   Update M_MultPurpose
	   Set Num1 = @Num1
	   Where DataID = @DataID and DataKey = 1
END
