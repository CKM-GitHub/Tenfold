IF EXISTS (select * from sys.objects where name = 'pr_r_temp_mes_Select_M_REMessage_By_RealECD')
BEGIN
    DROP PROCEDURE [pr_r_temp_mes_Select_M_REMessage_By_RealECD]
END
GO

CREATE PROCEDURE [dbo].[pr_r_temp_mes_Select_M_REMessage_By_RealECD]
	-- Add the parameters for the stored procedure here
	@RealECD varchar(10)
AS
BEGIN

		SELECT											
		RealECD											
		,ROW_NUMBER() OVER(ORDER BY RealECD,MessageSEQ　ASC) AS SEQ											
		,MessageSEQ											
		,MessageTitle											
		,MessageTEXT											
		FROM M_REMessage											
		WHERE RealECD  = @RealECD										
		AND MessageKBN =1           　								
		Order By MessageSEQ
END