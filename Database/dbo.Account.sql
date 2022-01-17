CREATE TABLE [dbo].[Account] (
    [Email]              NCHAR (20)     NOT NULL,
	[FirstName]			 NVARCHAR (MAX) NULL,
	[LastName]           NVARCHAR (MAX) NULL,
    [Mobile]             NCHAR (10)     NULL,
    [Nric]               NVARCHAR (MAX) NULL,
    [PasswordHash]       NVARCHAR (MAX) NULL,
    [PasswordSalt]       NVARCHAR (MAX) NULL,
    [DateTimeRegistered] DATETIME       NOT NULL,
    [MobileVerified]     NCHAR (2)      NOT NULL,
    [EmailVerified]      NCHAR (2)      NOT NULL,
    [IV]                 NVARCHAR (MAX) NULL,
    [Key]                NVARCHAR (MAX) NULL,
    [DOB]                DATETIME2		NULL, 
    [CardNumber]		 NVARCHAR(MAX)	NULL, 
    [CardCV]			 NVARCHAR(MAX)	NULL, 
    [CardExpiry]		 DATETIME2		NULL, 
    [ProfileURL]		 NVARCHAR(MAX)	NULL, 
    PRIMARY KEY CLUSTERED ([Email])
);

