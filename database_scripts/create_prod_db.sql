GO
USE MASTER
GO
DROP DATABASE NINERFI
GO
CREATE DATABASE NINERFI
GO
USE NINERFI
GO
CREATE TABLE [Hosts] (
	UID uniqueidentifier NOT NULL DEFAULT (NEWID()),
	Hostname nchar(50) NOT NULL,
	IPAddress nchar(13) NOT NULL
  CONSTRAINT [PK_HOSTS] PRIMARY KEY CLUSTERED
  (
  [UID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [LogCode] (
	Code int NOT NULL,
	Description ntext,
  CONSTRAINT [PK_LOGCODE] PRIMARY KEY CLUSTERED
  (
  [Code] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [LogEntry] (
	UID uniqueidentifier NOT NULL DEFAULT (NEWID()),
	EntryTimestamp datetime NOT NULL,
	Host uniqueidentifier NOT NULL,
	Process uniqueidentifier NOT NULL,
	LogCode int NOT NULL,
	EntryType int NOT NULL,
  CONSTRAINT [PK_LOGENTRY] PRIMARY KEY CLUSTERED
  (
  [UID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [EntryType] (
	UID int NOT NULL IDENTITY(1,1),
	TypeName nchar(5) NOT NULL,
  CONSTRAINT [PK_ENTRYTYPE] PRIMARY KEY CLUSTERED
  (
  [UID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Process] (
	UID uniqueidentifier NOT NULL DEFAULT (NEWID()),
	ProcessName nchar(10) NOT NULL,
	ProcessNumber int NOT NULL,
  CONSTRAINT [PK_PROCESS] PRIMARY KEY CLUSTERED
  (
  [UID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO



/***** START AIDEN VIEWS ******/

GO
CREATE VIEW vwLogCount
WITH schemabinding
as
SELECT 
	DATEPART(YEAR, EntryTimestamp) Year
	,DATEPART(MONTH, EntryTimestamp) Month
	,DATEPART(DAY, EntryTimestamp) Day
	,DATEPART(HOUR, EntryTimestamp) Hour
	,COUNT_BIG(*) number_of_logs
FROM dbo.LogEntry
GROUP BY 
	DATEPART(YEAR, EntryTimestamp) 
	,DATEPART(MONTH, EntryTimestamp) 
	,DATEPART(DAY, EntryTimestamp) 
	,DATEPART(HOUR, EntryTimestamp);
GO

create unique clustered index UIX_vwLogCount on vwLogCount (Year, Month, Day, Hour)

GO
CREATE VIEW vwErrorTracking
WITH schemabinding as
SELECT
	DATEPART(YEAR, A.EntryTimestamp) Year
	,DATEPART(MONTH, A.EntryTimestamp) Month
	,DATEPART(DAY, A.EntryTimestamp) Day
	,B.Hostname
	,C.TypeName
	,COUNT_BIG(*) log_entries
FROM dbo.LogEntry A
INNER JOIN  dbo.Hosts B
ON A.Host = B.UID
INNER JOIN dbo.EntryType C
ON  A.EntryType = C.UID
WHERE C.TypeName IN
('ERRS', 'INFO', 'WARN', 'NOTI') AND Hostname LIKE '%[A-Z]%'
GROUP BY 
	DATEPART(YEAR, A.EntryTimestamp) 
	,DATEPART(MONTH, A.EntryTimestamp) 
	,DATEPART(DAY, A.EntryTimestamp)
	,B.Hostname
	,C.TypeName;
GO

create unique clustered index UIX_vwErrorTracking on vwErrorTracking (Year, Month, Day, Hostname, Typename)


GO
CREATE VIEW vwTrafficStats
WITH schemabinding AS
SELECT
	DATEPART(YEAR, A.EntryTimestamp) Year
	,DATEPART(MONTH, A.EntryTimestamp) Month
	,DATEPART(DAY, A.EntryTimestamp) Day
	,B.Hostname
	,COUNT_BIG(*) log_entries
FROM dbo.LogEntry A
INNER JOIN  dbo.Hosts B
ON A.Host = B.UID
WHERE Hostname LIKE '%[a-z]%'
GROUP BY
	DATEPART(YEAR, A.EntryTimestamp) 
	,DATEPART(MONTH, A.EntryTimestamp) 
	,DATEPART(DAY, A.EntryTimestamp)
	,B.Hostname
GO

create unique clustered index UIX_vwTrafficStats on vwTrafficStats (Year, Month, Day, Hostname)


/***** END AIDEN VIEWS ******/

ALTER TABLE [LogEntry] WITH CHECK ADD CONSTRAINT [LogEntry_fk0] FOREIGN KEY ([Host]) REFERENCES [Hosts]([UID])
ON UPDATE CASCADE
GO
ALTER TABLE [LogEntry] CHECK CONSTRAINT [LogEntry_fk0]
GO
ALTER TABLE [LogEntry] WITH CHECK ADD CONSTRAINT [LogEntry_fk1] FOREIGN KEY ([Process]) REFERENCES [Process]([UID])
ON UPDATE CASCADE
GO
ALTER TABLE [LogEntry] CHECK CONSTRAINT [LogEntry_fk1]
GO
ALTER TABLE [LogEntry] WITH CHECK ADD CONSTRAINT [LogEntry_fk2] FOREIGN KEY ([LogCode]) REFERENCES [LogCode]([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [LogEntry] CHECK CONSTRAINT [LogEntry_fk2]
GO
ALTER TABLE [LogEntry] WITH CHECK ADD CONSTRAINT [LogEntry_fk3] FOREIGN KEY ([EntryType]) REFERENCES [EntryType]([UID])
ON UPDATE CASCADE
GO
ALTER TABLE [LogEntry] CHECK CONSTRAINT [LogEntry_fk3]
GO


USE [NINERFI]
GO
CREATE LOGIN [CAPSTONE\aidenl] FROM WINDOWS;
CREATE LOGIN [CAPSTONE\willr] FROM WINDOWS;
CREATE LOGIN [NT AUTHORITY\NETWORK SERVICE] FROM WINDOWS;
GO
ALTER ROLE [db_owner] ADD MEMBER [CAPSTONE\aidenl]
ALTER ROLE [db_owner] ADD MEMBER [CAPSTONE\willr]
ALTER ROLE [db_datareader] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
ALTER ROLE [db_datawriter] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO