USE [master]
GO
/****** Object:  Database [EDDB]    Script Date: 10/10/2018 19:57:23 ******/
CREATE DATABASE [EDDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EDDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EDDB.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EDDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EDDB_log.ldf' , SIZE = 1072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EDDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EDDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EDDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EDDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EDDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EDDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EDDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EDDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EDDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EDDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EDDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EDDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EDDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EDDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EDDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EDDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EDDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EDDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EDDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EDDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EDDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EDDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EDDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [EDDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EDDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EDDB] SET  MULTI_USER 
GO
ALTER DATABASE [EDDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EDDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EDDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EDDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EDDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [EDDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CommentEntity]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommentEntity](
	[UserName] [nvarchar](450) NULL,
	[TimeStamp] [datetime2](7) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[EncounterEntityId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CommentEntity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Encounters]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encounters](
	[Id] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[SportName] [nvarchar](450) NULL,
	[HomeTeamName] [nvarchar](450) NULL,
	[HomeTeamSportName] [nvarchar](450) NULL,
	[AwayTeamName] [nvarchar](450) NULL,
	[AwayTeamSportName] [nvarchar](450) NULL,
 CONSTRAINT [PK_Encounters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sports]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sports](
	[SportName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Sports] PRIMARY KEY CLUSTERED 
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Teams](
	[Name] [nvarchar](450) NOT NULL,
	[SportName] [nvarchar](450) NOT NULL,
	[Logo] [varbinary](max) NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeamUsers]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamUsers](
	[TeamName] [nvarchar](450) NOT NULL,
	[SportName] [nvarchar](450) NULL,
	[UserName] [nvarchar](450) NULL,
	[UserNamee] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_TeamUsers] PRIMARY KEY CLUSTERED 
(
	[TeamName] ASC,
	[UserNamee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/10/2018 19:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Surname] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Mail] [nvarchar](max) NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_CommentEntity_EncounterEntityId]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_CommentEntity_EncounterEntityId] ON [dbo].[CommentEntity]
(
	[EncounterEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_CommentEntity_UserName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_CommentEntity_UserName] ON [dbo].[CommentEntity]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Encounters_AwayTeamName_AwayTeamSportName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_Encounters_AwayTeamName_AwayTeamSportName] ON [dbo].[Encounters]
(
	[AwayTeamName] ASC,
	[AwayTeamSportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Encounters_HomeTeamName_HomeTeamSportName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_Encounters_HomeTeamName_HomeTeamSportName] ON [dbo].[Encounters]
(
	[HomeTeamName] ASC,
	[HomeTeamSportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Encounters_SportName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_Encounters_SportName] ON [dbo].[Encounters]
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Teams_SportName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_Teams_SportName] ON [dbo].[Teams]
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TeamUsers_TeamName_SportName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TeamUsers_TeamName_SportName] ON [dbo].[TeamUsers]
(
	[TeamName] ASC,
	[SportName] ASC
)
WHERE ([SportName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TeamUsers_UserName]    Script Date: 10/10/2018 19:57:23 ******/
CREATE NONCLUSTERED INDEX [IX_TeamUsers_UserName] ON [dbo].[TeamUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CommentEntity]  WITH CHECK ADD  CONSTRAINT [FK_CommentEntity_Encounters_EncounterEntityId] FOREIGN KEY([EncounterEntityId])
REFERENCES [dbo].[Encounters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommentEntity] CHECK CONSTRAINT [FK_CommentEntity_Encounters_EncounterEntityId]
GO
ALTER TABLE [dbo].[CommentEntity]  WITH CHECK ADD  CONSTRAINT [FK_CommentEntity_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommentEntity] CHECK CONSTRAINT [FK_CommentEntity_Users_UserName]
GO
ALTER TABLE [dbo].[Encounters]  WITH CHECK ADD  CONSTRAINT [FK_Encounters_Sports_SportName] FOREIGN KEY([SportName])
REFERENCES [dbo].[Sports] ([SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Encounters] CHECK CONSTRAINT [FK_Encounters_Sports_SportName]
GO
ALTER TABLE [dbo].[Encounters]  WITH CHECK ADD  CONSTRAINT [FK_Encounters_Teams_AwayTeamName_AwayTeamSportName] FOREIGN KEY([AwayTeamName], [AwayTeamSportName])
REFERENCES [dbo].[Teams] ([Name], [SportName])
GO
ALTER TABLE [dbo].[Encounters] CHECK CONSTRAINT [FK_Encounters_Teams_AwayTeamName_AwayTeamSportName]
GO
ALTER TABLE [dbo].[Encounters]  WITH CHECK ADD  CONSTRAINT [FK_Encounters_Teams_HomeTeamName_HomeTeamSportName] FOREIGN KEY([HomeTeamName], [HomeTeamSportName])
REFERENCES [dbo].[Teams] ([Name], [SportName])
GO
ALTER TABLE [dbo].[Encounters] CHECK CONSTRAINT [FK_Encounters_Teams_HomeTeamName_HomeTeamSportName]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_Sports_SportName] FOREIGN KEY([SportName])
REFERENCES [dbo].[Sports] ([SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_Sports_SportName]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Teams_TeamName_SportName] FOREIGN KEY([TeamName], [SportName])
REFERENCES [dbo].[Teams] ([Name], [SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_Teams_TeamName_SportName]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_Users_UserName]
GO
USE [master]
GO
ALTER DATABASE [EDDB] SET  READ_WRITE 
GO
