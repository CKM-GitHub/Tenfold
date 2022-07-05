IF EXISTS (select * from sys.objects where name = 'pr_r_template_Select_DisplayData')
BEGIN
    DROP PROCEDURE [pr_r_template_Select_DisplayData]
END
GO

Create procedure [dbo].[pr_r_template_Select_DisplayData] 
					 @RealECD as varchar(10)
					 as 
					 Begin
					 					
					SELECT																											
					 TemplateNo																											
					,CreatePage																											
					,CASE WHEN CreatePage = 1 THEN 'エリア'																											
					         WHEN CreatePage = 2 THEN '路線'																											
					         WHEN CreatePage = 3 THEN 'マンション'																											
					         ELSE ''																											
					 END AS 作成ページ区分																											
					,TemplateName																											
					, (	select convert(varchar, ISNULL(UpdateDateTime,InsertDateTime), 111) )    As LstEdtDt																										
					  FROM   M_Template																											
					WHERE   TemplateKBN = 0 and  RealECD = @RealECD --or TemplateKBN = 1 or TemplateKBN = 2 or TemplateKBN = 3   																
					 
					Order By　TemplateNo			
									
					SELECT																											
					 TemplateNo																											
					,CreatePage																											
					,CASE WHEN CreatePage = 1 THEN 'エリア'																											
					         WHEN CreatePage = 2 THEN '路線'																											
					         WHEN CreatePage = 3 THEN 'マンション'																											
					         ELSE ''																											
					 END AS 作成ページ区分																											
					,TemplateName																											
						, (	select convert(varchar, ISNULL(UpdateDateTime,InsertDateTime), 111) )    As LstEdtDt																											
					  FROM   M_Template																											
					WHERE   TemplateKBN = 1 and  RealECD = @RealECD --or TemplateKBN = 1 or TemplateKBN = 2 or TemplateKBN = 3   																
					 
					Order By　TemplateNo						
					SELECT																											
					 TemplateNo																											
					,CreatePage																											
					,CASE WHEN CreatePage = 1 THEN 'エリア'																											
					         WHEN CreatePage = 2 THEN '路線'																											
					         WHEN CreatePage = 3 THEN 'マンション'																											
					         ELSE ''																											
					 END AS 作成ページ区分																											
					,TemplateName																											
					, (	select convert(varchar, ISNULL(UpdateDateTime,InsertDateTime), 111) )    As LstEdtDt																												
					  FROM   M_Template																											
					WHERE   TemplateKBN = 2 and  RealECD = @RealECD--or TemplateKBN = 1 or TemplateKBN = 2 or TemplateKBN = 3   																
					 
					Order By　TemplateNo						
					SELECT	 																							
					 TemplateNo																											
					,CreatePage																											
					,CASE WHEN CreatePage = 1 THEN 'エリア'																											
					         WHEN CreatePage = 2 THEN '路線'																											
					         WHEN CreatePage = 3 THEN 'マンション'																											
					         ELSE ''																											
					 END AS 作成ページ区分																											
					,TemplateName																											
						, (	select convert(varchar, ISNULL(UpdateDateTime,InsertDateTime), 111) )    As LstEdtDt																											
					  FROM   M_Template																											
					WHERE   TemplateKBN = 3 and  RealECD = @RealECD--or TemplateKBN = 1 or TemplateKBN = 2 or TemplateKBN = 3   																
					 
					Order By　TemplateNo		
					End