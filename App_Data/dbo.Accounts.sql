CREATE TABLE [dbo].[Accounts] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Email]              NVARCHAR (MAX) NOT NULL,
    [FirstName]          NVARCHAR (MAX) NULL,
    [LastName]           NVARCHAR (MAX) NULL,
    [Mobile]             NVARCHAR (MAX) NULL,
    [Nric]               NVARCHAR (MAX) NULL,
    [PasswordHash]       NVARCHAR (MAX) NULL,
    [PasswordSalt]       NVARCHAR (MAX) NULL,
    [DateTimeRegistered] NVARCHAR (MAX) NOT NULL,
    [MobileVerified]     NCHAR (2)      NULL,
    [EmailVerified]      NCHAR (2)      NULL,
    [IV]                 NVARCHAR (MAX) NULL,
    [Key]                NVARCHAR (MAX) NULL,
    [DOB]                NVARCHAR (MAX) NULL,
    [CardNumber]         NVARCHAR (MAX) NULL,
    [CardCV]             NVARCHAR (MAX) NULL,
    [CardExpiry]         NVARCHAR (MAX) NULL,
    [ProfileURL]         NVARCHAR (MAX) NULL,
    [Lockout]            NVARCHAR (MAX) NULL,
    [LockoutRecoveryDateTime] NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

