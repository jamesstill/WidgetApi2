﻿CREATE TABLE [dbo].[Widget]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(50) NULL, 
    [Shape] NVARCHAR(50) NULL,
	CONSTRAINT [PK_Widget] PRIMARY KEY NONCLUSTERED ([Id])
)