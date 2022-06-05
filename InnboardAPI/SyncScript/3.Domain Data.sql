IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LastSyncDateTime' AND SetupName = 'LastSyncDateTime' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'LastSyncDateTime', N'LastSyncDateTime', convert(varchar, getdate(), 127), N'Last Sync Date Time', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SyncDayRange' AND SetupName = 'SyncDayRange' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SyncDayRange', N'SyncDayRange', N'30', N'Sync Data Day Range', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

