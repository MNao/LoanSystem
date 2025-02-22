USE [master]
GO
/****** Object:  Database [LeshLoanDb]    Script Date: 8/9/2019 12:35:31 PM ******/
CREATE DATABASE [LeshLoanDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LeshDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LeshDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LeshDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LeshDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LeshLoanDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LeshLoanDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LeshLoanDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LeshLoanDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LeshLoanDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LeshLoanDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LeshLoanDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [LeshLoanDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LeshLoanDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LeshLoanDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LeshLoanDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LeshLoanDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LeshLoanDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LeshLoanDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LeshLoanDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LeshLoanDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LeshLoanDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LeshLoanDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LeshLoanDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LeshLoanDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LeshLoanDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LeshLoanDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LeshLoanDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LeshLoanDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LeshLoanDb] SET RECOVERY FULL 
GO
ALTER DATABASE [LeshLoanDb] SET  MULTI_USER 
GO
ALTER DATABASE [LeshLoanDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LeshLoanDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LeshLoanDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LeshLoanDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LeshLoanDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'LeshLoanDb', N'ON'
GO
ALTER DATABASE [LeshLoanDb] SET QUERY_STORE = OFF
GO
USE [LeshLoanDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [LeshLoanDb]
GO
/****** Object:  Table [dbo].[AuditTrail]    Script Date: 8/9/2019 12:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditTrail](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [varchar](50) NULL,
	[TableName] [varchar](50) NULL,
	[CompanyCode] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Action] [nvarchar](4000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 8/9/2019 12:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[RecordId] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientNo] [varchar](50) NOT NULL,
	[ClientName] [varchar](50) NULL,
	[ClientAddress] [varchar](50) NULL,
	[TelephoneNumber] [varchar](50) NULL,
	[IDType] [varchar](50) NULL,
	[IDNumber] [varchar](50) NOT NULL,
	[ClientImage] [varchar](max) NOT NULL,
	[RefereeName] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Verified] [bit] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[Gender] [varchar](50) NULL,
	[RefereePhoneNo] [varchar](50) NULL,
	[IDImage] [varchar](max) NULL,
	[DOB] [varchar](50) NULL,
	[BusinessLoc] [varchar](50) NULL,
	[Occupation] [varchar](50) NULL,
	[NoOfBeneficiaries] [varchar](50) NULL,
	[EducLvl] [varchar](50) NULL,
	[MonthlyIncome] [varchar](50) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Counter]    Script Date: 8/9/2019 12:35:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Counter](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountIDClient] [varchar](50) NULL,
	[CountIDSup] [varchar](50) NULL,
	[CountIDLoan] [varchar](50) NULL,
	[CountIDRcpt] [varchar](50) NULL,
	[CountIDVch] [varchar](50) NULL,
	[CountIDInvEmp] [varchar](50) NULL,
	[CountIDPegEmp] [varchar](50) NULL,
	[CountIDSale] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Errorlogs]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Errorlogs](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorId] [varchar](50) NULL,
	[StackTrace] [varchar](7000) NULL,
	[Message] [varchar](950) NULL,
	[RecordDate] [datetime] NULL,
	[ErrorType] [varchar](100) NULL,
	[CompanyCode] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NULL,
	[ExpenseNo] [varchar](50) NOT NULL,
	[Amount] [varchar](100) NULL,
	[ExpenseDate] [datetime] NULL,
	[ExpenseDesc] [varchar](1000) NULL,
	[ExpenseType] [varchar](50) NULL,
	[ReceiptNo] [varchar](50) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[ExpenseNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incomes]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incomes](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NULL,
	[ClientCode] [varchar](50) NULL,
	[IncomeNo] [varchar](50) NOT NULL,
	[Amount] [varchar](100) NULL,
	[IncomeDate] [datetime] NULL,
	[IncomeDesc] [varchar](1000) NULL,
	[IncomeType] [varchar](50) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Incomes] PRIMARY KEY CLUSTERED 
(
	[IncomeNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Injections]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Injections](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NULL,
	[InjectionNo] [varchar](50) NOT NULL,
	[InjectionName] [varchar](50) NULL,
	[InjectionAmount] [varchar](100) NULL,
	[InjectionDate] [datetime] NULL,
	[InjectionDesc] [varchar](1000) NULL,
	[InjectionType] [varchar](50) NULL,
	[InjectionPayDate] [date] NULL,
	[InjectionPayAmount] [varchar](50) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Injections] PRIMARY KEY CLUSTERED 
(
	[InjectionNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterestTypes]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterestTypes](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NOT NULL,
	[InterestCode] [varchar](50) NOT NULL,
	[InterestName] [varchar](50) NOT NULL,
	[InterestValue] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_InterestTypes] PRIMARY KEY CLUSTERED 
(
	[InterestCode] ASC,
	[InterestName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanCollaterals]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanCollaterals](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[ClientNo] [varchar](50) NULL,
	[LoanNo] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Model] [varchar](500) NULL,
	[Make] [varchar](50) NULL,
	[SerialNumber] [varchar](50) NULL,
	[EstimatedPrice] [varchar](50) NULL,
	[ImageProof] [varchar](max) NULL,
	[Observations] [varchar](5000) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanPayments]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanPayments](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[ClientNo] [varchar](50) NULL,
	[LoanNo] [varchar](50) NULL,
	[AmountPaid] [varchar](50) NULL,
	[PaymentDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[PrincipalAmount] [varchar](50) NULL,
	[InterestAmount] [varchar](50) NULL,
	[OutStandingLoanAmount] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loans]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loans](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NULL,
	[LoanNo] [varchar](50) NOT NULL,
	[LoanAmount] [varchar](100) NULL,
	[LoanDate] [datetime] NULL,
	[LoanDesc] [varchar](1000) NULL,
	[LoanType] [varchar](50) NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ClientID] [varchar](50) NULL,
	[Approved] [bit] NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovedOn] [datetime] NULL,
	[AgreeT&C] [bit] NULL,
	[InterestRate] [varchar](50) NULL,
	[ApprovedAmount] [varchar](50) NULL,
	[LastLoanAmount] [varchar](50) NULL,
	[CurrentDebt] [varchar](50) NULL,
	[organization] [varchar](50) NULL,
	[EasyPaidAmount/Month] [varchar](50) NULL,
	[MonthsToPayIn] [varchar](50) NULL,
	[LoanStatus] [varchar](50) NULL,
	[LoanBalance] [varchar](50) NULL,
	[GuarantorProof] [varchar](max) NULL,
 CONSTRAINT [PK_Loans] PRIMARY KEY CLUSTERED 
(
	[LoanNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogInTrail]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogInTrail](
	[RecordID] [bigint] IDENTITY(1,1) NOT NULL,
	[Channel] [varchar](50) NULL,
	[SessionID] [varchar](50) NULL,
	[ChannelCode] [varchar](50) NULL,
	[ChannelMsg] [varchar](5000) NULL,
	[Action] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[IP] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordTrack]    Script Date: 8/9/2019 12:35:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordTrack](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [varchar](50) NULL,
	[OldPwd] [varchar](50) NULL,
	[RecordDate] [datetime] NULL,
	[IP] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemSettings]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[ValueID] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NOT NULL,
	[SettingCode] [varchar](50) NOT NULL,
	[SettingValue] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[SettingCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUsers]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUsers](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NULL,
	[UserId] [varchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[RoleCode] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[IsActive] [bit] NULL,
	[Email] [varchar](50) NULL,
	[ResetPassword] [bit] NULL,
 CONSTRAINT [PK_SystemUsers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTypes]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTypes](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](50) NOT NULL,
	[RoleCode] [varchar](50) NOT NULL,
	[RoleName] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[RoleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ChangePassword]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ChangePassword]
@UserId varchar(50),
@CompanyCode varchar(50),
@Password varchar(150),
@userType varchar(50)
as 
Begin
if exists(select * from SystemUsers where UserId=@UserId)
	Begin
	Update SystemUsers set Password=@Password,ModifiedBy=@userType,ModifiedOn=GETDATE(),ResetPassword=0 where
	CompanyCode=@CompanyCode and UserId=@UserId
	End
	Select SCOPE_IDENTITY() as UserTypeCode
End
GO
/****** Object:  StoredProcedure [dbo].[DeactivateUser]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[DeactivateUser]
@UserID varchar(50),
@Channel varchar(50),
@Ip varchar(50),
@CompanyCode varchar(50)

as
Begin
if exists(select * from Systemusers where UserID=@UserID and CompanyCode=@CompanyCode)
Begin
	Update SystemUsers set IsActive=0 where
	CompanyCode=@CompanyCode and UserId=@UserId
End
End
GO
/****** Object:  StoredProcedure [dbo].[GetClientDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetClientDetails]
@CompanyCode varchar(50),
@ClientNo varchar(50)
as
Select * from Clients where (ClientNo=@ClientNo or @ClientNo='') --CompanyCode=@CompanyCode
GO
/****** Object:  StoredProcedure [dbo].[GetClientSearchDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetClientSearchDetails]
@ClientDet varchar(50)
as
Begin
select top 10 ClientNo,ClientName from Clients where (ClientName like ''+@ClientDet+'%' or  ClientNo like ''+@ClientDet+'%')
End
GO
/****** Object:  StoredProcedure [dbo].[GetClientsForDropDown]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetClientsForDropDown]
@CompanyCode varchar(50),
@ClientNo varchar(50)
as
Select ClientNo,ClientName from Clients where (ClientNo=@ClientNo or @ClientNo='') --CompanyCode=@CompanyCode
GO
/****** Object:  StoredProcedure [dbo].[GetCountID]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetCountID]
@SystemCode varchar(20)
as
Begin
--select max(countID) as Count from Counter 
if(@SystemCode='CLI')
	Begin
	SELECT RIGHT('1000'+ CONVERT(VARCHAR,CountIDClient),5) as CountID FROM counter
	update Counter set CountIDClient=CountIDClient+1
	End
else if(@SystemCode='LOAN')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDLoan),5) as CountID FROM counter
	update Counter set CountIDLoan=CountIDLoan+1
	End
else if(@SystemCode='VCH')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDVch),5) as CountID FROM counter
	update Counter set CountIDVch=CountIDVch+1
	End
else if(@SystemCode='RCPT')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDRcpt),5) as CountID FROM counter
	update Counter set CountIDRcpt=CountIDRcpt+1
	End
else if(@SystemCode='INVEMP')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDInvEmp),5) as CountID FROM counter
	update Counter set CountIDInvEmp=CountIDInvEmp+1
	End
else if(@SystemCode='PEGEMP')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDPegEmp),5) as CountID FROM counter
	update Counter set CountIDPegEmp=CountIDPegEmp+1
	End
else if(@SystemCode='SALE')
	Begin
	SELECT RIGHT('000'+ CONVERT(VARCHAR,CountIDSale),5) as CountID FROM counter
	update Counter set CountIDSale=CountIDSale+1
	End
End
GO
/****** Object:  StoredProcedure [dbo].[GetLoanDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetLoanDetails]
@ClientNo varchar(50),
@LoanNo varchar(50)
as
Select * from Loans where (ClientID=@ClientNo or @ClientNo='') and LoanNo=@LoanNo --CompanyCode=@CompanyCode
GO
/****** Object:  StoredProcedure [dbo].[GetLoanDetailsForSchedule]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetLoanDetailsForSchedule]
@ClientNo varchar(50),
@LoanNo varchar(50)
as
Declare @ClientName varchar(100);
Declare @ApprovedAmount varchar(50);
Declare @LoanAmount varchar(50);
Declare @InterestAmount varchar(50);
set @ClientName=(select ClientName from Clients where ClientNo=@ClientNo)
select @ApprovedAmount=approvedAmount, @LoanAmount=LoanAmount 
,@InterestAmount=(Convert(float,approvedAmount)- Convert(float,LoanAmount)) 
from Loans where LoanNo=@LoanNo and ClientID=@ClientNo
--set @InterestAmount= (@ApprovedAmount - @LoanAmount);
Select LoanNo,ClientID,@ClientName as ClientName,LoanDate,LoanAmount,[EasyPaidAmount/Month] as MonthlyInstallment, @InterestAmount as InterestAmount, MonthsToPayIn as PaymentPeriod, LoanDesc, InterestRate from Loans where (ClientID=@ClientNo) and LoanNo=@LoanNo
GO
/****** Object:  StoredProcedure [dbo].[GetLoanPayments]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetLoanPayments]
@ClientID varchar(50),
@LoanNo varchar(50),
@CapturedBy varchar(50)
as
Begin
select * from LoanPayments where (ClientNo=@ClientID or @ClientID='') and (LoanNo=@LoanNo or @LoanNo='') and (AddedBy=@CapturedBy or @CapturedBy='') order by AddedOn desc
End
GO
/****** Object:  StoredProcedure [dbo].[GetSystemUserByUserId]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[GetSystemUserByUserId]
@UserId varchar(50)
as
SELECT * FROM [dbo].[SystemUsers] where UserId=@UserId
GO
/****** Object:  StoredProcedure [dbo].[GetUserTypesForDropDown]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetUserTypesForDropDown]
as
select RoleName,RoleCode  from UserTypes
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoAuditTrail]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[InsertIntoAuditTrail]
@ActionType varchar(50),
@TableName varchar(50),
@CompanyCode varchar(50),
@ModifiedBy varchar(50),
@Action nvarchar(4000)
as
Insert into AuditTrail([Action],ActionType,CompanyCode,ModifiedBy,ModifiedOn,TableName)
values(@Action,@ActionType,@CompanyCode,@ModifiedBy,GETDATE(),@TableName)

Select SCOPE_IDENTITY() as InsertedId
GO
/****** Object:  StoredProcedure [dbo].[LogUserLogin]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[LogUserLogin]
@Channel varchar(50),
@Ip varchar(50),
@UserId varchar(50),
@SessionID varchar(50),
@Code varchar(50),
@Message varchar(500),
@Action varchar(50)

as
Begin Transaction trans
INSERT INTO LogInTrail
           (Channel
           ,IP
           ,ModifiedBy
           ,SessionID
           ,ChannelCode
           ,ChannelMsg
           ,Action
           ,[ModifiedOn])
     VALUES
           (@Channel,
		   @Ip,
		   @UserId,
		   @SessionID,
		   @Code,
		   @Message,
		   @Action,
		   GETDATE())

Select @UserId as InsertedID
Commit Transaction trans
GO
/****** Object:  StoredProcedure [dbo].[PasswordTrack_SelectRow]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[PasswordTrack_SelectRow]
@UserId varchar(50),
@Password varchar(50)

as
select * from passwordTrack where UserID=@UserId and OldPwd=@password
GO
/****** Object:  StoredProcedure [dbo].[PasswordTracker_Update]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[PasswordTracker_Update]
@UserId varchar(50),
@OldPwd varchar(50),
@Ip varchar(50)
as 
	Begin
	insert into PasswordTrack(UserID,OldPwd,IP,RecordDate) values(@UserId,@OldPwd,@Ip,GETDATE())
	End
GO
/****** Object:  StoredProcedure [dbo].[SaveClientsDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveClientsDetails]
@ClientNo varchar(50),
@ClientName varchar(50),
@ClientAddress varchar(50),
@ClientPhoneNo varchar(50),
@Gender varchar(50),
@IDType varchar(50),
@IDNo varchar(500),
@ClientPhoto varchar(MAX),
@IDPhoto varchar(MAX),
@RefereeName varchar(50),
@RefereePhoneNo varchar(50),
@ClientEmail varchar(50),
@ModifiedBy varchar(50),
@ClientPwd varchar(100)
as
if exists(select * from Clients where ClientNo=@ClientNo)
Begin
	Update Clients set ClientName=@ClientName,TelephoneNumber=@ClientPhoneNo,IDNumber=@IDNo,Email=@ClientEmail,ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() where ClientNo=@ClientNo
End
Else
Begin
INSERT INTO [dbo].Clients
           (ClientNo
			,ClientName
			,ClientAddress
			,TelephoneNumber
			,IDType
			,IDNumber
		   ,ClientImage
		   ,IDImage
		   ,RefereeName
		   ,RefereePhoneNo
		   ,Email
		   ,Gender
           ,CreatedBy
           ,CreatedOn)
     VALUES
           (@ClientNo,
           @ClientName,
           @ClientAddress,
		   @ClientPhoneNo,
		   @IDType,
		   @IDNo,
		   @ClientPhoto,
		   @IDPhoto,
		   @RefereeName,
		   @RefereePhoneNo,
		   @ClientEmail,
		   @Gender,
		   @ModifiedBy,
           GETDATE())

		   Declare @RoleCode varchar(50)
		   set @RoleCode = (select RoleCode from UserTypes where RoleName='Client')
		   exec SaveSystemUser 'Lensh',@ClientNo,@ClientPwd,@ClientName,@RoleCode,@ModifiedBy,1,@ClientEmail
End
Select @ClientNo as InsertedID
GO
/****** Object:  StoredProcedure [dbo].[SaveCollateralDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[SaveCollateralDetails]
@LoanNo varchar(50),
@Name varchar(50),
@Type varchar(50),
@Model varchar(50),
@Make varchar(50),
@SerialNo varchar(50),
@EstimatedPrice varchar(50),
@ImgProof varchar(max),
@Observations varchar(5000),
@AddedBy varchar(50)
as
if exists (select * from Loans where LoanNo=@LoanNo)
Begin
insert into LoanCollaterals (
		LoanNo,
		Name,
		Type,
		Model,
		Make,
		SerialNumber,
		EstimatedPrice,
		ImageProof,
		Observations,
		AddedBy,
		AddedOn
		)
		VALUES(
		@LoanNo,
		@Name,
		@Type,
		@Model,
		@Make,
		@SerialNo,
		@EstimatedPrice,
		@ImgProof,
		@Observations,
		@AddedBy,
		GETDATE()
		)
End
select @LoanNo as InsertedID
GO
/****** Object:  StoredProcedure [dbo].[SaveLoanApproval]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SaveLoanApproval]
@LoanNo varchar(50),
@Approved varchar(50),
@ApprovedAmount varchar(50),
@MonthsToPayIn varchar(50),
@AmountPerMonth varchar(50),
@GuarantorImageproof varchar(MAX),
@ApprovedBy varchar(50)
as
if exists (select * from Loans where LoanNo=@LoanNo)
Begin
Update Loans set [EasyPaidAmount/Month]=@AmountPerMonth,ApprovedAmount=@ApprovedAmount,LoanBalance=@ApprovedAmount,MonthsToPayIn=@MonthsToPayIn, GuarantorProof=@GuarantorImageproof, Approved=@Approved, ApprovedBy=@ApprovedBy, ApprovedOn=GETDATE() where LoanNo=@LoanNo
End
select @LoanNo as InsertedID
GO
/****** Object:  StoredProcedure [dbo].[SaveLoanDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveLoanDetails]
@ClientNo varchar(50),
@LoanNo varchar(50),
@LoanDate varchar(50),
@LoanAmount varchar(50),
@InterestRate varchar(50),
@ApprovedAmount varchar(50),
@LoanDesc varchar(500),
@LastLoanAmount varchar(500),
@CurrentDebt varchar(500),
@Organization varchar(50),
@EasyPdAmountPerMonth varchar(50),
@ModifiedBy varchar(50)
as
if exists(select * from Loans where LoanNo=@LoanNo and ClientID=@ClientNo)
Begin
	Update Loans set LoanDate=@LoanDate,LoanAmount=@LoanAmount,InterestRate=@InterestRate,ApprovedAmount=@ApprovedAmount,LoanDesc=@LoanDesc,ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() where LoanNo=@LoanNo and ClientID=@ClientNo
End
Else
Begin
INSERT INTO [dbo].Loans
           (ClientID
			,LoanNo
			,LoanDate
			,LoanAmount
			,InterestRate
			,ApprovedAmount
		   ,LoanDesc
		   ,LastLoanAmount
		   ,CurrentDebt
		   ,organization
		   ,[EasyPaidAmount/Month]
           ,[AddedBy]
           ,[AddedOn]
		   ,[AgreeT&C])
     VALUES
           (@ClientNo,
           @LoanNo,
           @LoanDate,
		   @LoanAmount,
		   @InterestRate,
		   @ApprovedAmount,
		   @LoanDesc,
		   @LastLoanAmount,
		   @CurrentDebt,
		   @Organization,
		   @EasyPdAmountPerMonth,
		   @ModifiedBy,
           GETDATE(),
		   0)
End
Select @LoanNo as InsertedID
GO
/****** Object:  StoredProcedure [dbo].[SaveLoanPaymentDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SaveLoanPaymentDetails]
@ClientID varchar(50),
@LoanNo varchar(50),
@PaidAmount varchar(50),
@PaymentDate varchar(50),
@Remarks varchar(5000),
@AddedBy varchar(50)
as
Begin Try
Begin Transaction Payment

Begin
Declare @LoanBalance varchar(50);
Declare @NewLoanBalance varchar(50);
Declare @PrevLoanBalance varchar(50);
Declare @ErrorMsg varchar(50);

--if not exists(select * from Loans where LoanNo=@LoanNo and ClientID=@ClientID and LoanBalance>0)
select @LoanBalance=LoanBalance from Loans where LoanNo=@LoanNo and ClientID=@ClientID
if((@LoanBalance is NULL) or (@LoanBalance<=0))
Begin
set @ErrorMsg = 'NO LOAN TO PAY FOUND. LOAN MUST HAVE BEEN PAID OFF';
	RAISERROR (@ErrorMsg, -- Message text.
				   16,    -- Severity.
				   1      -- State.
				   );
	RETURN
End
Else
Begin

set @NewLoanBalance = Convert(decimal,@LoanBalance) - Convert(decimal,@PaidAmount)
Update Loans set LoanBalance=@NewLoanBalance where LoanNo=@LoanNo
insert into LoanPayments (ClientNo,LoanNo,AmountPaid,PaymentDate,Remarks,OutStandingLoanAmount,AddedBy,AddedOn) VALUES (@ClientID,@LoanNo,@PaidAmount,@PaymentDate,@Remarks,@NewLoanBalance,@AddedBy,GETDATE())
select RecordId as InsertedID from LoanPayments
End

Commit Transaction Payment
End
End Try
BEGIN CATCH
    Rollback Transaction Payment
 DECLARE
   @ErMessage NVARCHAR(2048),
   @ErSeverity INT,
   @ErState INT
 
 SELECT
   @ErMessage = ERROR_MESSAGE(),
   @ErSeverity = ERROR_SEVERITY(),
   @ErState = ERROR_STATE()
 
 RAISERROR (@ErMessage,
             @ErSeverity,
             @ErState )
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[SaveSystemUser]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SaveSystemUser]
@CompanyCode varchar(50),
@UserId varchar(50),
@Password varchar(50),
@Name varchar(50),
@UserType varchar(50),
@ModifiedBy varchar(50),
@IsActive bit,
@Email varchar(50)
as
Begin Transaction trans
INSERT INTO [dbo].[SystemUsers]
           ([CompanyCode]
           ,[UserId]
           ,[Password]
           ,[Name]
           ,RoleCode
           ,[ModifiedBy]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[CreatedOn]
           ,[IsActive]
           ,Email
		   ,ResetPassword)
     VALUES
           (@CompanyCode,
		   @UserId,
		   @Password,
		   @Name,
		   @UserType,
		   @ModifiedBy,
		   @ModifiedBy,
		   GETDATE(),
		   GETDATE(),
		   @IsActive,
		   @Email,
		   1)

Select @UserId as InsertedID
Commit Transaction trans
GO
/****** Object:  StoredProcedure [dbo].[SearchClientDetailsTable]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SearchClientDetailsTable]
@ClientNo varchar(50),
@UserID varchar(50),
@Status varchar(50),
@StartDate varchar(50),
@EndDate varchar(50)

as

select ClientNo,ClientName,ClientAddress,TelephoneNumber,IDType,IDNumber,Email,Gender,RefereeName,RefereePhoneNo,Verified,CreatedBy,CreatedOn from Clients where (ClientNo=@ClientNo or @ClientNo='') order by CreatedOn desc
GO
/****** Object:  StoredProcedure [dbo].[SearchLoanDetailsTable]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SearchLoanDetailsTable]
@ClientNo varchar(50),
@UserID varchar(50),
@Status varchar(50),
@StartDate varchar(50),
@EndDate varchar(50)

as
--select * from Loans
select ClientID,LoanNo,LoanAmount,LoanDesc,Convert(varchar(10),LoanDate,101) as LoanDate,InterestRate,MonthsToPayIn,[EasyPaidAmount/Month] as AmountPerMonth,ApprovedAmount,Approved,AddedBy,AddedOn from Loans where (ClientID=@ClientNo or @ClientNo='') order by AddedOn desc
GO
/****** Object:  StoredProcedure [dbo].[SearchLoanForApproval]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SearchLoanForApproval]
@ClientNo varchar(50),
@UserID varchar(50),
@Status varchar(50),
@StartDate varchar(50),
@EndDate varchar(50)

as
--select * from Loans
select ClientID,LoanNo,LoanAmount,LoanDesc,InterestRate,
[EasyPaidAmount/Month] as AmountPermonth,MonthsToPayIn,ApprovedAmount,Approved,ApprovedBy,ApprovedOn,
AddedBy,AddedOn from Loans where (ClientID=@ClientNo or @ClientNo='') and (Approved=0 or Approved is null) order by AddedOn desc
GO
/****** Object:  StoredProcedure [dbo].[SearchLoanForRepay]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SearchLoanForRepay]
@ClientNo varchar(50),
@LoanNo varchar(50),
@UserID varchar(50),
@Status varchar(50),
@StartDate varchar(50),
@EndDate varchar(50)

as
--select * from Loans
select ClientID,LoanNo,LoanAmount,Convert(varchar(10),LoanDate,101) as LoanDate,LoanDesc,MonthsToPayIn,ApprovedAmount,[EasyPaidAmount/Month] as AmountPermonth, LoanBalance from Loans where (LoanNo=@LoanNo or @LoanNo='') and (ClientID=@ClientNo or @ClientNo='') and LoanBalance>0 order by AddedOn desc
GO
/****** Object:  StoredProcedure [dbo].[SystemSettings_SelectRow]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[SystemSettings_SelectRow]
@CompanyCode varchar(50),
@SettingCode varchar(50)
	
As
Begin
	Select 
	*
	From SystemSettings
	Where (CompanyCode=@CompanyCode or CompanyCode='ALL') and 
	(SettingCode=@SettingCode or @SettingCode='')
End
GO
/****** Object:  StoredProcedure [dbo].[UpdateLoanStatus]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UpdateLoanStatus]
@LoanNo varchar(50),
@ClientID varchar(50),
@UserType varchar(50),
@UserID varchar(50)
as
update Loans set [AgreeT&C]=1 where LoanNo=@LoanNo and ClientID=@ClientID
GO
/****** Object:  StoredProcedure [dbo].[UpdatewithAdditionalClientsDetails]    Script Date: 8/9/2019 12:35:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UpdatewithAdditionalClientsDetails]
@ClientNo varchar(50),
@DOB varchar(50),
@BusinessLoc varchar(50),
@Occup varchar(50),
@NoOfBenef varchar(50),
@EducLvl varchar(50),
@MonthlyInc varchar(50),
@ModifiedBy varchar(50)
as
if exists(select * from Clients where ClientNo=@ClientNo)
Begin
	Update Clients set DOB=@DOB,BusinessLoc=@BusinessLoc,Occupation=@Occup,NoOfBeneficiaries=@NoOfBenef,EducLvl=@EducLvl,MonthlyIncome=@MonthlyInc,ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() where ClientNo=@ClientNo
End
Select @ClientNo as InsertedID
GO
USE [master]
GO
ALTER DATABASE [LeshLoanDb] SET  READ_WRITE 
GO
