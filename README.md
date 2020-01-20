# WaterMango
 
 Tested On Visual Studio 2019 and MSQL v17.9.1
 
To Setup
1. Replace {INSERT_DATASOURCE} in Data Source in WebConfig. This line can found under line 8 of the WebConfig 

```
<add name="TapMangoDatabase" connectionString="Data Source={INSERT_DATASOURCE};Initial Catalog=TapMangoDatabase;Integrated Security=True;" providerName="System.Data.SqlClient" />
```

2. Create Database with TapMango_CreateDatabase.sql 
3. Create Table with TapMango_CreateTable.sql
4. Create stored Procedure with usp_TapMangoPlant and usp_WateringSession
5. Run project through Visual Studio