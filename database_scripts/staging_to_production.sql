use NINERFI
go

select COUNT(*) numberoflogsbeforetransfer from NINERFI.dbo.LogEntry;

begin tran

-- insert log codes
insert into NINERFI.dbo.LogCode(Code)
	select DISTINCT LogCode
	from NINERFISTAGING.dbo.STAGING s
	WHERE NOT EXISTS (
		SELECT 1
		FROM NINERFI.dbo.LogCode p
		WHERE Code = s.LogCode
		);

-- insert hosts
insert into NINERFI.dbo.Hosts(Hostname, IPAddress)
	select distinct s.Hostname, s.IPAddress
	from NINERFISTAGING.dbo.STAGING s
	where not exists (
		select 1
		from NINERFI.dbo.Hosts p
		where p.Hostname = s.Hostname
		);


-- insert Process
insert into NINERFI.dbo.Process(ProcessName, ProcessNumber)
	select distinct s.ProcessName, s.ProcessNumber
	from NINERFISTAGING.dbo.STAGING s
	where not exists (
		select 1
		from NINERFI.dbo.Process p
		where p.ProcessName = s.ProcessName and p.ProcessNumber = p.ProcessNumber
		);



-- insert EntryType
insert into NINERFI.dbo.EntryType(TypeName)
	select distinct s.EntryTypeName
	from NINERFISTAGING.dbo.STAGING s
	where not exists (
		select 1
		from NINERFI.dbo.EntryType p
		where p.TypeName = s.EntryTypeName
		);



-- insert logentry

insert into NINERFI.dbo.LogEntry(EntryTimestamp, Host, Process, LogCode, EntryType)
	select s.EntryTimestamp, h.UID hostname, p.UID processname, lc.Code, et.UID entrytype
	from NINERFISTAGING.dbo.STAGING s
	inner join NINERFI.dbo.EntryType et ON et.TypeName = s.EntryTypeName
	inner join NINERFI.dbo.Hosts h ON h.Hostname = s.Hostname AND h.IPAddress = s.IPAddress
	inner join NINERFI.dbo.LogCode lc ON lc.Code = s.LogCode
	inner join NINERFI.dbo.Process p ON p.ProcessName = s.ProcessName and p.ProcessNumber = s.ProcessNumber
	where not exists (
		select 1
		from NINERFI.dbo.LogEntry le
		where le.EntryTimestamp = s.EntryTimestamp AND h.Hostname = s.Hostname AND lc.Code = s.LogCode AND p.ProcessName = s.ProcessName AND p.ProcessNumber = s.ProcessNumber
		) order by s.EntryTimestamp





-- rollback tran
commit tran

select COUNT(*) numberoflogsaftertransfer from NINERFI.dbo.LogEntry;


-- select * from Process
-- select * from LogCode
-- select * from Hosts
-- select * from EntryType
-- select * from LogEntry
