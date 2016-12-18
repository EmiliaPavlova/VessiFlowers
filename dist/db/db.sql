use master
go

-- Configurations for database
declare @dbName varchar(255) = 'iliankostov_shared' -- change database name according to project name

declare @mdfDir varchar(255) = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\'
declare @mdfInitialSize varchar(255) = '10MB'
declare @mdfMaxSize varchar(255) = '1000MB'
declare @mdfFileGrowth varchar(255) = '10MB'

declare @ldfDir varchar(255) = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\'
declare @ldfInitialSize varchar(255) = '10MB'
declare @ldfMaxSize varchar(255) = '500MB'
declare @ldfFileGrowth varchar(255) = '10%'

-- Create database
if not exists (select name from master.dbo.sysdatabases where name = @dbName)
	begin
		declare @query varchar(max) = '
		create database ' + @dbName + '
		on 
		( name = ' + @dbName + 'Data,
			filename = "' + @mdfDir + lower(@dbName) + '.mdf",
			size = ' + @mdfInitialSize + ',
			maxsize = ' + @mdfMaxSize + ',
			filegrowth = '+ @mdfFileGrowth +' )
		log on
		( name = ' + @dbName + 'Log,
			filename = "' + @ldfDir + lower(@dbName) + '.ldf",
			size = ' + @ldfInitialSize + ',
			maxsize = ' + @ldfMaxSize + ',
			filegrowth = ' + @ldfFileGrowth + ' )
		'
		exec(@query)
	end
go

use iliankostov_shared
go

/****** Object:  Table [dbo].[AspNetRoles]   Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'AspNetRoles')
	begin
		create table [dbo].[AspNetRoles](
			[Id] [nvarchar](128) not null,
			[Name] [nvarchar](256) not null,
			constraint [PK_dbo.AspNetRoles] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'AspNetUserClaims')
	begin
		create table [dbo].[AspNetUserClaims](
			[Id] [int] identity(1,1) not null,
			[UserId] [nvarchar](128) not null,
			[ClaimType] [nvarchar](max) null,
			[ClaimValue] [nvarchar](max) null,
			constraint [PK_dbo.AspNetUserClaims] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'AspNetUserLogins')
	begin
		create table [dbo].[AspNetUserLogins](
			[LoginProvider] [nvarchar](128) not null,
			[ProviderKey] [nvarchar](128) not null,
			[UserId] [nvarchar](128) not null,
			constraint [PK_dbo.AspNetUserLogins] primary key clustered 
			(
				[LoginProvider] asc,
				[ProviderKey] asc,
				[UserId] asc
			)
		)
	end
go

/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'AspNetUserRoles')
	begin
		create table [dbo].[AspNetUserRoles](
			[UserId] [nvarchar](128) not null,
			[RoleId] [nvarchar](128) not null,
			constraint [PK_dbo.AspNetUserRoles] primary key clustered 
			(
				[UserId] asc,
				[RoleId] asc
			)
		)
	end
go

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'AspNetUsers')
	begin
		create table [dbo].[AspNetUsers](
			[Id] [nvarchar](128) not null,
			[Email] [nvarchar](256) null,
			[EmailConfirmed] [bit] not null,
			[PasswordHash] [nvarchar](max) null,
			[SecurityStamp] [nvarchar](max) null,
			[PhoneNumber] [nvarchar](max) null,
			[PhoneNumberConfirmed] [bit] not null,
			[TwoFactorEnabled] [bit] not null,
			[LockoutEndDateUtc] [datetime] null,
			[LockoutEnabled] [bit] not null,
			[AccessFailedCount] [int] not null,
			[UserName] [nvarchar](256) not null,
			constraint [PK_dbo.AspNetUsers] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Index [RoleNameIndex]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='RoleNameIndex' and object_id = object_id('AspNetRoles'))
	create unique nonclustered index [RoleNameIndex] on [dbo].[AspNetRoles] ([Name] asc)
go

/****** Object:  Index [IX_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='IX_UserId' and object_id = object_id('AspNetUserClaims'))
	create nonclustered index [IX_UserId] on [dbo].[AspNetUserClaims] ([UserId] asc)
go

/****** Object:  Index [IX_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='IX_UserId' and object_id = object_id('AspNetUserLogins'))
	create nonclustered index [IX_UserId] on [dbo].[AspNetUserLogins] ([UserId] asc)
go

/****** Object:  Index [IX_RoleId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='IX_RoleId' and object_id = object_id('AspNetUserRoles'))
	create nonclustered index [IX_RoleId] on [dbo].[AspNetUserRoles] ([RoleId] asc)
go

/****** Object:  Index [IX_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='IX_UserId' and object_id = object_id('AspNetUserRoles'))
	create nonclustered index [IX_UserId] on [dbo].[AspNetUserRoles] ([UserId] asc)
go

/****** Object:  Index [UserNameIndex]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.indexes where name='UserNameIndex' and object_id = object_id('AspNetUsers'))
	create unique nonclustered index [UserNameIndex] on [dbo].[AspNetUsers] ([UserName] asc)
go

/****** Object:  Index [FK_AspNetUserClaims_AspNetUsers_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_AspNetUserClaims_AspNetUsers_UserId')
	begin
		alter table [dbo].[AspNetUserClaims]  with check add  constraint [FK_AspNetUserClaims_AspNetUsers_UserId] foreign key([UserId])
		references [dbo].[AspNetUsers] ([Id])
		on delete cascade

		alter table [dbo].[AspNetUserClaims] check constraint [FK_AspNetUserClaims_AspNetUsers_UserId]
	end
go

/****** Object:  Index [FK_AspNetUserLogins_AspNetUsers_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_AspNetUserLogins_AspNetUsers_UserId')
	begin
		alter table [dbo].[AspNetUserLogins]  with check add  constraint [FK_AspNetUserLogins_AspNetUsers_UserId] foreign key([UserId])
		references [dbo].[AspNetUsers] ([Id])
		on delete cascade

		alter table [dbo].[AspNetUserLogins] check constraint [FK_AspNetUserLogins_AspNetUsers_UserId]
	end
go

/****** Object:  Index [FK_AspNetUserRoles_AspNetRoles_RoleId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_AspNetUserRoles_AspNetRoles_RoleId')
	begin
		alter table [dbo].[AspNetUserRoles]  with check add  constraint [FK_AspNetUserRoles_AspNetRoles_RoleId] foreign key([RoleId])
		references [dbo].[AspNetRoles] ([Id])
		on delete cascade
		
		alter table [dbo].[AspNetUserRoles] check constraint [FK_AspNetUserRoles_AspNetRoles_RoleId]
	end
go

/****** Object:  Index [FK_AspNetUserRoles_AspNetUsers_UserId]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_AspNetUserRoles_AspNetUsers_UserId')
	begin
		alter table [dbo].[AspNetUserRoles]  with check add  constraint [FK_AspNetUserRoles_AspNetUsers_UserId] foreign key([UserId])
		references [dbo].[AspNetUsers] ([Id])
		on delete cascade

		alter table [dbo].[AspNetUserRoles] check constraint [FK_AspNetUserRoles_AspNetUsers_UserId]
	end
go

/****** Object:  Table [dbo].[Users]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'Users')
	begin
		create table [dbo].[Users](
			[Id] [nvarchar](128) NOT NULL,
			[FirstName] [nvarchar](128) NOT NULL,
			[LastName] [nvarchar](128) NOT NULL,
			constraint [PK_Users] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [dbo].[FK_Users_AspNetUsers]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_Users_AspNetUsers')
	begin
		alter table [dbo].[Users]  with check add  constraint [FK_Users_AspNetUsers] foreign key([Id])
		references [dbo].[AspNetUsers] ([Id])
	
		alter table [dbo].[Users] check constraint [fk_Users_AspNetUsers]
	end
go

/****** Object:  schema [vf]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from sys.schemas where name = 'vf')
	exec('create schema vf')
go

/****** Object:  Table [vf].[Pages]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'Pages')
	begin
		create table [vf].[Pages](
			[Id] [int] identity,
			[Keyword] [nvarchar](128) not null,
			[Title] [nvarchar](128) not null,
			[Content] [nvarchar](max) not null,
			constraint [PK_Pages] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [vf].[GalleryType]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'GalleryType')
	begin
		create table [vf].[GalleryType](
			[Id] [int] identity,
			[Keyword] [nvarchar](128) not null,
			[Name] [nvarchar](128) not null,
			constraint [PK_GalleryType] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [vf].[Gallery]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'Gallery')
	begin
		create table [vf].[Gallery](
			[Id] [int] identity,
			[GalleryTypeId] [int] not null,
			[Title] [nvarchar](128) not null,
			[Position] [int] not null,
			constraint [PK_Gallery] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [vf].[FK_Gallery_GalleryType]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_Gallery_GalleryType')
	begin
		alter table [vf].[Gallery]  with check add constraint [FK_Gallery_GalleryType] foreign key([GalleryTypeId])
		references [vf].[GalleryType] ([Id])
		on delete cascade

		alter table [vf].[Gallery] check constraint [FK_Gallery_GalleryType]
	end
go

/****** Object:  Table [vf].[Medias]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select * from information_schema.tables where table_name = 'Medias')
	begin
		create table [vf].[Medias](
			[Id] [int] identity,
			[GalleryId] [int] not null,
			[Url] [nvarchar](128) not null,
			[DataSize] [nvarchar](16) not null,
			constraint [PK_Medias] primary key clustered ([Id] asc)
		)
	end
go

/****** Object:  Table [vf].[FK_Medias_GalleryType]    Script Date: 01-Dec-16 11:24:48 ******/
if not exists (select name from sys.foreign_keys where name='FK_Medias_GalleryType')
	begin
		alter table [vf].[Medias]  with check add constraint [FK_Medias_GalleryType] foreign key([GalleryId])
		references [vf].[Gallery] ([Id])
		on delete cascade

		alter table [vf].[Medias] check constraint [FK_Medias_GalleryType]
	end
go


