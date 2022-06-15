IF EXISTS (select * from sys.objects where name = 'pr_r_template_DeleteData')
BEGIN
    DROP PROCEDURE [pr_r_template_DeleteData]
END
GO

Create Procedure [dbo].[pr_r_template_DeleteData]
					--declare 
					@RealECD as varchar(10),--='RE01',
					@TemplateNo as varchar(10),--=8,
					@TemplateKBN as tinyint--,-- = 0 
					as
					Begin
					if (@TemplateKBN =0) 
					Begin
					Delete from M_Template where RealECD =@RealECD and TemplateNo =@TemplateNo 
					Delete from M_TemplateBasic where RealECD =@RealECD and TemplateNo =@TemplateNo 
					Delete from M_TemplateRent where RealECD =@RealECD and TemplateNo =@TemplateNo 
					Delete from M_TemplateOpt where RealECD =@RealECD and TemplateNo =@TemplateNo 

					End

					else if @TemplateKBN =1
					Begin
					Delete from M_TemplateBasic where RealECD =@RealECD and TemplateNo =@TemplateNo  
					Delete from M_Template where RealECD =@RealECD and TemplateNo =@TemplateNo 
					
					End

						else if @TemplateKBN =2
					Begin
					Delete from M_TemplateRent where RealECD =@RealECD and TemplateNo =@TemplateNo  
					Delete from M_Template where RealECD =@RealECD and TemplateNo =@TemplateNo 
					
					End

						else if @TemplateKBN =3
					Begin
					Delete from M_TemplateOpt where RealECD =@RealECD and TemplateNo =@TemplateNo  
					Delete from M_Template where RealECD =@RealECD and TemplateNo =@TemplateNo 
					
					End
					End