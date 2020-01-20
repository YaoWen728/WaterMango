CREATE TABLE [dbo].[TapMangoPlant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [Nvarchar](max) NOT NULL,
	[Uri] [Nvarchar](max) NOT NULL,
	[CreatedOn] [DateTime] NOT NULL
 CONSTRAINT [PK_TapMangoPlant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

  INSERT [dbo].[TapMangoPlant] (Name,Uri,CreatedOn) VALUES('Cactus','/ItemImages/Cactus.jpg','2020-1-18 18:00:00.000')
  INSERT [dbo].[TapMangoPlant] (Name,Uri,CreatedOn) VALUES('Hibiscus Brilliant','/ItemImages/Hibiscus_Brilliant.jpg','2020-1-18 18:00:00.000')
  INSERT [dbo].[TapMangoPlant] (Name,Uri,CreatedOn) VALUES('Mango','/ItemImages/Mango.jpg','2020-1-18 18:00:00.000')
  INSERT [dbo].[TapMangoPlant] (Name,Uri,CreatedOn) VALUES('Red Rose','/ItemImages/Red_Rose.jpg','2020-1-18 18:00:00.000')
  INSERT [dbo].[TapMangoPlant] (Name,Uri,CreatedOn) VALUES('Sakura','/ItemImages/Sakura.jpg','2020-1-18 18:00:00.000')
  
  
CREATE TABLE [dbo].[WateringSession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TapMangoPlantId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StartTime] [DateTime] NOT NULL,
	[ExpectToCompleteOn] [DateTime] NOT NULL,
	[CreatedOn] [DateTime] NOT NULL
 CONSTRAINT [PK_WateringSession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[WateringSession]  WITH CHECK ADD  CONSTRAINT [FK_TapMangoPlant_WateringSession] FOREIGN KEY([TapMangoPlantId])
REFERENCES [dbo].[TapMangoPlant] ([Id])
ON DELETE CASCADE
