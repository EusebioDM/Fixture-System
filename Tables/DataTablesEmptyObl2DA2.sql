USE [master]
GO
/****** Object:  Database [EDDB]    Script Date: 22/11/2018 22:10:18 ******/
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
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 22/11/2018 22:10:18 ******/
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
/****** Object:  Table [dbo].[Comments]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[UserName] [nvarchar](450) NULL,
	[TimeStamp] [datetime2](7) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[EncounterEntityId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Encounters]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encounters](
	[Id] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[SportName] [nvarchar](450) NULL,
 CONSTRAINT [PK_Encounters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EncounterTeam]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EncounterTeam](
	[EncounterId] [uniqueidentifier] NOT NULL,
	[TeamName] [nvarchar](450) NOT NULL,
	[SportName] [nvarchar](450) NOT NULL,
	[TeamNameFk] [nvarchar](450) NULL,
	[SportNameFk] [nvarchar](450) NULL,
 CONSTRAINT [PK_EncounterTeam] PRIMARY KEY CLUSTERED 
(
	[TeamName] ASC,
	[SportName] ASC,
	[EncounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Logs]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[DateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sports]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sports](
	[SportName] [nvarchar](450) NOT NULL,
	[EncounterPlayerCount] [int] NOT NULL,
 CONSTRAINT [PK_Sports] PRIMARY KEY CLUSTERED 
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TeamResult]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamResult](
	[TeamName] [nvarchar](450) NULL,
	[TeamSportName] [nvarchar](450) NULL,
	[EncounterId] [uniqueidentifier] NOT NULL,
	[TeamId] [nvarchar](450) NOT NULL,
	[Position] [int] NOT NULL,
	[EncounterEntityId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TeamResult] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC,
	[EncounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 22/11/2018 22:10:18 ******/
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
/****** Object:  Table [dbo].[TeamUsers]    Script Date: 22/11/2018 22:10:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamUsers](
	[TeamName] [nvarchar](450) NOT NULL,
	[SportName] [nvarchar](450) NOT NULL,
	[TeamName1] [nvarchar](450) NULL,
	[TeamSportName] [nvarchar](450) NULL,
	[UserName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_TeamUsers] PRIMARY KEY CLUSTERED 
(
	[TeamName] ASC,
	[SportName] ASC,
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 22/11/2018 22:10:18 ******/
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
/****** Object:  Index [IX_Comments_EncounterEntityId]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_Comments_EncounterEntityId] ON [dbo].[Comments]
(
	[EncounterEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Comments_UserName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_Comments_UserName] ON [dbo].[Comments]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Encounters_SportName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_Encounters_SportName] ON [dbo].[Encounters]
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EncounterTeam_EncounterId]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_EncounterTeam_EncounterId] ON [dbo].[EncounterTeam]
(
	[EncounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_EncounterTeam_TeamNameFk_SportNameFk]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_EncounterTeam_TeamNameFk_SportNameFk] ON [dbo].[EncounterTeam]
(
	[TeamNameFk] ASC,
	[SportNameFk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamResult_EncounterEntityId]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_TeamResult_EncounterEntityId] ON [dbo].[TeamResult]
(
	[EncounterEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TeamResult_TeamName_TeamSportName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_TeamResult_TeamName_TeamSportName] ON [dbo].[TeamResult]
(
	[TeamName] ASC,
	[TeamSportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Teams_SportName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_Teams_SportName] ON [dbo].[Teams]
(
	[SportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TeamUsers_TeamName1_TeamSportName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_TeamUsers_TeamName1_TeamSportName] ON [dbo].[TeamUsers]
(
	[TeamName1] ASC,
	[TeamSportName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TeamUsers_UserName]    Script Date: 22/11/2018 22:10:18 ******/
CREATE NONCLUSTERED INDEX [IX_TeamUsers_UserName] ON [dbo].[TeamUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Encounters_EncounterEntityId] FOREIGN KEY([EncounterEntityId])
REFERENCES [dbo].[Encounters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Encounters_EncounterEntityId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users_UserName]
GO
ALTER TABLE [dbo].[Encounters]  WITH CHECK ADD  CONSTRAINT [FK_Encounters_Sports_SportName] FOREIGN KEY([SportName])
REFERENCES [dbo].[Sports] ([SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Encounters] CHECK CONSTRAINT [FK_Encounters_Sports_SportName]
GO
ALTER TABLE [dbo].[EncounterTeam]  WITH CHECK ADD  CONSTRAINT [FK_EncounterTeam_Encounters_EncounterId] FOREIGN KEY([EncounterId])
REFERENCES [dbo].[Encounters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EncounterTeam] CHECK CONSTRAINT [FK_EncounterTeam_Encounters_EncounterId]
GO
ALTER TABLE [dbo].[EncounterTeam]  WITH CHECK ADD  CONSTRAINT [FK_EncounterTeam_Teams_TeamNameFk_SportNameFk] FOREIGN KEY([TeamNameFk], [SportNameFk])
REFERENCES [dbo].[Teams] ([Name], [SportName])
GO
ALTER TABLE [dbo].[EncounterTeam] CHECK CONSTRAINT [FK_EncounterTeam_Teams_TeamNameFk_SportNameFk]
GO
ALTER TABLE [dbo].[TeamResult]  WITH CHECK ADD  CONSTRAINT [FK_TeamResult_Encounters_EncounterEntityId] FOREIGN KEY([EncounterEntityId])
REFERENCES [dbo].[Encounters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeamResult] CHECK CONSTRAINT [FK_TeamResult_Encounters_EncounterEntityId]
GO
ALTER TABLE [dbo].[TeamResult]  WITH CHECK ADD  CONSTRAINT [FK_TeamResult_Teams_TeamName_TeamSportName] FOREIGN KEY([TeamName], [TeamSportName])
REFERENCES [dbo].[Teams] ([Name], [SportName])
GO
ALTER TABLE [dbo].[TeamResult] CHECK CONSTRAINT [FK_TeamResult_Teams_TeamName_TeamSportName]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_Sports_SportName] FOREIGN KEY([SportName])
REFERENCES [dbo].[Sports] ([SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_Sports_SportName]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Teams_TeamName1_TeamSportName] FOREIGN KEY([TeamName1], [TeamSportName])
REFERENCES [dbo].[Teams] ([Name], [SportName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_Teams_TeamName1_TeamSportName]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_Users_UserName]
GO
USE [master]
GO
ALTER DATABASE [EDDB] SET  READ_WRITE 
GO
