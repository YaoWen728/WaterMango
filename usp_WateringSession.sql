CREATE PROCEDURE [dbo].[usp_WateringSession]
@action	NVARCHAR(200),
    @Id int = NULL OUTPUT,
	@TapMangoPlantId int = NULL,
	@StatusId int = NULL,
	@StartTime datetime = null,
	@ExpectToCompleteOn datetime = null,
	@CreatedOn datetime = null

AS
BEGIN
	IF lower(@action) = 'delete'
	BEGIN
		--Delete a single record based on PK
		DELETE FROM [dbo].[WateringSession]
		WHERE
			[Id] = @Id
	END
	
	IF lower(@action) = 'insert'
	BEGIN
		--Insert a new record
		INSERT INTO [dbo].[WateringSession] (
		   [TapMangoPlantId]
		   ,[StatusId]
		   ,[StartTime]
           ,[ExpectToCompleteOn]
           ,[CreatedOn])
		 VALUES (
		   @TapMangoPlantId
		   ,@StatusId
           ,@StartTime
		   ,@ExpectToCompleteOn
           ,@CreatedOn
        )
		
		SELECT @Id = SCOPE_IDENTITY()
	END

	IF lower(@action) = 'update'
	BEGIN
		UPDATE [dbo].[WateringSession]
		   SET [StatusId]  = @StatusId
			  ,[StartTime] = @StartTime
			  ,[ExpectToCompleteOn] = @ExpectToCompleteOn
		 WHERE [Id] = @Id
	END

	IF lower(@action) = 'select'
		BEGIN
        	SELECT [Id]
		   ,[TapMangoPlantId]
		   ,[StatusId]
		   ,[StartTime]
		   ,[ExpectToCompleteOn]
           ,[CreatedOn]
			  FROM [dbo].[WateringSession]
      END

	IF lower(@action) = 'getbytapmangoplantid'
		BEGIN
        	SELECT [Id]
		   ,[TapMangoPlantId]
		   ,[StatusId]
		   ,[StartTime]
		   ,[ExpectToCompleteOn]
           ,[CreatedOn]
			  FROM [dbo].[WateringSession]
			  WHERE [TapMangoPlantId] = @TapMangoPlantId
			  ORDER BY CreatedOn DESC
      END
END

