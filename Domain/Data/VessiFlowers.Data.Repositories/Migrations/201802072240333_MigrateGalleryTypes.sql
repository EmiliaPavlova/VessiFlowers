BEGIN TRANSACTION
SET IDENTITY_INSERT [vf].[GalleryType] ON
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (1, N'flowers', N'Цветя')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (3, N'cake', N'Торта от хартия')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (4, N'beauty', N'Малки красоти')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (5, N'wedding', N'За сватба')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (6, N'spring', N'Пролет')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (7, N'easter', N'За Великден')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (8, N'autumn', N'Есен')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (9, N'christmas', N'За Коледа')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (10, N'crepe', N'Цветя от креп хартия')
INSERT INTO [vf].[GalleryType] ([Id], [Keyword], [Name]) VALUES (11, N'diy', N'Аранжирай сам')
SET IDENTITY_INSERT [vf].[GalleryType] OFF
COMMIT TRANSACTION