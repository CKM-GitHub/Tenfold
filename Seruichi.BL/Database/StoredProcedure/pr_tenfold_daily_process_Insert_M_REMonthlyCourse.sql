IF EXISTS (select * from sys.objects where name = 'pr_tenfold_daily_process_Insert_M_REMonthlyCourse')
BEGIN
    DROP PROCEDURE [pr_tenfold_daily_process_Insert_M_REMonthlyCourse]
END
GO
CREATE PROCEDURE [dbo].[pr_tenfold_daily_process_Insert_M_REMonthlyCourse] 
	-- Add the parameters for the stored procedure here
	
	 @IPAddress as varchar(20),
	 @SystemDateTime datetime 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	---Table transfer specification A
		INSERT INTO M_REMonthlyCourse
			( 
			RealECD	,
			AppYYYYMM,
			CourseCD,
			InsertOperator,
			InsertDateTime,
			InsertIPAddress,
			UpdateOperator,
			UpdateDateTime,
			UpdateIPAddress,
			DeleteOperator,
			DeleteDateTime,
			DeleteIPAddress
		)
		Select 
		      A.RealECD,
			   FORMAT(@SystemDateTime,'yyyyMM'),
			   Case when A.NextCourseCD is not Null Then A.NextCourseCD
			   When A.NextCourseCD is  Null Then  A.CourseCD 
			   End As NextCourseCD,
			   Null,
			   @SystemDateTime,
			   @IPAddress,
			    Null,
			   @SystemDateTime,
			   @IPAddress,
			   Null,
			   Null,
			   Null
		From  M_RECourse A
		Inner Join M_RealEstate B on B.RealECD = A.RealECD
		where  A.DeleteDateTime is  Null
		AND    B.DeleteDateTime is Null 



		--Table transfer specification B
		UPDATE M_RECourse
		SET    CourseCD =A.CourseCD,
		       NextCourseCD = NUll,
			   UpdateOperator =NUll,
			   UpdateDateTime = @SystemDateTime,
			   UpdateIPAddress =@IPAddress
		From   M_RECourse A 
		Inner Join M_RealEstate B on B.RealECD=A.RealECD
		Where A.DeleteDateTime is Null 
		And   A.NextCourseCD is not Null 
		And   B.DeleteDateTime is Null 


END
