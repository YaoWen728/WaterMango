CREATE PROCEDURE [dbo].[usp_TapMangoPlant]
@action	NVARCHAR(200),
    @Id int = NULL OUTPUT,
	@Name nvarchar(MAX) = '',
	@Uri nvarchar(max) = '',
	@CreatedOn datetime = null

AS
BEGIN
	IF lower(@action) = 'delete'
	BEGIN
		--Delete a single record based on PK
		DELETE FROM [dbo].[TapMangoPlant]
		WHERE
			[Id] = @Id
	END
	
	IF lower(@action) = 'insert'
	BEGIN
		--Insert a new record
		INSERT INTO [dbo].[TapMangoPlant] (
		   [Name]
           ,[Uri]
           ,[CreatedOn])
		 VALUES (
		   @Name
           ,@Uri
           ,@CreatedOn
        )
		
		SELECT @Id = SCOPE_IDENTITY()
	END

	IF lower(@action) = 'update'
	BEGIN
		UPDATE [dbo].[TapMangoPlant]
		   SET [Name] = @Name
			  ,[Uri] = @Uri
			  ,[CreatedOn] = @CreatedOn
		 WHERE @Id IS NULL OR [Id] = @Id
	END

	IF lower(@action) = 'select'
		BEGIN
        	SELECT [Id]
		   ,[Name]
		   ,[Uri]
           ,[CreatedOn]
			  FROM [dbo].[TapMangoPlant]
      END

	IF lower(@action) = 'getbyid'
		BEGIN
        	SELECT [Id]
		   ,[Name]
		   ,[Uri]
           ,[CreatedOn]
			  FROM [dbo].[TapMangoPlant]
			  WHERE [Id] = @Id
      END
END

