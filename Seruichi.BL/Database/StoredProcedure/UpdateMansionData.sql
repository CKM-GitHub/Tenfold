IF EXISTS (select * from sys.objects where name = 'UpdateMansionData')
BEGIN
    DROP PROCEDURE [UpdateMansionData]
END
GO
CREATE PROCEDURE [dbo].[UpdateMansionData] 
	-- Add the parameters for the stored procedure here
	@xml as xml
AS
BEGIN
	 DECLARE  @hQuantityAdjust AS INT 
	 CREATE TABLE #Temp
                (   
                    MansionCD                  varchar(10) COLLATE DATABASE_DEFAULT,
                    Longitude                 decimal(9,6),
                    Latitude                 decimal(9,6),
                    [Address]                varchar(50) COLLATE DATABASE_DEFAULT
                )
        EXEC sp_xml_preparedocument @hQuantityAdjust OUTPUT, @xml

        
        INSERT INTO #Temp
           (MansionCD,Longitude,Latitude,[Address])
             
                SELECT MansionCD,Longitude,Latitude,[Address]
                    FROM OPENXML(@hQuantityAdjust, 'RecordSet/Records')
                    WITH
                    (
                    MansionCD            varchar(10)  'MansionCD',
                    Longitude            decimal(9,6) 'Longitude',
                    Latitude             decimal(9,6) 'Latitude',
                    [Address]            varchar(50)  'Address'
                    )
        EXEC sp_xml_removedocument @hQuantityAdjust

		 BEGIN TRY
            BEGIN TRANSACTION;
            Update M_Mansion SET 
					  CityCD = (select CityCD from M_Address where M_Address.PrefCD = mm.PrefCD and M_Address.CityName= mm.CityName and M_Address.AddressLevel = 1),
					  TownCD = (select TownCD from M_Address where M_Address.PrefCD = mm.PrefCD and M_Address.CityName= mm.CityName and M_Address.TownName=mm.TownName and M_Address.AddressLevel = 2),
					  [Address]= t.[Address],
					  Longitude = t.Longitude,
					  Latitude=t.Latitude				  
				  from  #Temp t
				  inner join M_Mansion mm on mm.MansionCD=t.MansionCD
				  where t.MansionCD = mm.MansionCD
            IF @@TRANCOUNT > 0
                COMMIT;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK;
        END CATCH;
END
