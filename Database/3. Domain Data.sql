 -- -- -- -- -- CommonModuleType  ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Maintenance' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Maintenance')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Task Management' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Task Management')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Fixed Asset' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Fixed Asset')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Document Management' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Document Management')
END
GO
-- -- -- -- -- CommonModuleName  ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Maintenance' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 13, N'Maintenance', N'grpMaintenance', N'/Maintenance/', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Report:: Maintenance' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 13, N'Report:: Maintenance', N'grpReportMaintenance ', N'/Maintenance/Reports', 1, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Task Management' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 14, N'Task Management', N'grpTaskManagement', N'/TaskManagement/', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Report:: Task Management' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 14, N'Report:: Task Management', N'grpReportTaskManagement ', N'/TaskManagement/Reports', 1, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Fixed Asset' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 15, N'Fixed Asset', N'grpFixedAsset', N'/FixedAsset/', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Report:: Fixed Asset' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 15, N'Report:: Fixed Asset', N'grpReportFixedAsset', N'/FixedAsset/Reports', 0, 1)
END
GO
IF EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Document Management' AND  [ModulePath]='/Inventory')
BEGIN
		UPDATE CommonModuleName
		SET [ModulePath]='/DocumentManagement'
		WHERE ModuleName = 'Document Management' AND  [ModulePath]='/Inventory'

END
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Document Management' )
BEGIN
	INSERT [dbo].[CommonModuleName] ([TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 16, N'Document Management', N'grpDocumentManagement', N'/DocumentManagement', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Report:: Document Management' )
BEGIN
	INSERT [dbo].[CommonModuleName] ([TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 16, N'Report:: Document Management', N'grpReportDocumentManagement', N'/DocumentManagement/Reports', 1, 1)END
GO
-- -- -- -- -- CommonSetup  ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SendSMS' AND SetupName = 'SendSMSConfiguration' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'SendSMS', N'SendSMSConfiguration', N'2', N'EzzyGroup~90<4V22c~ezzygroup~123456789', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'HowMuchPointNeedToGetOneMoney' AND SetupName = 'HowMuchPointNeedToGetOneMoney' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'HowMuchPointNeedToGetOneMoney', N'HowMuchPointNeedToGetOneMoney', N'2', N'HowMuchPointNeedToGetOneMoney', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects' AND SetupName = 'IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects', N'IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects', N'0', N'IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'ApprovalPolicyConfiguration' AND SetupName = 'ApprovalPolicyConfiguration' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'ApprovalPolicyConfiguration', N'ApprovalPolicyConfiguration', N'0', N'O FOR OR Configuration, 1 FOR AND Configuration', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsMembershipPaymentEnable' AND SetupName = 'IsMembershipPaymentEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsMembershipPaymentEnable', N'IsMembershipPaymentEnable', N'0', N'IsMembershipPaymentEnable', 1, CAST(N'2019-03-06T11:40:11.130' AS DateTime), null, null)
END

GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmployeeSalarySummaryPostingExpenseAccountHeadId' AND SetupName = 'EmployeeSalarySummaryPostingExpenseAccountHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmployeeSalarySummaryPostingExpenseAccountHeadId', N'EmployeeSalarySummaryPostingExpenseAccountHeadId', N'350', N'EmployeeSalarySummaryPostingExpenseAccountHeadId', 1, CAST(N'2019-01-23T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemSerialFillWithAutoSearch' AND SetupName = 'IsItemSerialFillWithAutoSearch' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemSerialFillWithAutoSearch', N'IsItemSerialFillWithAutoSearch', N'0', N'IsItemSerialFillWithAutoSearch', 1, CAST(N'2019-01-23T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'HowMuchMoneySpendToGetOnePoint' AND SetupName = 'HowMuchMoneySpendToGetOnePoint' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'HowMuchMoneySpendToGetOnePoint', N'HowMuchMoneySpendToGetOnePoint', N'25', N'HowMuchMoneySpendToGetOnePoint', 1, CAST(N'2019-02-13T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsSupplierCodeAutoGenerate' AND SetupName = 'IsSupplierCodeAutoGenerate' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsSupplierCodeAutoGenerate', N'IsSupplierCodeAutoGenerate', N'0', N'SC', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsSupplierUserPanalEnable' AND SetupName = 'IsSupplierUserPanalEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsSupplierUserPanalEnable', N'IsSupplierUserPanalEnable', N'0', N'SC', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAutoNightAuditAndApprovalProcessEnable' AND SetupName = 'IsAutoNightAuditAndApprovalProcessEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAutoNightAuditAndApprovalProcessEnable', N'IsAutoNightAuditAndApprovalProcessEnable', N'0', N'SC', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBanquetIntegrateWithAccounts' AND SetupName = 'IsBanquetIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBanquetIntegrateWithAccounts', N'IsBanquetIntegrateWithAccounts', N'0', N'IsBanquetIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmailAutoPosting' AND SetupName = 'IsRoomReservationEmailAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmailAutoPosting', N'IsRoomReservationEmailAutoPostingEnable', N'0', N'IsRoomReservationEmailAutoPostingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmailAutoPosting' AND SetupName = 'IsRoomRegistrationEmailAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmailAutoPosting', N'IsRoomRegistrationEmailAutoPostingEnable', N'0', N'IsRoomRegistrationEmailAutoPostingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmailAutoPosting' AND SetupName = 'IsCheckOutEmailAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmailAutoPosting', N'IsCheckOutEmailAutoPostingEnable', N'0', N'IsCheckOutEmailAutoPostingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmailAutoPosting' AND SetupName = 'IsBanquetReservationEmailAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmailAutoPosting', N'IsBanquetReservationEmailAutoPostingEnable', N'0', N'IsBanquetReservationEmailAutoPostingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'FrontOfficeMasterInvoiceTemplate' AND SetupName = 'IsBillSummaryPartWillHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'FrontOfficeMasterInvoiceTemplate', N'IsBillSummaryPartWillHide', N'0', N'IsBillSummaryPartWillHide', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'AdvancePaymentAdjustmentAccountsHeadId' AND SetupName = 'AdvancePaymentAdjustmentAccountsHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'AdvancePaymentAdjustmentAccountsHeadId', N'AdvancePaymentAdjustmentAccountsHeadId', N'83', N'AdvancePaymentAdjustmentAccountsHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'HoldUpAccountsPostingAccountReceivableHeadId' AND SetupName = 'HoldUpAccountsPostingAccountReceivableHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'HoldUpAccountsPostingAccountReceivableHeadId', N'HoldUpAccountsPostingAccountReceivableHeadId', N'0', N'HoldUpAccountsPostingAccountReceivableHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DirectorDividendNodeId' AND SetupName = 'DirectorDividendNodeId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DirectorDividendNodeId', N'DirectorDividendNodeId', N'0', N'DirectorDividendNodeId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillLockAndPreviewEnableForCheckOut' AND SetupName = 'IsBillLockAndPreviewEnableForCheckOut' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillLockAndPreviewEnableForCheckOut', N'IsBillLockAndPreviewEnableForCheckOut', N'1', N'IsBillLockAndPreviewEnableForCheckOut', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
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
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRestaurantWaiterConfirmationEnable' AND SetupName = 'IsRestaurantWaiterConfirmationEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsRestaurantWaiterConfirmationEnable', N'IsRestaurantWaiterConfirmationEnable', N'30', N'Sync Data Day Range', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsInventoryIntegratationWithAccountsAutomated' AND SetupName = 'IsInventoryIntegratationWithAccountsAutomated' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsInventoryIntegratationWithAccountsAutomated', N'IsInventoryIntegratationWithAccountsAutomated', N'0', N'Is Inventory Integratation With Accounts Automated', 1, CAST(N'2019-01-18T16:31:11.130' AS DateTime), 1, CAST(N'2019-01-18T16:31:11.130' AS DateTime))
END

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemOriginHide' AND SetupName = 'IsItemOriginHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemOriginHide', N'IsItemOriginHide', N'0', N'Is Item Origin Hide', 1, CAST(N'2019-01-18T16:31:11.130' AS DateTime), 1, CAST(N'2019-01-18T16:31:11.130' AS DateTime))
END

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsFrontOfficeIntegrateWithAccounts' AND SetupName = 'IsFrontOfficeIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsFrontOfficeIntegrateWithAccounts', N'IsFrontOfficeIntegrateWithAccounts', N'0', N'IsFrontOfficeIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPOSIntegrateWithAccounts' AND SetupName = 'IsPOSIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPOSIntegrateWithAccounts', N'IsPOSIntegrateWithAccounts', N'0', N'IsPOSIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsInventoryIntegrateWithAccounts' AND SetupName = 'IsInventoryIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsInventoryIntegrateWithAccounts', N'IsInventoryIntegrateWithAccounts', N'0', N'IsInventoryIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollIntegrateWithAccounts' AND SetupName = 'IsPayrollIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollIntegrateWithAccounts', N'IsPayrollIntegrateWithAccounts', N'0', N'IsPayrollIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsFoodNBeverageSalesRelatedDataHide' AND SetupName = 'IsFoodNBeverageSalesRelatedDataHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsFoodNBeverageSalesRelatedDataHide', N'IsFoodNBeverageSalesRelatedDataHide', N'0', N'Is Food And Beverage Sales Related Data Hide', 1, CAST(N'2019-01-31T11:40:11.130' AS DateTime), 0, CAST(N'2019-01-31T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsTransferProductReceiveDisable' AND SetupName = 'IsTransferProductReceiveDisable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsTransferProductReceiveDisable', N'IsTransferProductReceiveDisable', N'0', N'Is Transfer Product Receive Enable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemDescriptionSuggestInPurchaseOrder' AND SetupName = 'IsItemDescriptionSuggestInPurchaseOrder' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemDescriptionSuggestInPurchaseOrder', N'IsItemDescriptionSuggestInPurchaseOrder', N'0', N'Is Item Description Suggest In Purchase Order', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'TaxDeductedAtSourceByCustomerAccountHeadId' AND SetupName = 'TaxDeductedAtSourceByCustomerAccountHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'TaxDeductedAtSourceByCustomerAccountHeadId', N'TaxDeductedAtSourceByCustomerAccountHeadId', N'0', N'TaxDeductedAtSourceByCustomerAccountHeadId', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsMinimumRoomRateCheckingForRoomTypeEnable' AND SetupName = 'IsMinimumRoomRateCheckingForRoomTypeEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsMinimumRoomRateCheckingForRoomTypeEnable', N'IsMinimumRoomRateCheckingForRoomTypeEnable', N'0', N'Is Minimum RoomRate Checking For RoomType Enable', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts' AND SetupName = 'IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts', N'IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts', N'1', N'Is Minimum RoomRate Checking For RoomType Enable', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsDealStageCanChangeMoreThanOneStep' AND SetupName = 'IsDealStageCanChangeMoreThanOneStep' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsDealStageCanChangeMoreThanOneStep', N'IsDealStageCanChangeMoreThanOneStep', N'0', N'Is Minimum RoomRate Checking For RoomType Enable', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsQuotationCreateFromSiteServeyFeedback' AND SetupName = 'IsQuotationCreateFromSiteServeyFeedback' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsQuotationCreateFromSiteServeyFeedback', N'IsQuotationCreateFromSiteServeyFeedback', N'1', N'Is Minimum RoomRate Checking For RoomType Enable', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsLifeCycleStageCanChangeMoreThanOneStep' AND SetupName = 'IsLifeCycleStageCanChangeMoreThanOneStep' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsLifeCycleStageCanChangeMoreThanOneStep', N'IsLifeCycleStageCanChangeMoreThanOneStep', N'0', N'Is Minimum RoomRate Checking For RoomType Enable', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SalesCompanyNumberPrefix' AND SetupName = 'SalesCompanyNumberPrefix' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SalesCompanyNumberPrefix', N'SalesCompanyNumberPrefix', N'CMN', N'Sales CompanyNumber Prefix', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SalesContactNumberPrefix' AND SetupName = 'SalesContactNumberPrefix' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SalesContactNumberPrefix', N'SalesContactNumberPrefix', N'CTN', N'Sales ContactNumber Prefix', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SalesDealNumberPrefix' AND SetupName = 'SalesDealNumberPrefix' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SalesDealNumberPrefix', N'SalesDealNumberPrefix', N'DLN', N'Sales DealNumber Prefix', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsHotelGuestCompanyRestrictionForAllUsers' AND SetupName = 'IsHotelGuestCompanyRestrictionForAllUsers' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsHotelGuestCompanyRestrictionForAllUsers', N'IsHotelGuestCompanyRestrictionForAllUsers', N'1', N'Is Hotel Guest Company Restriction For All Users', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsContactInformationRestrictedForAllUsers' AND SetupName = 'IsContactInformationRestrictedForAllUsers' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsContactInformationRestrictedForAllUsers', N'IsContactInformationRestrictedForAllUsers', N'1', N'Is Contact Information Restricted For All Users', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsDealRestrictedForAllUsers' AND SetupName = 'IsDealRestrictedForAllUsers' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsDealRestrictedForAllUsers', N'IsDealRestrictedForAllUsers', N'1', N'Is Deal Restricted For All Users', 1, CAST(N'2019-06-28T12:56:51.423' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollApprovalConfig' AND SetupName = 'PayrollApprovalConfig' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollApprovalConfig', N'PayrollApprovalConfig', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LCInformationConfig' AND SetupName = 'LCInformationConfig' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'LCInformationConfig', N'LCInformationConfig', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsShowReferenceVoucherNumber' AND SetupName = 'IsShowReferenceVoucherNumber' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsShowReferenceVoucherNumber', N'IsShowReferenceVoucherNumber', N'0', N'Is Show Reference Voucher Number', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DataSynchronizationIPAddress' AND SetupName = 'DataSynchronizationIPAddress' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DataSynchronizationIPAddress', N'DataSynchronizationIPAddress', N'192.168.88.135', N'Data Synchronization IP Address', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollProvidentFundDeductHide' AND SetupName = 'IsPayrollProvidentFundDeductHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollProvidentFundDeductHide', N'IsPayrollProvidentFundDeductHide', N'2', N'IsPayrollProvidentFundDeductHide', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'CashRequisition' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'CashRequisition', N'Cash Requisition Information', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'CashRequisitionApprovalConfig' AND SetupName = 'CashRequisitionApprovalConfig' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'CashRequisitionApprovalConfig', N'CashRequisitionApprovalConfig', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'LCInformationConfig' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'LCInformationConfig', N'LC Information', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LCInformationConfig' AND SetupName = 'LCInformationConfig' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'LCInformationConfig', N'LCInformationConfig', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'LCApproval' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'LCApproval', N'LC Approval', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'OverHeadApproval' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'OverHeadApproval', N'LC Over Head Expense Approval', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'CNFApproval' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'CNFApproval', N'LC CNF Transaction Approval', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'VMOverHeadApproval' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'VMOverHeadApproval', N'VM Over Head Expense Approval', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LCApproval' AND SetupName = 'LCApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'LCApproval', N'LCApproval', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'OverHeadApproval' AND SetupName = 'OverHeadApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'OverHeadApproval', N'OverHeadApproval', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'CNFApproval' AND SetupName = 'CNFApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'CNFApproval', N'CNFApproval', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'VMOverHeadApproval' AND SetupName = 'VMOverHeadApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'VMOverHeadApproval', N'VMOverHeadApproval', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollCostCenterDivHide' AND SetupName = 'IsPayrollCostCenterDivHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollCostCenterDivHide', N'IsPayrollCostCenterDivHide', N'1', N'IsMembershipPaymentEnable', 1, CAST(N'2019-03-06T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsFORoomBillAutoProcessStop' AND SetupName = 'IsFORoomBillAutoProcessStop' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsFORoomBillAutoProcessStop', N'IsFORoomBillAutoProcessStop', N'0', N'IsFORoomBillAutoProcessStop', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsSalesNoteEnable' AND SetupName = 'IsSalesNoteEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsSalesNoteEnable', N'IsSalesNoteEnable', N'0', N'Is Sales Note Enable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'GLVoucherApprovalConfig' AND SetupName = 'GLVoucherApprovalConfig' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'GLVoucherApprovalConfig', N'GLVoucherApprovalConfig', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsInvCategoryCodeAutoGenerate' AND SetupName = 'IsInvCategoryCodeAutoGenerate' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsInvCategoryCodeAutoGenerate', N'IsInvCategoryCodeAutoGenerate', N'0', N'IsInvCategoryCodeAutoGenerate', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsEmployeeCodeAutoGenerate' AND SetupName = 'IsEmployeeCodeAutoGenerate' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsEmployeeCodeAutoGenerate', N'IsEmployeeCodeAutoGenerate', N'0', N'IsEmployeeCodeAutoGenerate', 0, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsSDCIntegrationEnable' AND SetupName = 'IsSDCIntegrationEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsSDCIntegrationEnable', N'IsSDCIntegrationEnable', N'0', N'Is SDC Integration Enable', 0, CAST(N'2019-03-06T11:40:11.130' AS DateTime), null, null)
END

GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'Restaurant' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'Restaurant', N'Restaurant', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'GuestRoom' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'GuestRoom', N'Guest Room', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'Banquet' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'Banquet', N'Banquet', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'ServiceOutlet' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'ServiceOutlet', N'Service Outlet', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'Item' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'Item', N'Item Information', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CRMQuotaionServiceType' AND FieldValue = 'Service' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CRMQuotaionServiceType', N'Service', N'Service Information', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsVehicleManagementIntegratedwithAccounts' AND SetupName = 'IsVehicleManagementIntegratedwithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsVehicleManagementIntegratedwithAccounts', N'IsVehicleManagementIntegratedwithAccounts', N'1', N'IsVehicleManagementIntegratedwithAccounts', 1, CAST(N'2019-03-06T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsDealIntegrateWithAccounts' AND SetupName = 'IsDealIntegrateWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsDealIntegrateWithAccounts', N'IsDealIntegrateWithAccounts', N'0', N'IsDealIntegrateWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsEmployeeCanEditDetailsInfo' AND SetupName = 'IsEmployeeCanEditDetailsInfo' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsEmployeeCanEditDetailsInfo', N'IsEmployeeCanEditDetailsInfo', N'0', N'IsEmployeeCanEditDetailsInfo', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollEmpBalanceTransferApproval' AND SetupName = 'PayrollEmpBalanceTransferApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'PayrollEmpBalanceTransferApproval', N'PayrollEmpBalanceTransferApproval', N'0', N'Payroll Employee Balance Transfer Approval', 1, CAST(N'2020-01-16T11:40:11.130' AS DateTime), 0, CAST(N'2020-01-16T17:57:09.330' AS DateTime))
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollEmpBalanceTransferApproval')
BEGIN
	UPDATE CommonSetup 
	SET [Description] = 'Payroll Employee Balance Transfer Approval' 
	WHERE TypeName = 'PayrollEmpBalanceTransferApproval' AND SetupName = 'PayrollEmpBalanceTransferApproval'
END

IF EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'SupportNTicket ' )
BEGIN
 UPDATE CommonCustomFieldData 
		SET FieldValue = 'SupportNTicket'
		WHERE FieldType = 'TaskType' AND FieldValue = 'SupportNTicket '
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'SupportNTicket' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'SupportNTicket', N'Support & Ticket ', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'CRM' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'CRM', N'CRM', 1)
END
GO
-- -- -- -- -- Update --------------

IF EXISTS (SELECT LevelId  FROM PayrollEducationLevel WHERE LevelName = 'Secondary')
BEGIN
	UPDATE PayrollEducationLevel
	SET LevelName = 'School Secondary' 
	WHERE LevelName = 'Secondary'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollPFEmployerContributionId' AND SetupName = 'PayrollPFEmployerContributionId' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = 'Basic',
	TypeName = 'PFCompanyContributionOn',
	SetupName = 'PFCompanyContributionOn'
	WHERE TypeName = 'PayrollPFEmployerContributionId' AND SetupName = 'PayrollPFEmployerContributionId'
END
GO

IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsProductOutApprovalEnable' AND SetupName = 'IsProductOutApprovalEnable' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsProductOutApprovalEnable' AND SetupName = 'IsProductOutApprovalEnable'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPurchaseOrderApprovalEnable' AND SetupName = 'IsPurchaseOrderApprovalEnable' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsPurchaseOrderApprovalEnable' AND SetupName = 'IsPurchaseOrderApprovalEnable' 
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsReceivedProductApprovalEnable' AND SetupName = 'IsReceivedProductApprovalEnable' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsReceivedProductApprovalEnable' AND SetupName = 'IsReceivedProductApprovalEnable'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsProductOutReceiveApprovalEnable' AND SetupName = 'IsProductOutReceiveApprovalEnable' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsProductOutReceiveApprovalEnable' AND SetupName = 'IsProductOutReceiveApprovalEnable'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRequisitionApprovalEnable' AND SetupName = 'IsRequisitionApprovalEnable' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsRequisitionApprovalEnable' AND SetupName = 'IsRequisitionApprovalEnable'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'hfIsBanquetBillAmountWillRound' AND SetupName = 'hfIsBanquetBillAmountWillRound' )
BEGIN
	UPDATE CommonSetup
	SET TypeName = 'IsBanquetBillAmountWillRound',
		SetupName = 'IsBanquetBillAmountWillRound',
		Description = 'IsBanquetBillAmountWillRound'
	WHERE TypeName = 'hfIsBanquetBillAmountWillRound' AND SetupName = 'hfIsBanquetBillAmountWillRound'
END

IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCheckedAndApprovedBySamePerson' AND SetupName = 'IsCheckedAndApprovedBySamePerson' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '0',
	    TypeName = 'InventoryTransactionSetup',
		SetupName = 'InventoryTransactionSetup',
		Description = '0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both' 
	WHERE TypeName = 'IsCheckedAndApprovedBySamePerson' AND SetupName = 'IsCheckedAndApprovedBySamePerson'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'InventoryTransactionSetup' AND SetupName = 'InventoryTransactionSetup' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'InventoryTransactionSetup', N'InventoryTransactionSetup', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsInventoryReportItemCostRescitionForNonAdminUsers' AND SetupName = 'IsInventoryReportItemCostRescitionForNonAdminUsers' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsInventoryReportItemCostRescitionForNonAdminUsers', N'IsInventoryReportItemCostRescitionForNonAdminUsers', N'0', N'IsInventoryReportItemCostRescitionForNonAdminUsers', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollIntegrateWithFrontOffice' AND SetupName = 'IsPayrollIntegrateWithFrontOffice' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsPayrollIntegrateWithFrontOffice', N'IsPayrollIntegrateWithFrontOffice', N'0', N'1 = Load Employee, 0 = Not Load Employee', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBanquetReservationRestictionForAllUser ' AND SetupName = 'IsBanquetReservationRestictionForAllUser ' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsBanquetReservationRestictionForAllUser ', N'IsBanquetReservationRestictionForAllUser ', N'0', N'1 = Not Load All Search, 0 = Load All Search', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRequisitionCheckedByEnable' AND SetupName = 'IsRequisitionCheckedByEnable' AND SetupValue LIKE '0' )
BEGIN
	UPDATE CommonSetup
	SET SetupValue = '1'
	WHERE TypeName = 'IsRequisitionCheckedByEnable' AND SetupName = 'IsRequisitionCheckedByEnable'
END
GO
-- -- -- -- -- CommonCustomFieldData ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ReportType' AND FieldValue = 'Hotel Revenue' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'ReportType', N'Hotel Revenue', N'Hotel Revenue', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'GeneralLedger' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'GeneralLedger', N'General Ledger', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Requisition' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Requisition', N'Item Requisition', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Local Purchase' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Local Purchase', N'Local Purchase Order', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Foreign Purchase' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Foreign Purchase', N'Foreign Purchase Order', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Item Consumption' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Item Consumption', N'Item Consumption', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Gate Pass' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Gate Pass', N'Gate Pass', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Receive' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Receive', N'Item Receive', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'ItemTransfer' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'ItemTransfer', N'Item Transfer', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'TransferReturn' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'TransferReturn', N'Transfer Item Return', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'ReceivedItemReturn' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'ReceivedItemReturn', N'Receive Item Return', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'FixedAssetTransferNReturnApproval' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'FixedAssetTransferNReturnApproval', N'Fixed Asset Transfer & Return Approval', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'Mr.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'Mr.', N'Mr.', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'Mrs.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'Mrs.', N'Mrs.', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'Miss.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'Miss.', N'Ms.', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'MrNMrs.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'MrNMrs.', N'Mr. & Mrs.', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'Dr.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'Dr.', N'Dr.', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'N/A' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'N/A', N'Not Applicable', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GuestTitle' AND FieldValue = 'Mr.' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'GuestTitle', N'Mr.', N'Mr.', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'Relationship' AND FieldValue = 'Spouse' )
BEGIN
INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'Relationship', N'Spouse', NULL, 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'Relationship' AND FieldValue = 'Son' )
BEGIN
INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'Relationship', N'Son', NULL, 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'Relationship' AND FieldValue = 'Daughter' )
BEGIN
INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'Relationship', N'Daughter', NULL, 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Leave' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Leave', N'Leave Information', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'LateAttendence' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'LateAttendence', N'Late Attendence', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'FrontOfficeInvoiceRoomServiceName' AND FieldValue = 'Default' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'FrontOfficeInvoiceRoomServiceName', N'Default', N'Default', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'FrontOfficeInvoiceRoomServiceName' AND FieldValue = 'RoomTariff' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'FrontOfficeInvoiceRoomServiceName', N'RoomTariff', N'Room Tariff', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'FrontOfficeInvoiceRoomServiceName' AND FieldValue = 'Default' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES (N'FrontOfficeInvoiceRoomServiceName', N'RoomRent', N'Room Rent', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BanquetEventType' AND FieldValue = 'Internal' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'BanquetEventType', N'Internal', N'Internal', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BanquetEventType' AND FieldValue = 'Rental' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'BanquetEventType', N'Rental', N'Rental', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabel' AND FieldValue = 'Home' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabel', N'Home', N'Home', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabel' AND FieldValue = 'Work' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabel', N'Work', N'Work', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabel' AND FieldValue = 'Other' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabel', N'Other', N'Other', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'Facebook' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'Facebook', N'Facebook', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'Skype' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'Skype', N'Skype', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'WhatsApp' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'WhatsApp', N'WhatsApp', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'Viber' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'Viber', N'Viber', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'Instagram' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'Instagram', N'Instagram', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ContactDetailsLabelSocialMedia' AND FieldValue = 'Other' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'ContactDetailsLabelSocialMedia', N'Other', N'Other', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'PayrollEmpBalanceTransferApproval' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'InnboardFeatures', N'PayrollEmpBalanceTransferApproval', N'Payroll Employee Balance Transfer Approval', 1)
END
GO
------------------------------- Delete -------------------------------------------------------------------------------------------------
IF EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Payroll' )
BEGIN
 DELETE FROM CommonCustomFieldData WHERE FieldValue = 'Payroll'
END
GO
IF EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'General Ledger' )
BEGIN
 DELETE FROM CommonCustomFieldData WHERE FieldValue = 'General Ledger'
END
GO
-- -- -- -- -- HotelRoomReservationRoomDetail  ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM HotelRoomReservationRoomDetail)
BEGIN	
	DECLARE @ProcessId INT
	DECLARE @getProcessId CURSOR
	SET @getProcessId = CURSOR FOR
	
	SELECT Id
	FROM HotelRoomReservationSummary
	
	OPEN @getProcessId
	FETCH NEXT
	FROM @getProcessId INTO @ProcessId
	WHILE @@FETCH_STATUS = 0
	BEGIN	
		DECLARE @RoomQuantity INT = 0, @InnerRow INT = 1, @InnerCount INT = 0	
		SELECT @RoomQuantity = RoomQuantity FROM HotelRoomReservationSummary WHERE Id = @ProcessId
		
		if(@roomQuantity > 1)
		BEGIN
			SET @InnerRow = 1
			SET @InnerCount = @RoomQuantity
		END
		ELSE 
		BEGIN
			INSERT INTO HotelRoomReservationRoomDetail(ReservationId, RoomTypeId, RoomQuantity)
			SELECT ReservationId, RoomTypeId, 1 FROM HotelRoomReservationSummary WHERE Id = @ProcessId
		END

		WHILE(@InnerRow <= @InnerCount)
		BEGIN		
			INSERT INTO HotelRoomReservationRoomDetail(ReservationId, RoomTypeId, RoomQuantity)
			SELECT ReservationId, RoomTypeId, 1 FROM HotelRoomReservationSummary WHERE Id = @ProcessId
			SET @InnerRow = @InnerRow + 1
		END
	FETCH NEXT
	FROM @getProcessId INTO @ProcessId
	END
	CLOSE @getProcessId
	DEALLOCATE @getProcessId
END
GO
-- -- -- -- -- CommonPaymentMode  ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------
GO
IF NOT EXISTS(SELECT * FROM CommonPaymentMode WHERE PaymentMode = 'Rounded')
BEGIN
	INSERT [dbo].[CommonPaymentMode] ([AncestorId], [PaymentMode], [DisplayName], [PaymentCode], [Hierarchy], [Lvl], [HierarchyIndex], [PaymentAccountsPostingId], [ReceiveAccountsPostingId], [ActiveStat]) VALUES (18, N'Rounded', N'Rounded', N'rd', N'.18.', 0, N'.00018.', 31, 31, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonPaymentMode WHERE PaymentMode = 'Rebate')
BEGIN
	INSERT [dbo].[CommonPaymentMode] ([AncestorId], [PaymentMode], [DisplayName], [PaymentCode], [Hierarchy], [Lvl], [HierarchyIndex], [PaymentAccountsPostingId], [ReceiveAccountsPostingId], [ActiveStat]) VALUES (19, N'Rebate', N'Rebate', N'rb', N'.19.', 0, N'.00019.', 31, 31, 1)
END
-- -- -- -- -- SecurityMenuLinks  ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmItemClassification')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmItemClassification', N'Item Classification', N'Item Classification', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportCanceledReservation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (5, N'frmReportCanceledReservation', N'Canceled Reservation Info', N'Canceled Reservation Info', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(0x0000A93D0101DF0E AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LCInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (14, N'LCInformation', N'LC Information', N'LC Information', N'aspx', N'/LCManagement', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), 1, CAST(0x0000A78800F59794 AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBudget')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'frmBudget', N'Budget Information', N'Budget Information', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSupplierCompanyBalanceTransfer')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'frmSupplierCompanyBalanceTransfer', N'Supplier Company Balance Transfer', N'Supplier Company Balance Transfer', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBudgetApproval')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'frmBudgetApproval', N'Budget Approval', N'Budget Approval', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportRevenueStatement')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (3, N'frmReportRevenueStatement', N'P&L Statement', N'P&L Statement', N'aspx', N'/GeneralLedger/Reports', N'Report', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmDeviceEmployeeMapping')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (18, N'frmDeviceEmployeeMapping', N'Device Employee Mapping', N'Device Employee Mapping', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CashRequisitionOrBillVoucherInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (18, N'CashRequisitionOrBillVoucherInformation', N'Cash Requisition Or Bill Voucher Information', N'Cash Requisition Or Bill Voucher Information', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RoomFeatures')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'RoomFeatures', N'Room Features Head', N'Room Features Head', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(0x0000A96800DD53B5 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RoomFeaturesInfo')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'RoomFeaturesInfo', N'Room Features Information', N'Room Features Information', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(0x0000A96800DD78FB AS DateTime), 1, CAST(0x0000A96800DDF863 AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmUserConfiguration')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (21, N'frmUserConfiguration', N'Approval Configuration', N'Approval Configuration', N'aspx', N'/UserInformation', N'Page', NULL, 1, 1, CAST(N'2018-08-30T18:34:37.657' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmGatePass')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (30, N'frmGatePass', N'Gate Pass', N'Gate Pass', N'aspx', N'/Maintenance', N'Page', NULL, 1, 1, CAST(N'2018-10-02T14:15:51.720' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'GatePassInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (30, N'GatePassInformation', N'Gate Pass Information', N'Gate Pass Information', N'aspx', N'/Maintenance', N'Page', NULL, 1, 1, CAST(N'2018-10-02T14:24:13.480' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'GuestBirthdayNotification')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'GuestBirthdayNotification', N'Guest Birthday Notification', N'Guest Birthday Notification', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(N'2018-10-04T14:24:13.480' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSettlementSummaryReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (5, N'frmSettlementSummaryReport', N'Settlement Summary', N'Settlement Summary', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2018-10-09T18:36:50.313' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmManagerReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (5, N'frmManagerReport', N'Manager Report', N'Manager Report', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2018-10-09T18:36:50.313' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemSalesPopularityAnalysis')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (7, N'ItemSalesPopularityAnalysis', N'Item Sales Popularity Analysis', N'Item Sales Popularity Analysis', N'aspx', N'/POS/Reports', N'Report', NULL, 1, 1, CAST(N'2018-11-26T11:48:32.030' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CNFTransaction')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (14, N'CNFTransaction', N'CNF Transaction', N'CNF Transaction', N'aspx', N'/LCManagement', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), 1, CAST(0x0000A78800C66332 AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ReportLCOverHeadExpense')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 15, N'ReportLCOverHeadExpense', N'LC OverHead Expense Information', N'LC OverHead Expense Information', N'aspx', N'/LCManagement/Reports', N'Report', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportCnfLedger')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 15, N'frmReportCnfLedger', N'Cnf Ledger Information', N'Cnf Ledger Information', N'aspx', N'/LCManagement/Reports', N'Report', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPurchaseReturnReport') 
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (13, N'frmPurchaseReturnReport', N'Purchase Return New', N'Purchase Return Report', N'aspx', N'/PurchaseManagment/Reports', N'Report', NULL, 1, 1, CAST(N'2018-12-12T13:41:57.867' AS DateTime), 1, CAST(N'2018-12-13T15:22:08.907' AS DateTime))

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'AttendenceInformation') 
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (18, N'AttendenceInformation', N'Attendence Information', N'Attendence Information', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2018-12-12T13:41:57.867' AS DateTime), 1, CAST(N'2018-12-13T15:22:08.907' AS DateTime))
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'AttendenceInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET 
	[PageName] = 'Attendance Information',
	[PageDisplayCaption] = 'Attendance Information'
	WHERE PageId = 'AttendenceInformation'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmAppraisalEvaluationBy')
BEGIN
	UPDATE SecurityMenuLinks
	SET 
	PageId = 'AppraisalEvaluationBy'
	WHERE PageId = 'frmAppraisalEvaluationBy'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'BillDetailsReport')
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (7, N'BillDetailsReport', N'Bill Details Report', N'Bill Details', N'aspx', N'/POS/Reports', N'Report', NULL, 1, 1, CAST(N'2019-01-07T16:49:43.080' AS DateTime), NULL, NULL)
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PayrollEmployeeType')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (18, N'PayrollEmployeeType', N'Payroll Employee Type', N'Payroll Employee Type',N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2019-01-16' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'EmployeeDashboard')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (18, N'EmployeeDashboard', N'Employee Dashboard', N'Employee Dashboard',N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2019-07-22' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'EmployeeWorkStation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (18, N'EmployeeWorkStation', N'Employee Work Station', N'Employee Work Station',N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2019-07-22' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RetailPosItemReturnToStoreInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (6, N'RetailPosItemReturnToStoreInformation', N'Item Return To Store Information', N'Return To Store Information',N'aspx', N'/POS', N'Page', NULL, 1, 1, CAST(N'2019-01-21' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DiscountSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 1, N'DiscountSetup', N'Discount Setup', N'Discount Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-02-13T15:20:21.883' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CommonConfiguration')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 1, N'CommonConfiguration', N'Common Configuration', N'Common Configuration', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-12-17T15:20:21.883' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SignupStatusSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (22, N'SignupStatusSetup', N'Sales Company Signup Status Setup', N'Sales Company Signup Status Setup', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-04T12:47:39.633' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'OnlineMembership')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES 
	(26, N'OnlineMembership', N'Online Membership', N'Online Membership', N'aspx', N'/Membership', N'Page', NULL, 1, 1, CAST(N'2019-03-04T12:47:39.633' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmMembershipPublicAdmin')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES 
	(26, N'frmMembershipPublicAdmin', N'Online Membership Public Admin', N'Online Membership Public Admin', N'aspx', N'/Membership', N'Page', NULL, 1, 1, CAST(N'2019-03-07T17:40:15.763' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'MembershipPublicReport')
BEGIN
    INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
	VALUES (27, N'MembershipPublicReport', N'Membership Public Report', N'Membership Public Report', N'aspx', N'/Membership/Reports', N'Report', NULL, 1, 1, CAST(N'2019-03-25T17:55:55.827' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealCreation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'DealCreation', N'Deal Creation', N'Deal Creation', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-05T15:39:34.887' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealStageWiseCompanyStatus')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'DealStageWiseCompanyStatus', N'Deal Stage Wise Company Status', N'Deal Creation', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-05T15:39:34.887' AS DateTime), NULL, NULL)
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmContactCreation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ContactCreation',
	PageName = 'Contact Information',
	PageDisplayCaption = 'Contact Information'
	WHERE PageId = 'frmContactCreation'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportCompanyLedger')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 23,
	PagePath = '/SalesAndMarketing/Reports'
	WHERE PageId = 'frmReportCompanyLedger'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContactCreation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'ContactCreation', N'Contact Information', N'Contact Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-05T15:39:34.887' AS DateTime), NULL, NULL)
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContactDetails')
BEGIN
	DELETE FROM SecurityMenuLinks WHERE PageId = 'ContactDetails'
END
GO


IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'AssignTask' AND ModuleId = 18)
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 32,
	PageName = 'Task Assignment' ,
	PagePath = '/TaskManagement'	
	WHERE PageId = 'AssignTask'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'AssignTask')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 32, N'AssignTask', N'Task Assignment', N'Task Assignment', N'aspx', N'/TaskManagement', N'Page', NULL, 1, 1, CAST(N'2019-03-07T18:11:38.657' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'GanttChartInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 32, N'GanttChartInformation', N'Gantt Chart Information', N'Gantt Chart Information', N'aspx', N'/TaskManagement', N'Page', NULL, 1, 1, CAST(N'2019-03-07T18:11:38.657' AS DateTime), NULL, NULL)
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'AssignTask' AND ModuleId = 32 AND PagePath = '/TaskManagement/')
BEGIN
	UPDATE SecurityMenuLinks
	SET
	PagePath = '/TaskManagement'	
	WHERE PageId = 'AssignTask'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ReportAssignTask')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (33, N'ReportAssignTask', N'Task Information', N'Task Information', N'aspx', N'/TaskManagement/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'Depreciation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 34, N'Depreciation', N'Fixed Asset', N'Fixed Asset', N'aspx', N'/FixedAsset', N'Page', NULL, 1, 1, CAST(N'2019-03-07T18:11:38.657' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'FixedAssetTransfer')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 34, N'FixedAssetTransfer', N'Fixed Asset Transfer', N'Fixed Asset Transfer', N'aspx', N'/FixedAsset', N'Page', NULL, 1, 1, CAST(N'2019-03-07T18:11:38.657' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'FixedAssetReturn')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 34, N'FixedAssetReturn', N'Fixed Asset Return', N'Fixed Asset Return', N'aspx', N'/FixedAsset', N'Page', NULL, 1, 1, CAST(N'2019-03-07T18:11:38.657' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SMQuotationSearch')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (22, N'SMQuotationSearch', N'Quotation Search', N'Quotation Search', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-14T11:44:50.753' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmQuotationWiseSalesReport')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (23, N'frmQuotationWiseSalesReport', N'Quotation Wise Sales Report', N'Quotation Wise Sales Report', N'aspx', N'/SalesAndMarketing/Reports', N'Page', NULL, 1, 1, CAST(N'2019-03-20T11:44:50.753' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SalesItemTransfer')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SalesItemTransfer', N'Sales Item Transfer', N'Sales Item Transfer', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-29T15:39:34.887' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SalesBill')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SalesBill', N'Sales Bill', N'Sales Bill', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-04-03T11:14:46.800' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SalesNote')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SalesNote', N'Sales Note', N'Sales Note', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-04-04T12:31:29.947' AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'StageSetup')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'DealStage',
	[PageName] = 'Deal Stage',
	[PageDisplayCaption] = 'Deal Stage'
	WHERE PageId = 'StageSetup'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealStage')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'DealStage', N'Deal Stage', N'Deal Stage', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LifeCycleStage')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'LifeCycleStage', N'Life Cycle Stage', N'Life Cycle Stage', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SourceInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SourceInformation', N'Source Information', N'Source Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CompanyTypeInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'CompanyTypeInformation', N'Company Type Information', N'Company Type Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'OwnershipInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'OwnershipInformation', N'Ownership Information', N'Ownership Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealProbabilityStageInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'DealProbabilityStageInformation', N'Deal Probability Stage Information', N'Deal Probability Stage Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SegmentInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SegmentInformation', N'Segment Information', N'Segment Information', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CRMConfiguration')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'CRMConfiguration', N'Sales And Marketing Configuration', N'Sales And Marketing Configuration', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContactTransfer')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'ContactTransfer', N'Contact Transfer', N'Contact Transfer', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-21T13:42:11.777' AS DateTime), NULL, NULL)
END
GO


IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealStageWiseDashboard')
BEGIN
	DELETE FROM SecurityMenuLinks WHERE PageId = 'DealStageWiseDashboard'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SiteSurveyFeedbackForTechnicalDepartment')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'SiteSurveyFeedbackForTechnicalDepartment', N'Site Survey Feedback', N'Site Survey Feedback', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-07-12T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DealImplementationFeedback')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'DealImplementationFeedback', N'Deal Implementation Feedback', N'Deal Implementation Feedback', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-07-12T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LogActivityInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'LogActivityInformation', N'Log Activity', N'Log Activity', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-07-12T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CallToActionInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'CallToActionInformation', N'Call To Action', N'Call To Action', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-07-12T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'TaskStage' AND [PagePath]='/Payroll')
BEGIN
	UPDATE SecurityMenuLinks
	SET
	[PagePath] ='/TaskManagement'
	WHERE PageId = 'TaskStage'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'TaskStage')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (32, N'TaskStage', N'Task Stage Information', N'Task Stage Information',N'aspx', N'/TaskManagement', N'Page', NULL, 1, 1, CAST(N'2019-01-16' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CustomNotice')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (1, N'CustomNotice', N'Custom Notice', N'Custom Notice', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-08-06T20:03:07.697' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'OpeningBalance')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'OpeningBalance', N'Opening Balance', N'Opening Balance', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DataSynchronization')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'DataSynchronization', N'Data Synchronization', N'Data Synchronization', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContributionAnalysisReport')
BEGIN
 INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
 VALUES (5, N'ContributionAnalysisReport', N'Contribution Analysis', N'Contribution Analysis', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-09-01T12:46:43.100' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContributionAnalysisReport')
BEGIN
 INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
 VALUES (1, N'PageWiseActivityLogDetailsSetup', N'Activity Log Details Setup', N'Activity Log Details Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-09-06T12:39:50.957' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PackageInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 10, N'PackageInformation', N'Package Information', N'Package Information', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2019-09-11T15:11:24.767' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PriceMatrix')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 10, N'PriceMatrix', N'Price Matrix', N'Price Matrix', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2019-09-12T16:56:30.427' AS DateTime), NULL, NULL)
END
GO

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'InvServiceBandwidth')
BEGIN
   INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
   VALUES (10, N'InvServiceBandwidth', N'Service Bandwidth', N'Service Bandwidth', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2019-09-12T17:40:13.550' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ServiceBandwidthFrequency')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 10, N'ServiceBandwidthFrequency', N'Bandwidth Frequency', N'Bandwidth Frequency', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2019-09-26T14:01:30.063' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DocumentManagement')
BEGIN
   INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
   VALUES (36, N'DocumentManagement', N'Document Management', N'Document Management', N'aspx', N'/DocumentManagement', N'Page', NULL, 1, 1, CAST(N'2019-09-12T17:40:13.550' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DMConfiguration')
BEGIN
   INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
   VALUES (36, N'DMConfiguration', N'Document Configuration', N'Document Configuration', N'aspx', N'/DocumentManagement', N'Page', NULL, 1, 1, CAST(N'2019-09-12T17:40:13.550' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'rptDocumentManagementReport')
BEGIN
   INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
   VALUES (37, N'rptDocumentManagementReport', N'Document Management Report', N'Document Management Report', N'aspx', N'/DocumentManagement/Reports', N'Page', NULL, 1, 1, CAST(N'2019-09-12T17:40:13.550' AS DateTime), NULL, NULL)
END
GO

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LcCNFList')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (14, N'LcCNFList', N'CNF List', N'CNF List', N'aspx', N'/LCManagement', N'Page', NULL, 1, 1, CAST(N'2019-10-11T16:19:41.600' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmDivisionList')
BEGIN
  INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
  VALUES (18, N'frmDivisionList', N'Division Information', N'Division Information', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2016-04-25T21:45:25.640' AS DateTime), 1, CAST(N'2019-10-29T15:39:19.520' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RateChartInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 1, N'RateChartInformation', N'Rate Chart Information', N'Rate Chart Information', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-02-13T15:20:21.883' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ReportHKLostNFound')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES (29, N'ReportHKLostNFound', N'Lost and Found', N'Lost and Found', N'aspx', N'/HouseKeeping/Reports', N'Report', NULL, 1, 1, CAST(N'2019-11-11T18:12:03.137' AS DateTime), 1, CAST(N'2019-11-11T18:14:03.470' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'HKLostFound')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( 28, N'HKLostFound', N'Lost and Found', N'Lost and Found', N'aspx', N'/HouseKeeping', N'Page', NULL, 1, 1, CAST(N'2019-11-06T11:25:56.120' AS DateTime), NULL, NULL)
END 
GO


IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'TemplateInformation')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (1, N'TemplateInformation', N'Template Setup', N'Template Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-08-06T20:03:07.697' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'TemplateUse')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (1, N'TemplateUse', N'Template Use', N'Template Use', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-08-06T20:03:07.697' AS DateTime), NULL, NULL)
END
GO
-- -- -- -- -- Update --------------

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmDivisionList')
BEGIN
	UPDATE SecurityMenuLinks
	SET  PageName = 'Division Information', PageDisplayCaption = 'Division Information'
	WHERE PageId = 'frmDivisionList'
END
GO


IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CostAnalysis')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'CostAnalysisInformation'
	WHERE PageId = 'CostAnalysis'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmEmpLeaveApplication')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Leave Application',
	PageId = 'LeaveInformation',
	PageDisplayCaption = 'Leave Application'
	WHERE PageId = 'frmEmpLeaveApplication'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmInvFinishedProductApproval')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Finished Item Approval',
	PageDisplayCaption = 'Finished Item Approval'
	WHERE PageId = 'frmInvFinishedProductApproval'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'DealCreation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Deal Information',
	PageDisplayCaption = 'Deal Information'
	WHERE PageId = 'DealCreation'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmInvFinishedProduct')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Finished Item',
	PageDisplayCaption = 'Finished Item'
	WHERE PageId = 'frmInvFinishedProduct'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmProductOutForRoom')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Item Out For Room',
	PageDisplayCaption = 'Item Out For Room'
	WHERE PageId = 'frmProductOutForRoom'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportProductOutInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Item Transfer Information',
	PageDisplayCaption = 'Item Transfer Information'
	WHERE PageId = 'frmReportProductOutInfo'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportProductOutInfo')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 11, N'frmReportProductOutInfo', N'Item Transfer Information', N'Item Transfer Information', N'aspx', N'/Inventory/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO


IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportProductReceiveInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Item Receive',
	PageDisplayCaption = 'Item Receive'
	WHERE PageId = 'frmReportProductReceiveInfo'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmProductOut')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'frmItemConsumption',
	PageName = 'Item Consumption',
	PageDisplayCaption = 'Item Consumption'
	WHERE PageId = 'frmProductOut'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmRoomCheckOut')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'frmExpectedDeparture' 
	WHERE PageId = 'frmRoomCheckOut'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmPMProductPO')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'PurchaseInfo',
	PageName = 'Item Purchase Order',
	PageDisplayCaption = 'Item Purchase Order'
	WHERE PageId = 'frmPMProductPO'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'PurchaseInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Item Purchase Order',
	PageDisplayCaption = 'Item Purchase Order'
	WHERE PageId = 'PurchaseInfo'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPMRequisition')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ItemRequisitionInformation', PageName = 'Requisition', PageDisplayCaption = 'Item Requisition'
	WHERE PageId = 'frmPMRequisition'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemRequisitionInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Requisition', PageDisplayCaption = 'Item Requisition'
	WHERE PageId = 'ItemRequisitionInformation'
END
GO
UPDATE SecurityMenuLinks
SET ActiveStat = 0 WHERE pageId LIKE '%frmPMRequisitionApproval%'
GO
DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId LIKE '%frmPMRequisitionApproval%')
GO
UPDATE SecurityMenuLinks
SET ActiveStat = 0 WHERE pageId LIKE '%frmPMProductPOApproval%'
GO
DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId LIKE '%frmPMProductPOApproval%')
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportFrontOfficeCash')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Payment Transaction',
	PageDisplayCaption = 'Payment Transaction'
	WHERE PageId = 'frmReportFrontOfficeCash'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmHMDayClose')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 1,
		ActiveStat = 1,
		PagePath = '/HMCommon',
		PageId = 'frmDayClose'
	WHERE PageId = 'frmHMDayClose'
END
GO
--- Existing Menu Name Changes If Exists
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmPMProductReceive')
BEGIN
	UPDATE SecurityMenuLinks SET PageId = 'ItemReceive', PageName = 'Item Receive', PageDisplayCaption = 'Item Receive'
	WHERE PageId = 'frmPMProductReceive'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmProductReceiveApproval')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ItemReceiveInfo', PageName = 'Item Receive Information', PageDisplayCaption = 'Item Receive Information'
	WHERE PageId = 'frmProductReceiveApproval'
END
GO

--- Existing Menu Name Changes If Exists
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmPMProductOut')
BEGIN
	UPDATE SecurityMenuLinks SET PageId = 'ItemTransfer', PageName = 'Item Transfer', PageDisplayCaption = 'Item Transfer'
	WHERE PageId = 'frmPMProductOut'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmProductOutApproval')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ItemTransferInformation', PageName = 'Item Transfer Information', PageDisplayCaption = 'Item Transfer Information'
	WHERE PageId = 'frmProductOutApproval'
END
GO



IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PurchaseOrder')
BEGIN
DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN 
(SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId LIKE 'PurchaseOrder')

DELETE SecurityMenuLinks WHERE pageId LIKE 'PurchaseOrder'

END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemTransfer')
BEGIN

DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN 
(SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId LIKE 'ItemTransfer')

DELETE SecurityMenuLinks WHERE pageId LIKE 'ItemTransfer'

END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmItemConsumption')
BEGIN

DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN 
(SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId = 'frmItemConsumption')

DELETE SecurityMenuLinks WHERE pageId LIKE 'frmItemConsumption'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemReceive')
BEGIN

DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN 
(SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId = 'ItemReceive')

DELETE SecurityMenuLinks WHERE pageId LIKE 'ItemReceive'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemReceive')
BEGIN

DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId IN 
(SELECT MenuLinksId FROM SecurityMenuLinks WHERE pageId = 'ItemReceive')

DELETE SecurityMenuLinks WHERE pageId LIKE 'ItemReceive'
END
GO


IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemTransferInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (10, N'ItemTransferInformation', N'Item Transfer Information', N'Item Transfer Information', 
			N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2018-10-11' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemConsumptionInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (10, N'ItemConsumptionInformation', N'Item Consumption Information', N'Item Consumption Information', 
			N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2018-10-11' AS DateTime), NULL, NULL)
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPMOutProductReceive')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'TransferredProductReceive', PageName = 'Transferred Item Receive', PageDisplayCaption = 'Transferred Item Receive'
	WHERE PageId = 'frmPMOutProductReceive'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'TransferredProductReceive')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Transferred Item Receive', PageDisplayCaption = 'Transferred Item Receive'
	WHERE PageId = 'TransferredProductReceive'
END 
GO


IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RestaurantBillReSettlementLogReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (7, N'RestaurantBillReSettlementLogReport', N'Re-Settlement Log Report', N'Re-Settlement Log Report', 
			N'aspx', N'/POS/Reports', N'Report', NULL, 1, 1, CAST(N'2018-11-14' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'BanquetBillReSettlementLogReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, N'BanquetBillReSettlementLogReport', N'Banquet Re-Settlement Log Report', N'Banquet Re-Settlement Log Report', 
			N'aspx', N'/Banquet/Reports', N'Report', NULL, 1, 1, CAST(N'2018-11-14' AS DateTime), NULL, NULL)
END
GO


IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'OutSalesReturnInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ConsumptionReturnInformation', PageName = 'Consumption Item Return', PageDisplayCaption = 'Consumption Item Return'
	WHERE PageId = 'OutSalesReturnInformation'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ConsumptionReturnInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (10, N'ConsumptionReturnInformation', N'Consumption Item Return', N'Consumption Item Return', 
			N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2018-10-11' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RecipeModifierType')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (8, N'RecipeModifierType', N'Recipe Modifier Type', N'Recipe Modifier Type', 
			N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(N'2019-04-18' AS DateTime), NULL, NULL)
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmPMPurchaseReturn')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'PurchaseReturn' 
	WHERE PageId = 'frmPMPurchaseReturn'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'ItemConsumptionInformationReport')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageType = 'Report',
		PageName = 'Date Wise Consumption',
		PageDisplayCaption = 'Date Wise Consumption'
	WHERE PageId = 'ItemConsumptionInformationReport'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportItemWiseStock')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageType = 'Report',
		PageName = 'Current Stock Information',
		PageDisplayCaption = 'Current Stock Information'
	WHERE PageId = 'frmReportItemWiseStock'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'RecipeModifierType')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 8
	WHERE PageId = 'RecipeModifierType'
END
GO


IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PurchaseReturnInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (12, N'PurchaseReturnInformation', N'Purchase Return Information', N'Purchase Return Information', 
			N'aspx', N'/PurchaseManagment', N'Page', NULL, 1, 1, CAST(N'2018-10-11' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RequestforQuotaion')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (12, N'RequestforQuotaion', N'Request for Quotaion', N'Request for Quotaion', 
			N'aspx', N'/PurchaseManagment', N'Page', NULL, 1, 1, CAST(N'2019-12-8' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'QuotaionFeedback')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (12, N'QuotaionFeedback', N'Quotaion Feedback', N'Quotaion Feedback', 
			N'aspx', N'/PurchaseManagment', N'Page', NULL, 1, 1, CAST(N'2019-12-8' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RFQComparativeStatement')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (12, N'RFQComparativeStatement', N'RFQ Comparative Statement', N'RFQ Comparative Statement', 
			N'aspx', N'/PurchaseManagment', N'Page', NULL, 1, 1, CAST(N'2019-12-8' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PageWiseMandatoryField')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (1, N'PageWiseMandatoryField', N'Page Wise Mandatory Field', N'Page Wise Mandatory Field', 
			N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-01-11' AS DateTime), NULL, NULL)
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPMPurchaseReturn' AND PageName = 'Purchase Return')
BEGIN
	Delete 
	FROM SecurityMenuLinks
	Where PageId = 'frmPMPurchaseReturn' AND PageName = 'Purchase Return'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportItemRecipe' AND PageName = 'Inventory Cost of Sales')
BEGIN
	Delete 
	FROM SecurityMenuLinks
	Where PageId = 'frmReportItemRecipe' AND PageName = 'Inventory Cost of Sales'
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'FormWiseFieldSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], 
	 [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (1, N'FormWiseFieldSetup', N'Form Wise Field Setup', N'Form Wise Field Setup', 
			N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2018-10-11' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemConsumptionReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 11, N'ItemConsumptionReport', N'Item Consumption Report', N'Item Consumption Report', N'aspx', N'/Inventory/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportMarketSegmentWiseInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageType = 'Report',
		PageName = 'Market Segment Information',
		PageDisplayCaption = 'Market Segment Information'
	WHERE PageId = 'frmReportMarketSegmentWiseInformation'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportMarketSegmentWiseInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (5, N'frmReportMarketSegmentWiseInformation', N'Market Segment Wise Information', N'Market Segment Information (Report)', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-01-23T17:41:43.823' AS DateTime), 1, CAST(N'2019-06-11T12:39:27.673' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ComplementaryGuestReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( 5, N'ComplementaryGuestReport', N'Complimentary Guest', N'Complementary Guest', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-01-31T17:19:56.313' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'WalkInGuestReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
	VALUES ( 5, N'WalkInGuestReport', N'Walk In Guest', N'Walk In Guest', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-01-31T17:20:58.877' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RoomBlocksReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
	VALUES ( 5, N'RoomBlocksReport', N'Room Blocks', N'Room Blocks', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-01-31T17:21:59.530' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'FOSalesComparison')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES (5, N'FOSalesComparison', N'FO Sales Comparison', N'FO Sales Comparison', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-04-17T17:50:40.470' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PosSalesComparison')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES (7, N'PosSalesComparison', N'Pos Sales Comparison', N'Pos Sales Comparison', N'aspx', N'/POS/Reports', N'Report', NULL, 1, 1, CAST(N'2019-04-18T10:59:04.870' AS DateTime), NULL, NULL)
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportSettlementDetails')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageType = 'Report'
	WHERE PageId = 'frmReportSettlementDetails'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportSettlementDetails')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES ( 5, N'frmReportSettlementDetails', N'Settlement Details', N'Settlement Details', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(N'2019-02-01T17:41:43.823' AS DateTime), 1, CAST(N'2019-06-11T12:39:12.377' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DiscountConfigurationSetup')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (1, N'DiscountConfigurationSetup', N'Discount Configuration Setup', N'Discount Configuration Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(N'2019-02-14T12:35:52.057' AS DateTime), 1, CAST(N'2019-02-14T12:41:15.257' AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CostAnalysisInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'CostAnalysisInformation', N'Cost Analysis', N'Cost Analysis', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-07-25T12:31:29.947' AS DateTime), NULL, NULL)
END
GO

-- -- -- -- -- CommonFormWiseFieldSetup  ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------
IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'ddlMarketSegment')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'ddlMarketSegment', N'Market Segment',10,CAST(N'2019-01-11' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'txtBookersName')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'txtBookersName', N'Bookers Name',1,CAST(N'2019-01-11' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 11 AND FieldId = 'txtNationalId')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (11, 0, N'txtNationalId', N'National ID',1,CAST(N'2019-01-29' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 11 AND FieldId = 'txtGuestDrivinlgLicense')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (11, 0, N'txtGuestDrivinlgLicense', N'Driving License',1,CAST(N'2019-01-29' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 11 AND FieldId = 'txtPassportNumber')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (11, 0, N'txtPassportNumber', N'Passport Number',1,CAST(N'2019-01-29' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 11 AND FieldId = 'txtGuestEmail')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (11, 0, N'txtGuestEmail', N'Email Address',1,CAST(N'2019-01-29' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'txtNationalId')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'txtNationalId', N'National ID',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'txtGuestDrivinlgLicense')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'txtGuestDrivinlgLicense', N'Driving License',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'txtPassportNumber')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'txtPassportNumber', N'Passport Number',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 9 AND FieldId = 'txtGuestEmail')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (9, 0, N'txtGuestEmail', N'Email Address',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO


IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 228 AND FieldId = 'txtNationalId')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (228, 0, N'txtNationalId', N'National ID',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 228 AND FieldId = 'txtGuestDrivinlgLicense')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (228, 0, N'txtGuestDrivinlgLicense', N'Driving License',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 228 AND FieldId = 'txtPassportNumber')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (228, 0, N'txtPassportNumber', N'Passport Number',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM CommonFormWiseFieldSetup WHERE PageId = 228 AND FieldId = 'txtGuestEmail')
BEGIN
	INSERT [dbo].[CommonFormWiseFieldSetup] 
	([PageId], [IsMandatory], [FieldId], [FieldName], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (228, 0, N'txtGuestEmail', N'Email Address',1,CAST(N'2019-01-30' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemConsumptionInformationReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (11, N'ItemConsumptionInformationReport', N'Date Wise Consumption', N'Date Wise Consumption', N'aspx', N'/Inventory/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmInventoryConfiguration')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmInventoryConfiguration', N'Inventory Configuration', N'Inventory Configuration', N'aspx', N'/Inventory', N'Page', NULL, 0, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBanquetConfiguration')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (8, N'frmBanquetConfiguration', N'Banquet Configuration', N'Banquet Configuration', N'aspx', N'/Banquet', N'Page', NULL, 0, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO
  GO
 IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmMemberBillReceive')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (26, N'frmMemberBillReceive', N'Member Bill Receive', N'Member Bill Receive', N'aspx', N'/Membership', N'Page', NULL, 1, 1, CAST(N'2019-09-20T15:30:48.983' AS DateTime), NULL, NULL)
END
Go

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmMemberBillGeneration')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (26, N'frmMemberBillGeneration', N'Member Bill Generation', N'Member Bill Generation', N'aspx', N'/Membership', N'Page', NULL, 1, 1, CAST(N'2019-09-23T11:30:39.880' AS DateTime), NULL, NULL)
END
GO

GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'CrmDynamicReport')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
 VALUES (23, N'CrmDynamicReport', N'CRM Dynamic Report', N'CRM Dynamic Report', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2020-01-06T11:51:40.207' AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LogActivityReport')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
 VALUES (23, N'LogActivityReport', N'Log Activity', N'Log Activity', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2020-01-06T11:51:40.207' AS DateTime), NULL, NULL)
END
GO
-- -- -- -- -- SecurityMenuGroup  ------------------------------------------------------------------------------------------
-- -- -- -- -- Save --------------
IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'Maintenance')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'Maintenance', N'Maintenance', 1, N'icon-group', 1, NULL, NULL, NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'FixedAsset')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'FixedAsset', N'Fixed Asset', 1, N'icon-group', 1, NULL, NULL, NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'Document Management')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Document Management', N'Document Management', 15, N'icon-suitcase', 1, 1, CAST(0x0000A5DA00000000 AS DateTime), 1, CAST(0x0000A60801400842 AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'Task Management')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Task Management', N'Task Management', 16, N'icon-suitcase', 1, 1, CAST(0x0000A5DA00000000 AS DateTime), 1, CAST(0x0000A60801400842 AS DateTime))
END
GO
-- -- -- -- -- HotelGuestBillPayment  ------------------------------------------------------------------------------------------
-- -- -- -- -- Update --------------
UPDATE HotelGuestBillPayment
SET PaymentType = 'PaidOut'
WHERE PaymentType = 'CashOut'
GO

-- -- -- -- -- BanquetReservation  ------------------------------------------------------------------------------------------
-- -- -- -- -- Update --------------
IF EXISTS(SELECT * FROM BanquetReservation WHERE ReservationDiscountType IS NULL)
BEGIN
	UPDATE BanquetReservation
	SET ReservationDiscountType = DiscountType,
		ReservationDiscountAmount = DiscountAmount
	WHERE ReservationDiscountType IS NULL
END
GO

-- -- -- -- -- SecurityMenuLinks  ------------------------------------------------------------------------------------------
-- -- -- -- -- Update --------------
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmMemberBillReceive')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Member Payment Receive', 
	    [PageDisplayCaption] = 'Member Payment Receive'
	WHERE PageId = 'frmMemberBillReceive'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmEmpIncrement')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Employee Increment', 
	    [PageDisplayCaption] = 'Employee Increment'
	WHERE PageId = 'frmEmpIncrement'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSupplierBillPayment')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'frmSupplierPayment'
	WHERE PageId = 'frmSupplierBillPayment'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportDateWiseSalesSummary')
BEGIN
	DELETE FROM SecurityMenuLinks WHERE PageId = 'frmReportDateWiseSalesSummary'
	
	DELETE FROM SecurityMenuWiseLinks 
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
	WHERE pageId = 'frmReportDateWiseSalesSummary')
END
GO
-- -- -- -- -- HotelRoomReservationSummary  ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM HotelRoomReservationSummary)
BEGIN
	INSERT INTO HotelRoomReservationSummary
	SELECT ReservationId, RoomTypeId, COUNT(RoomTypeId), ISNULL(RoomTypeWisePaxQuantity, 1) 
	FROM HotelRoomReservationDetail 
	GROUP BY ReservationId, RoomTypeId, RoomTypeWisePaxQuantity
END
GO

-- -- -- -- -- Update CommonCostCenter for CostCenterType = 'Other' ------------------------------------------------------------------------------------------
IF EXISTS(SELECT * FROM CommonCostCenter WHERE CostCenterType = 'Other')
BEGIN
	UPDATE CommonCostCenter
	SET CostCenterType = 'OtherOutlet'
	WHERE CostCenterType = 'Other'
END
GO
-- -- -- -- -- Update HotelGuestInformation for Title 'Mr. & Mrs.' to 'MrNMrs.' ------------------------------------------------------------------------------------------
-- -- -- -- -- Update --------------
IF EXISTS(SELECT * FROM HotelGuestInformation WHERE Title = 'Mr. & Mrs.')
BEGIN
	UPDATE HotelGuestInformation
	SET Title = 'MrNMrs.'
	WHERE Title = 'Mr. & Mrs.'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmGuestReference')
BEGIN
	UPDATE SecurityMenuLinks
	SET PagePath = '/SalesAndMarketing'
	WHERE PageId = 'frmGuestReference'
END
GO

-- -- -- -- -- Link Name Update from Reservation to Registration
UPDATE lrm
SET LRM.LinkName = hr2.ReservationNumber
FROM HotelLinkedRoomMaster lrm
INNER JOIN HotelRoomRegistration hrr ON lrm.RegistrationId = hrr.RegistrationId
INNER JOIN HotelRoomReservation hr2 ON hr2.ReservationId = hrr.ReservationId
WHERE ISNULL(LinkName, '') = ''
GO
-- -- -- -- -- PayrollEmpType  ------------------------------------------------------------------------------------------
IF EXISTS(SELECT * FROM PayrollEmpType WHERE TypeCategory IS NULL )
BEGIN
	UPDATE PayrollEmpType
	SET TypeCategory = 'Regular'
	WHERE TypeCategory IS NULL
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportItemWastageAndAdjustmentInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Item wastage & Adjustment',
	[PageDisplayCaption] = 'Item wastage & Adjustment'
	WHERE PageId = 'frmReportItemWastageAndAdjustmentInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportInvItemInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Item Information',
	[PageDisplayCaption] = 'Item Information'
	WHERE PageId = 'frmReportInvItemInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportHouseKeeping')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'House Keeping Information',
	[PageDisplayCaption] = 'House Keeping Information'
	WHERE PageId = 'frmReportHouseKeeping'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportRoomShiftInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Room Shift Information',
	[PageDisplayCaption] = 'Room Shift Information'
	WHERE PageId = 'frmReportRoomShiftInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportReservationCancelInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Reservation Cancel Information',
	[PageDisplayCaption] = 'Reservation Cancel Information'
	WHERE PageId = 'frmReportReservationCancelInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportBanquetSalesInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Banquet Sales Information',
	[PageDisplayCaption] = 'Banquet Sales Information'
	WHERE PageId = 'frmReportBanquetSalesInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportCanceledReservation')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Canceled Reservation Information',
	[PageDisplayCaption] = 'Canceled Reservation Information'
	WHERE PageId = 'frmReportCanceledReservation'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportGuestPaymentInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Guest Payment Information',
	[PageDisplayCaption] = 'Guest Payment Information'
	WHERE PageId = 'frmReportGuestPaymentInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmDailyOccupiedRoomList')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Daily Occupied Room Information',
	[PageDisplayCaption] = 'Daily Occupied Room Information'
	WHERE PageId = 'frmDailyOccupiedRoomList'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportGuestAirportPickupDropInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Pick Up/ Drop Off Information',
	[PageDisplayCaption] = 'Pick Up/ Drop Off Information'
	WHERE PageId = 'frmReportGuestAirportPickupDropInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmAirlineInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Vehicle Information',
	[PageDisplayCaption] = 'Vehicle Information'
	WHERE PageId = 'frmAirlineInformation'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportInvVarianceInfo')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Inventory Variance Information',
	[PageDisplayCaption] = 'Inventory Variance Information'
	WHERE PageId = 'frmReportInvVarianceInfo'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportSalesAudit')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Sales Summary Information',
	[PageDisplayCaption] = 'Sales Summary Information'
	WHERE PageId = 'frmReportSalesAudit'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportItemWiseRequisitionInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Item Requisition Information',
	[PageDisplayCaption] = 'Item Requisition Information'
	WHERE PageId = 'frmReportItemWiseRequisitionInformation'
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportDateWiseRequisitionInformation')
BEGIN
	Delete FROM SecurityMenuLinks	
	WHERE PageId = 'frmReportDateWiseRequisitionInformation'
END
GO

IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportItemWisePurchaseInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET [PageName] = 'Item Purchase Information',
	[PageDisplayCaption] = 'Item Purchase Information'
	WHERE PageId = 'frmReportItemWisePurchaseInformation'
END
GO
UPDATE SecurityMenuLinks
SET ActiveStat = 0 WHERE pageId = 'frmReportCashBookStatement'
GO
DELETE FROM SecurityMenuWiseLinks 
WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
WHERE pageId = 'frmReportCashBookStatement')
GO
UPDATE SecurityMenuLinks
SET ActiveStat = 0 WHERE pageId = 'frmReportBreakDownStatement'
GO
DELETE FROM SecurityMenuWiseLinks 
WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
WHERE pageId = 'frmReportBreakDownStatement')
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ProjectStage')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'ProjectStage', N'Project Stage', N'ProjectStage', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO

IF EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = '6 Months' )
BEGIN
	UPDATE CommonCustomfieldData
	SET FieldValue = 'Replacement'
	WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = '6 Months'
END
GO
IF EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = '1 Year' )
BEGIN
	UPDATE CommonCustomfieldData
	SET FieldValue = 'Service'
	WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = '1 Year'	

	Update InvItem
	SET ServiceWarranty = (select FieldId from CommonCustomfieldData WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = 'Service')
	FROM InvItem it
	INNER JOIN CommonCustomfieldData ccd
	ON it.ServiceWarranty = ccd.FieldId
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'InventoryProductServiceWarranty' AND FieldValue = 'Manufacturer' )
BEGIN
	INSERT INTO CommonCustomfieldData(FieldType, FieldValue, Description, ActiveStat)
	SELECT 'InventoryProductServiceWarranty', 'Manufacturer', '', 1
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CompanyContributionOn' AND FieldValue = 'Gross' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CompanyContributionOn', N'Gross', N'Gross Salary', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CompanyContributionOn' AND FieldValue = 'Basic' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CompanyContributionOn', N'Basic', N'Basic Salary', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CompanyContributionOn' AND FieldValue = 'EmployeeContribution' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CompanyContributionOn', N'EmployeeContribution', N'Employee Contribution', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'InterCompanyTransactionHeadId' AND SetupName = 'InterCompanyTransactionHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'InterCompanyTransactionHeadId', N'InterCompanyTransactionHeadId', N'1', N'InterCompanyTransactionHeadId', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportEmpCV')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ReportEmployeeProfile', 
		PageDisplayCaption = 'Employee Profile',
		PageName = 'Employee Profile'
	WHERE PageId = 'frmReportEmpCV'
END 
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportCashRequisitionNAdjustment')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], 
	[PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (19, N'frmReportCashRequisitionNAdjustment', N'Cash Requisition And Adjustment', N'Cash Requisition And Adjustment', 
	N'aspx', N'/Payroll/Reports', N'Report', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'DynamicEmployeeReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (19, N'DynamicEmployeeReport', N'Dynamic Employee', N'Dynamic Employee', N'aspx', N'/Payroll/Reports', N'Report', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ReportEmployeeProfile')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (19, N'ReportEmployeeProfile', N'Employee Profile', N'Employee Profile', N'aspx', N'/Payroll/Reports', N'Report', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'FiscalYear')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'FiscalYear', N'Fiscal Year', N'Fiscal Year', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmStaffRequisition')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 24, 
		PagePath = '/Recruitment'
	WHERE PageId = 'frmStaffRequisition'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmStaffingBudget')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 24, 
		PagePath = '/Recruitment'
	WHERE PageId = 'frmStaffingBudget'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportStaffingBudget')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 25, 
		PagePath = '/Recruitment/Reports'
	WHERE PageId = 'frmReportStaffingBudget'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportStaffRequisition')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 25, 
		PagePath = '/Recruitment/Reports'
	WHERE PageId = 'frmReportStaffRequisition'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ItemRequisitionInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Item Requisition',
		PageDisplayCaption = 'Item Requisition'
	WHERE PageId = 'ItemRequisitionInformation'
END 
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmGLProject')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'GLProject'
	WHERE PageId = 'frmGLProject'
END
GO
IF NOT EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'GLProject')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (2, N'GLProject', N'Project Information', N'Project Information', N'aspx', N'/GeneralLedger', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAutoLoanCollectionProcessEnable' AND SetupName = 'IsAutoLoanCollectionProcessEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAutoLoanCollectionProcessEnable', N'IsAutoLoanCollectionProcessEnable', N'0', N'IsAutoLoanCollectionProcessEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsEmpSearchFromDashboardEnable' AND SetupName = 'IsEmpSearchFromDashboardEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsEmpSearchFromDashboardEnable', N'IsEmpSearchFromDashboardEnable', N'0', N'IsEmpSearchFromDashboardEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LateBufferingTime' AND SetupName = 'LateBufferingTime' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'LateBufferingTime', N'LateBufferingTime', N'0', N'LateBufferingTime', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsEmpSearchDetailsFromDashboardEnable' AND SetupName = 'IsEmpSearchDetailsFromDashboardEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsEmpSearchDetailsFromDashboardEnable', N'IsEmpSearchDetailsFromDashboardEnable', N'0', N'IsEmpSearchDetailsFromDashboardEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TermsAndConditions' AND FieldValue = 'PurchaseOrder' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TermsAndConditions', N'PurchaseOrder', N'Purchase Order', 1)
END

IF NOT EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'TermsNConditionsSetup')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (1, N'TermsNConditionsSetup', N'Terms And Conditions Setup', N'Terms And Conditions Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'EmployeeBalanceTransfer')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (18, N'EmployeeBalanceTransfer', N'Employee Balance Transfer', N'Employee Balance Transfer', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportEmployeeBirthday')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 19, N'frmReportEmployeeBirthday', N'Employee Birthday Information', N'Employee Birthday Information', N'aspx', N'/Payroll/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportActivitiLogs')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'User Activity Report',
		PageDisplayCaption = 'User Activity Report'
	WHERE PageId = 'frmReportActivitiLogs'
END 
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportEmpProvisionPeriod')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 19, N'frmReportEmpProvisionPeriod', N'Employee Provision Period', N'Employee Provision Period', N'aspx', N'/Payroll/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CostCenterType' AND FieldValue = 'Billing' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CostCenterType', N'Billing', N'Billing', 1)
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmEmpTrainingType' AND PageName = 'Employee Training Type' AND PageDisplayCaption = 'Employee Training Type')
BEGIN
	UPDATE SecurityMenuLinks
	SET 
	PageName = 'Employee Training Information',
	PageDisplayCaption = 'Employee Training Information'
	WHERE PageId = 'frmEmpTrainingType'
END
GO

IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmApplicant' AND PageName = 'Applicant Info')
BEGIN
	UPDATE SecurityMenuLinks
	SET 
	PageName = 'Applicant Information'
	WHERE PageId = 'frmApplicant'
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ProductOutFor' AND FieldValue = 'Requisition' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'ProductOutFor', N'Requisition', N'Requisition Wise Transfer', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ProductOutFor' AND FieldValue = 'StockTransfer' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'ProductOutFor', N'StockTransfer', N'Stock Transfer', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ProductOutFor' AND FieldValue = 'SalesOut' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'ProductOutFor', N'SalesOut', N'Pre Sales', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'ProductOutFor' AND FieldValue = 'Billing' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'ProductOutFor', N'Billing', N'Billing', 1)
END
GO


IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'PreSales' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'PreSales', N'Pre Sales', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'Internal' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'Internal', N'Internal', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'Project' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'Project', N'Project', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TaskType' AND FieldValue = 'Billing' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'TaskType', N'Billing', N'Billing', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'GanttChart' AND FieldValue = 'Project' )
BEGIN
	INSERT INTO CommonCustomfieldData(FieldType, FieldValue, Description, ActiveStat)
	SELECT 'GanttChart', 'Project', 'Project', 1
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'GanttChart' AND FieldValue = 'PreSales' )
BEGIN
	INSERT INTO CommonCustomfieldData(FieldType, FieldValue, Description, ActiveStat)
	SELECT 'GanttChart', 'PreSales', 'Pre Sales', 1
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'GanttChart' AND FieldValue = 'Billing' )
BEGIN
	INSERT INTO CommonCustomfieldData(FieldType, FieldValue, Description, ActiveStat)
	SELECT 'GanttChart', 'Billing', 'Billing', 1
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomfieldData WHERE FieldType = 'GanttChart' AND FieldValue = 'Employee' )
BEGIN
	INSERT INTO CommonCustomfieldData(FieldType, FieldValue, Description, ActiveStat)
	SELECT 'GanttChart', 'Employee', 'Employee', 1
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateType' AND FieldValue = 'Email' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateType', N'Email', N'Email', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateType' AND FieldValue = 'Letter' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateType', N'Letter', N'Letter', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateType' AND FieldValue = 'SMS' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateType', N'SMS', N'SMS', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateFor' AND FieldValue = 'Employee' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateFor', N'Employee', N'PayrollEmployee', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateFor' AND FieldValue = 'Contacts' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateFor', N'Contacts', N'SMContactInformation', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateFor' AND FieldValue = 'Company' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateFor', N'Company', N'HotelGuestCompany', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateFor' AND FieldValue = 'Supplier' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateFor', N'Supplier', N'PMSupplier', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'TemplateFor' AND FieldValue = 'Common' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'TemplateFor', N'Common', N'CommonDataForTemplate', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CommonDataForTemplate' AND FieldValue = 'Current Date (DD/MM/YYYY)' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'CommonDataForTemplate', N'Current Date (DD/MM/YYYY)', N'CurrentDate(DD/MM/YYYY)', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CommonDataForTemplate' AND FieldValue = 'Current Date (MMM-DD-YYYY)' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'CommonDataForTemplate', N'Current Date (MMM-DD-YYYY)', N'CurrentDate(MMM-DD-YYYY)', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CommonDataForTemplate' AND FieldValue = 'Current Month' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'CommonDataForTemplate', N'Current Month', N'CurrentMonth', 1)
END
GO

IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CommonDataForTemplate' AND FieldValue = 'Current Year' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) 
	VALUES (N'CommonDataForTemplate', N'Current Year', N'CurrentYear', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'CNFAccountsHeadId' AND SetupName = 'CNFAccountsHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'CNFAccountsHeadId', N'CNFAccountsHeadId', N'103', N'CNFAccountsHeadId', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCompanyBillGeneration')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 22,
		PagePath = '/SalesAndMarketing'
	WHERE PageId = 'frmCompanyBillGeneration'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCompanyBillReceive')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 22,
		PagePath = '/SalesAndMarketing'
	WHERE PageId = 'frmCompanyBillReceive'
END 
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillingInvoiceTemplateWithoutHeader' AND SetupName = 'IsBillingInvoiceTemplateWithoutHeader' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES ( N'IsBillingInvoiceTemplateWithoutHeader', N'IsBillingInvoiceTemplateWithoutHeader', N'0', N'IsBillingInvoiceTemplateWithoutHeader', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'RemarksDetailsForBilling' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'RemarksDetailsForBilling', N'', N'Remarks Details For Billing', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'RemarksDetailsForSTBilling' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'RemarksDetailsForSTBilling', N'', N'Remarks Details For ST Billing', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'RemarksDetailsForATBilling' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'RemarksDetailsForATBilling', N'', N'Remarks Details For AT Billing', 1)
END
GO
DELETE FROM CommonSetup WHERE SetupId IN
(SELECT SetupId
FROM  (
		SELECT
			SetupId, TypeName, SetupName,
			ROW_NUMBER() OVER (PARTITION BY TypeName,SetupName ORDER BY [SetupName]) AS 'RANK'
			--, COUNT(SetupName) OVER (PARTITION BY  SetupName) AS 'MAXCOUNT'
		FROM CommonSetup
     ) a 
WHERE [RANK] > 1)
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmHotelConfiguration' AND PageName = 'FO Configuration' AND PageDisplayCaption = 'FO Configuration')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Front Office Configuration',
		PageDisplayCaption = 'Front Office Configuration'
	WHERE PageId = 'frmHotelConfiguration'
	AND PageName = 'FO Configuration'
	AND PageDisplayCaption = 'FO Configuration'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportRoomShiftInfo' and PageName = 'Room Shift Information' AND PageDisplayCaption = 'Room Shift Information')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Room Change Information',
		PageDisplayCaption = 'Room Change Information'
	WHERE PageId = 'frmReportRoomShiftInfo'
	AND PageName = 'Room Shift Information'
	AND PageDisplayCaption = 'Room Shift Information'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCashierInformation' and PageName = 'Restaurant User' AND PageDisplayCaption = 'Restaurant User')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Billing User',
		PageDisplayCaption = 'Billing User'
	WHERE PageId = 'frmCashierInformation'
	AND PageName = 'Restaurant User'
	AND PageDisplayCaption = 'Restaurant User'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmDesignation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Employee Designation',
		PageDisplayCaption = 'Employee Designation'
	WHERE PageId = 'frmDesignation'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmInvLocation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Store Location',
		PageDisplayCaption = 'Store Location'
	WHERE PageId = 'frmInvLocation'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPOSConfiguration')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Billing Configuration',
		PageDisplayCaption = 'Billing Configuration'
	WHERE PageId = 'frmPOSConfiguration'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'RecipeModifierType')
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId = 10
	WHERE PageId = 'RecipeModifierType'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportActivitiLogs')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'User Activity Log',
		PageDisplayCaption = 'User Activity Log'
	WHERE PageId = 'frmReportActivitiLogs'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportGuestInformationDetails')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'SB Report',
		PageDisplayCaption = 'SB Report'
	WHERE PageId = 'frmReportGuestInformationDetails'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportAdvReservationForecast' AND ModuleId = 5)
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Room Reservation Forecast',
		PageDisplayCaption = 'Room Reservation Forecast'
	WHERE PageId = 'frmReportAdvReservationForecast'
	AND ModuleId = 5
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportAdvReservationForecast' AND ModuleId = 9)
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'frmReportAdvBanquetReservationForecast',
		PageName = 'Banquet Reservation Forecast',
		PageDisplayCaption = 'Banquet Reservation Forecast'
	WHERE PageId = 'frmReportAdvReservationForecast'
	AND ModuleId = 9
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportBanquetTransaction')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Banquet Sales Transaction',
		PageDisplayCaption = 'Banquet Sales Transaction'
	WHERE PageId = 'frmReportBanquetTransaction'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBank')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Bank Information',
		PageDisplayCaption = 'Bank Information'
	WHERE PageId = 'frmBank'
END 
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'EmployeeLoanApproval' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'EmployeeLoanApproval', N'Employee Loan Approval', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmployeeLoanApproval' AND SetupName = 'EmployeeLoanApproval')
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmployeeLoanApproval', N'EmployeeLoanApproval', N'0', N'0 = Same person can create ,check and approve ; 1 = Creator cannot check/approve but Same person can check and approve both; 2 = Same person cannot create, check and approve both', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BillDeclaration')
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BillDeclaration', N'', N'', 1)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportAdvReservationForecast')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Room Reservation Forecast',
		PageDisplayCaption = 'Room Reservation Forecast'
	WHERE PageId = 'frmReportAdvReservationForecast'
END 
GO
UPDATE PayrollEmployee
SET Title = REPLACE(LTRIM(RTRIM(Title)), 'Mr', 'Mr.'),
	DisplayName = REPLACE(LTRIM(RTRIM(DisplayName)), 'Mr', 'Mr.')
WHERE LTRIM(RTRIM(Title)) = 'Mr'
GO
UPDATE PayrollEmployee
SET Title = REPLACE(LTRIM(RTRIM(Title)), 'Mrs', 'Mrs.'),
	DisplayName = REPLACE(LTRIM(RTRIM(DisplayName)), 'Mrs', 'Mrs.')
WHERE LTRIM(RTRIM(Title)) = 'Mrs'
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmEmpAttendance')
BEGIN
	UPDATE SecurityMenuLinks
	SET ActiveStat = 0 WHERE PageId = 'frmEmpAttendance'
	
	DELETE FROM SecurityMenuWiseLinks 
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
	WHERE PageId = 'frmEmpAttendance')
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmProductReceiveAccountsPostingApproval')
BEGIN
	UPDATE SecurityMenuLinks
	SET ActiveStat = 0 WHERE PageId = 'frmProductReceiveAccountsPostingApproval'
	
	DELETE FROM SecurityMenuWiseLinks 
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
	WHERE PageId = 'frmProductReceiveAccountsPostingApproval')
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmLeaveOpening')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Leave Balance',
		PageDisplayCaption = 'Leave Balance'
	WHERE PageId = 'frmLeaveOpening'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmLeaveInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET ActiveStat = 0 WHERE PageId = 'frmLeaveInformation'
	
	DELETE FROM SecurityMenuWiseLinks 
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
	WHERE PageId = 'frmLeaveInformation')
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'LeaveInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Leave Information',
		PageDisplayCaption = 'Leave Information'
	WHERE PageId = 'LeaveInformation'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCompanyBillAdjustment')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Company Advance Adjustment',
		PageDisplayCaption = 'Company Advance Adjustment',
		PagePath = '/SalesAndMarketing'
	WHERE PageId = 'frmCompanyBillAdjustment'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportComplimentaryGuest')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Complimentary Guest',
		PageDisplayCaption = 'Complimentary Guest'
	WHERE PageId = 'frmReportComplimentaryGuest'
END 
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ComplementaryGuestReport')
BEGIN
	DELETE FROM SecurityMenuWiseLinks 
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks 
	WHERE PageId = 'ComplementaryGuestReport')
	
	DELETE FROM SecurityMenuLinks
	WHERE PageId = 'ComplementaryGuestReport'
END 
GO
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Support & Ticket Management' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Support & Ticket Management')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Support & Ticket Management' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 17, N'Support & Ticket Management', N'grpSupportTicketManagement', N'/SupportAndTicket/', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Report:: Support & TicketManagement' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 17, N'Report:: Support & TicketManagement', N'grpSupportTicketManagement ', N'/SupportAndTicket/Reports', 1, 1)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'Support & Ticket Management')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'Support & Ticket Management', N'Support & Ticket Management', 1, N'icon-group', 1, NULL, NULL, NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (38, N'SupportSetup', N'Support Setup', N'Support Setup', N'aspx', N'/SupportAndTicket', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportPriceMatrixSetup' AND ModuleId = 39)
BEGIN
	UPDATE SecurityMenuLinks
	SET ModuleId= 38
	WHERE PageId = 'SupportPriceMatrixSetup' AND ModuleId = 39
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportPriceMatrixSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (38, N'SupportPriceMatrixSetup', N'Support Price Matrix Setup', N'Support Price Matrix Setup', N'aspx', N'/SupportAndTicket', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportPriceMatrixReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (39, N'SupportPriceMatrixReport', N'Support Price Matrix', N'Support Price Matrix', N'aspx', N'/SupportAndTicket/Reports', N'Report', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportCallBillingInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (38, N'SupportCallBillingInformation', N'Support Call Billing Information', N'Support Call Billing Information', N'aspx', N'/SupportAndTicket', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportCallInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (38, N'SupportCallInformation', N'Support Call Information', N'Support Call Information', N'aspx', N'/SupportAndTicket', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'ContactTitle')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'ContactTitle', N'Contact Title', N'Contact Title', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportCallImplementation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (38, N'SupportCallImplementation', N'Support Call Implementation', N'Support Call Implementation', N'aspx', N'/SupportAndTicket', N'Page', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCRMAreaFieldEnable' AND SetupName = 'IsCRMAreaFieldEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCRMAreaFieldEnable', N'IsCRMAreaFieldEnable', N'0', N'IsCRMAreaFieldEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCompanyPaymentCollection')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
	VALUES (23, N'frmCompanyPaymentCollection', N'Company Payment Collection', N'Company Payment Collection', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2020-01-06T11:51:40.207' AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBanquetRefference')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Banquet Reference',
		PageDisplayCaption = 'Banquet Reference'
	WHERE PageId = 'frmBanquetRefference'
END 
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCRMCompanyNumberEnable' AND SetupName = 'IsCRMCompanyNumberEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCRMCompanyNumberEnable', N'IsCRMCompanyNumberEnable', N'1', N'IsCRMCompanyNumberEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'BillingDefaultApprovedBySignature' AND SetupName = 'BillingDefaultApprovedBySignature' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'BillingDefaultApprovedBySignature', N'BillingDefaultApprovedBySignature', N'', N'BillingDefaultApprovedBySignature', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'SupportDashboardReport')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (39, N'SupportDashboardReport', N'Support Call Report', N'Support Call Report', N'aspx', N'/SupportAndTicket/Reports', N'Report', NULL, 1, 1, CAST(0x0000A919011AAA67 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAutoCompanyBillGenerationProcessEnable' AND SetupName = 'IsAutoCompanyBillGenerationProcessEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAutoCompanyBillGenerationProcessEnable', N'IsAutoCompanyBillGenerationProcessEnable', N'0', N'IsAutoCompanyBillGenerationProcessEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillCalculationProcessEnableForCompanyDiscountAndCashIncentive' AND SetupName = 'IsBillCalculationProcessEnableForCompanyDiscountAndCashIncentive' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillCalculationProcessEnableForCompanyDiscountAndCashIncentive', N'IsBillCalculationProcessEnableForCompanyDiscountAndCashIncentive', N'0', N'IsBillCalculationProcessEnableForCompanyDiscountAndCashIncentive', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsFOSalesReportFormatOneEnable' AND SetupName = 'IsFOSalesReportFormatOneEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsFOSalesReportFormatOneEnable', N'IsFOSalesReportFormatOneEnable', N'0', N'IsFOSalesReportFormatOneEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmAccountManager')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'AccountManagerNew'
	WHERE PageId = 'frmAccountManager'
END 
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DefaultVatCalculationCostcenterId' AND SetupName = 'DefaultVatCalculationCostcenterId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DefaultVatCalculationCostcenterId', N'DefaultVatCalculationCostcenterId', N'0', N'DefaultVatCalculationCostcenterId', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSalesSummary')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES (7, N'frmSalesSummary', N'Sales Summary', N'Sales Summary', N'aspx', N'/POS/Reports', N'Report', NULL, 1, 1, CAST(N'2019-04-18T10:59:04.870' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCompanyHyperlinkEnableFromGrid' AND SetupName = 'IsCompanyHyperlinkEnableFromGrid' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCompanyHyperlinkEnableFromGrid', N'IsCompanyHyperlinkEnableFromGrid', N'1', N'IsCompanyHyperlinkEnableFromGrid', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsContactHyperlinkEnableFromGrid' AND SetupName = 'IsContactHyperlinkEnableFromGrid' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsContactHyperlinkEnableFromGrid', N'IsContactHyperlinkEnableFromGrid', N'1', N'IsContactHyperlinkEnableFromGrid', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCashRequisitionEnable' AND SetupName = 'IsCashRequisitionEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCashRequisitionEnable', N'IsCashRequisitionEnable', N'1', N'IsCashRequisitionEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillVoucherEnable' AND SetupName = 'IsBillVoucherEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillVoucherEnable', N'IsBillVoucherEnable', N'1', N'IsBillVoucherEnable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmEmpMonthlyAttendanceProcess')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] 
	([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass],[ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	 VALUES (18, N'frmEmpMonthlyAttendanceProcess', N'Monthly Attendance Process', N'Monthly Attendance Process',N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(N'2019-01-16' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemAttributeEnable' AND SetupName = 'IsItemAttributeEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemAttributeEnable', N'IsItemAttributeEnable', N'1', N'Is Item Attribute Enable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'InvItemAttributeSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'InvItemAttributeSetup', N'Item Attribute', N'Item Attribute', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmVoucherEntrySearch')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Voucher Authorization',
		PageDisplayCaption = 'Voucher Authorization'
	WHERE PageId = 'frmVoucherEntrySearch'
END 
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'ContactCreation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageId = 'ContactCreation',
	PageName = 'Contact Information',
	PageDisplayCaption = 'Contact Information'
	WHERE PageId = 'ContactCreation'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSalesReturn')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
    VALUES (6, N'frmSalesReturn', N'Sales Return', N'Sales Return', N'aspx', N'/POS', N'Page', NULL, 1, 1, CAST(N'2019-04-18T10:59:04.870' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DeliveredByDepartmentId' AND SetupName = 'DeliveredByDepartmentId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DeliveredByDepartmentId', N'DeliveredByDepartmentId', N'1', N'DeliveredByDepartmentId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'DefaultBestRegardsSetup')
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'DefaultBestRegardsSetup', N'Abdullah Al Mamun~Chief Executive Officer~@CompanyName', N'', 1)
END
GO
IF EXISTS (SELECT * FROM PayrollEmpIncrement WHERE IncrementMode = 'Tk')
BEGIN
	UPDATE PayrollEmpIncrement
	SET IncrementMode = 'Fixed'
	WHERE IncrementMode = 'Tk'
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'CostCenterType' AND FieldValue = 'CallCenter' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'CostCenterType', N'CallCenter', N'Call Center', 0)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmInvProduction')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmInvProduction', N'Production', N'Production', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillingRemarksEnableOnVoucher' AND SetupName = 'IsBillingRemarksEnableOnVoucher' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillingRemarksEnableOnVoucher', N'IsBillingRemarksEnableOnVoucher', N'0', N'IsBillingRemarksEnableOnVoucher', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT * FROM PayrollEmployee WHERE GlCompanyId IS NULL)
BEGIN
	DECLARE @PGlCompanyId INT
	SELECT TOP 1 @PGlCompanyId = ISNULL(CompanyId, 1) FROM GLCompany
	
	UPDATE PayrollEmployee
	SET GlCompanyId = @PGlCompanyId
	WHERE GlCompanyId IS NULL
END
GO
IF NOT EXISTS(SELECT * FROM GLProject WHERE IsDefaultProject = 1 )
BEGIN
	DECLARE @GLCompanyID INT
	DECLARE @getGLCompanyID CURSOR
	SET @getGLCompanyID = CURSOR FOR
	SELECT CompanyId
	FROM GLCompany
	OPEN @getGLCompanyID
	FETCH NEXT
	FROM @getGLCompanyID INTO @GLCompanyID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT @GLCompanyID
		DECLARE @ProjectId INT
		SET @ProjectId = 1
		
		SELECT TOP 1
			@ProjectId = ISNULL(glp.ProjectId, 1)
		FROM GLProject glp
		INNER JOIN GLCompany glc ON glc.CompanyId = glp.CompanyId
		WHERE glp.CompanyId = @GLCompanyID
		
		UPDATE GLProject SET IsDefaultProject = 1 WHERE ProjectId = @ProjectId
	FETCH NEXT
	FROM @getGLCompanyID INTO @GLCompanyID
	END
	CLOSE @getGLCompanyID
	DEALLOCATE @getGLCompanyID
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollLetterPanelHide' AND SetupName = 'IsPayrollLetterPanelHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollLetterPanelHide', N'IsPayrollLetterPanelHide', N'1', N'IsPayrollLetterPanelHide', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollLetterEnableWithoutHeaderFooter' AND SetupName = 'IsPayrollLetterEnableWithoutHeaderFooter' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollLetterEnableWithoutHeaderFooter', N'IsPayrollLetterEnableWithoutHeaderFooter', N'0', N'IsPayrollLetterEnableWithoutHeaderFooter', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'SupplierContactType' AND FieldValue = 'Supplier' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'SupplierContactType', N'Supplier', N'Supplier', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'SupplierContactType' AND FieldValue = 'Support' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'SupplierContactType', N'Support', N'Support', 0)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'SupplierContactType' AND FieldValue = 'Development' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'SupplierContactType', N'Development', N'Development', 0)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'SupplierContactType' AND FieldValue = 'Manufacturer' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'SupplierContactType', N'Manufacturer', N'Manufacturer', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAverageCostEnableInItemConsumption' AND SetupName = 'IsAverageCostEnableInItemConsumption' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAverageCostEnableInItemConsumption', N'IsAverageCostEnableInItemConsumption', N'0', N'IsAverageCostEnableInItemConsumption', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityUserGroupProjectMapping)
BEGIN
	DECLARE @UserGroupID INT
	DECLARE @getUserGroupID CURSOR
	SET @getUserGroupID = CURSOR FOR
	SELECT UserGroupId
	FROM SecurityUserGroup
	OPEN @getUserGroupID
	FETCH NEXT
	FROM @getUserGroupID INTO @UserGroupID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT @UserGroupID
		INSERT INTO SecurityUserGroupProjectMapping(ProjectId, UserGroupId)
		SELECT ProjectId, @UserGroupID FROM GLProject
		
	FETCH NEXT
	FROM @getUserGroupID INTO @UserGroupID
	END
	CLOSE @getUserGroupID
	DEALLOCATE @getUserGroupID
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRestaurantBillClassificationWiseDevideForFOAccountsVoucher' AND SetupName = 'IsRestaurantBillClassificationWiseDevideForFOAccountsVoucher' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsRestaurantBillClassificationWiseDevideForFOAccountsVoucher', N'IsRestaurantBillClassificationWiseDevideForFOAccountsVoucher', N'0', N'IsRestaurantBillClassificationWiseDevideForFOAccountsVoucher', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'ReservationConfirmationLetterCheckInHours' AND SetupName = 'ReservationConfirmationLetterCheckInHours' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'ReservationConfirmationLetterCheckInHours', N'ReservationConfirmationLetterCheckInHours', N'13:00 PM', N'ReservationConfirmationLetterCheckInHours', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'ReservationConfirmationLetterCheckOutHours' AND SetupName = 'ReservationConfirmationLetterCheckOutHours' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'ReservationConfirmationLetterCheckOutHours', N'ReservationConfirmationLetterCheckOutHours', N'12:00 PM', N'ReservationConfirmationLetterCheckOutHours', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRoomReservationConfirmationLetterWeAcceptHide' AND SetupName = 'IsRoomReservationConfirmationLetterWeAcceptHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsRoomReservationConfirmationLetterWeAcceptHide', N'IsRoomReservationConfirmationLetterWeAcceptHide', N'0', N'IsRoomReservationConfirmationLetterWeAcceptHide', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
UPDATE CommonCustomFieldData SET ActiveStat = '0' WHERE FieldType = 'LeaveMode' AND FieldValue = 'LWithoutP'
GO
IF EXISTS(SELECT * FROM CommonPaymentMode WHERE PaymentMode = 'MBanking' AND DisplayName = 'Mobile Banking' )
BEGIN
	UPDATE CommonPaymentMode SET PaymentMode = 'M-Banking' WHERE PaymentMode = 'MBanking' AND DisplayName = 'Mobile Banking'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsReceiveBillInfoHideInSupplierBillPayment' AND SetupName = 'IsReceiveBillInfoHideInSupplierBillPayment' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsReceiveBillInfoHideInSupplierBillPayment', N'IsReceiveBillInfoHideInSupplierBillPayment', N'0', N'IsReceiveBillInfoHideInSupplierBillPayment', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPaymentBillInfoHideInCompanyBillReceive' AND SetupName = 'IsPaymentBillInfoHideInCompanyBillReceive' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPaymentBillInfoHideInCompanyBillReceive', N'IsPaymentBillInfoHideInCompanyBillReceive', N'0', N'IsPaymentBillInfoHideInCompanyBillReceive', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM HotelCompanyPayment WHERE PaymentFor = 'Receive')
BEGIN
	UPDATE HotelCompanyPayment SET PaymentFor = 'Receive' WHERE PaymentFor = 'Payment'
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsRestaurantReportRescitionForAllUsers' AND SetupName = 'IsRestaurantReportRescitionForAllUsers' )
BEGIN
	UPDATE CommonSetup
	SET TypeName = 'IsRestaurantReportRestrictionForAllUser',
		SetupName = 'IsRestaurantReportRestrictionForAllUser'
	WHERE TypeName = 'IsRestaurantReportRescitionForAllUsers' AND SetupName = 'IsRestaurantReportRescitionForAllUsers'
END
GO
IF EXISTS(SELECT * FROM PayrollEmployee WHERE GLProjectId IS NULL)
BEGIN
	DECLARE @EmployeeID INT
	DECLARE @getEmployeeID CURSOR
	SET @getEmployeeID = CURSOR FOR
	SELECT EmpId
	FROM PayrollEmployee
	OPEN @getEmployeeID
	FETCH NEXT
	FROM @getEmployeeID INTO @EmployeeID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT @EmployeeID
		DECLARE @ProjectId INT = 1
		SELECT TOP 1 @ProjectId = glp.ProjectId 
		FROM PayrollEmployee pe
		INNER JOIN GLCompany glc ON pe.GlCompanyId = glc.CompanyId
		INNER JOIN GLProject GLP on glp.CompanyId = glc.CompanyId
		WHERE pe.EmpId = @EmployeeID
		
		UPDATE PayrollEmployee
		SET GLProjectId = @ProjectId
		WHERE EmpId = @EmployeeID
	FETCH NEXT
	FROM @getEmployeeID INTO @EmployeeID
	END
	CLOSE @getEmployeeID
	DEALLOCATE @getEmployeeID
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsLCNumberAutoGenerate' AND SetupName = 'IsLCNumberAutoGenerate' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsLCNumberAutoGenerate', N'IsLCNumberAutoGenerate', N'0', N'IsLCNumberAutoGenerate', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAutoSaveEnableUserGroupProjectMapping' AND SetupName = 'IsAutoSaveEnableUserGroupProjectMapping' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAutoSaveEnableUserGroupProjectMapping', N'IsAutoSaveEnableUserGroupProjectMapping', N'0', N'IsAutoSaveEnableUserGroupProjectMapping', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM PayrollAttendanceSystemConfig WHERE config_name = 'BUTTON_MODE')
INSERT [dbo].[PayrollAttendanceSystemConfig] ([config_name], [value]) VALUES ('BUTTON_MODE', 'IDLE');
GO
IF NOT EXISTS(SELECT * FROM PayrollAttendanceSystemConfig WHERE config_name = 'EXECUTION_INTERVAL')
INSERT [dbo].[PayrollAttendanceSystemConfig] ([config_name], [value]) VALUES ('EXECUTION_INTERVAL', '60000');
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SupplierUserGroupId' AND SetupName = 'SupplierUserGroupId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SupplierUserGroupId', N'SupplierUserGroupId', N'5000', N'SupplierUserGroupId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportCallInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Ticket Information',
	PageDisplayCaption = 'Ticket Information'
	WHERE PageId = 'SupportCallInformation'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportCallImplementation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Ticket Assignment',
	PageDisplayCaption = 'Ticket Assignment'
	WHERE PageId = 'SupportCallImplementation'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportCallBillingInformation')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Billing Information',
	PageDisplayCaption = 'Billing Information'
	WHERE PageId = 'SupportCallBillingInformation'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportPriceMatrixSetup')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Price Matrix Setup',
	PageDisplayCaption = 'Price Matrix Setup'
	WHERE PageId = 'SupportPriceMatrixSetup'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportPriceMatrixReport')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Price Matrix',
	PageDisplayCaption = 'Price Matrix'
	WHERE PageId = 'SupportPriceMatrixReport'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'SupportDashboardReport')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Ticket Information',
	PageDisplayCaption = 'Ticket Information'
	WHERE PageId = 'SupportDashboardReport'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsGLCompanyWiseCRMCompanyDifferent' AND SetupName = 'IsGLCompanyWiseCRMCompanyDifferent' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsGLCompanyWiseCRMCompanyDifferent', N'IsGLCompanyWiseCRMCompanyDifferent', N'0', N'IsGLCompanyWiseCRMCompanyDifferent', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmPayrollConfiguration')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Payroll Configuration',
	PageDisplayCaption = 'Payroll Configuration'
	WHERE PageId = 'frmPayrollConfiguration'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportCompanyAging')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 23, N'frmReportCompanyAging', N'Company A/R Aging', N'Company A/R Aging', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2019-03-05T15:39:34.887' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportSupplierAging')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ( [ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 13, N'frmReportSupplierAging', N'Supplier A/P Aging', N'Supplier A/P Aging', N'aspx', N'/PurchaseManagment/Reports', N'Report', NULL, 1, 1, CAST(N'2019-03-05T15:39:34.887' AS DateTime), NULL, NULL)
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmEmpTaxDeduction')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Tax Band',
	PageDisplayCaption = 'Tax Band'
	WHERE PageId = 'frmEmpTaxDeduction'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportSalesOrderInformation')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
 VALUES (23, N'frmReportSalesOrderInformation', N'Sales Order', N'Sales Order', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2020-01-06T11:51:40.207' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'AccountManagerSalesTarget')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 22, N'AccountManagerSalesTarget', N'Sales Target', N'Sales Target', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportSalesTargetAndAchievement')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
 VALUES (23, N'frmReportSalesTargetAndAchievement', N'Sales Target & Achievement', N'Sales Target & Achievement', N'aspx', N'/SalesAndMarketing/Reports', N'Report', NULL, 1, 1, CAST(N'2020-01-06T11:51:40.207' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmBothContribution')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (18, N'frmBothContribution', N'Both Contribution', N'Both Contribution', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemConsumptionDeliveryChallanEnable' AND SetupName = 'IsItemConsumptionDeliveryChallanEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemConsumptionDeliveryChallanEnable', N'IsItemConsumptionDeliveryChallanEnable', N'0', N'IsItemConsumptionDeliveryChallanEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollCompanyAndEmployeeCompanyDifferent' AND SetupName = 'IsPayrollCompanyAndEmployeeCompanyDifferent' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollCompanyAndEmployeeCompanyDifferent', N'IsPayrollCompanyAndEmployeeCompanyDifferent', N'0', N'IsPayrollCompanyAndEmployeeCompanyDifferent', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportABCAnalysis')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (18, N'frmReportABCAnalysis', N'ABC Analysis', N'ABC Analysis', N'aspx', N'/Inventory', N'Report', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BudgetType' AND FieldValue = 'Yearly' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BudgetType', N'Yearly', N'Yearly', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BudgetType' AND FieldValue = 'Half Yearly' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BudgetType', N'Half Yearly', N'Half Yearly', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BudgetType' AND FieldValue = 'Quarterly' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BudgetType', N'Quarterly', N'Quarterly', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BudgetType' AND FieldValue = 'Monthly' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BudgetType', N'Monthly', N'Monthly', 1)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportBudgetStatement')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (3, N'frmReportBudgetStatement', N'Budget Statement', N'Budget Statement', N'aspx', N'/GeneralLedger/Reports', N'Report', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollCompanyLoanHeadId' AND SetupName = 'PayrollCompanyLoanHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollCompanyLoanHeadId', N'PayrollCompanyLoanHeadId', N'5', N'PayrollCompanyLoanHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportXYZAnalysis')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (11, N'frmReportXYZAnalysis', N'XYZ Analysis', N'XYZ Analysis', N'aspx', N'/Inventory', N'Report', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmPaymentInstruction')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'frmPaymentInstruction', N'Payment Instruction', N'Payment Instruction', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsSupplierDifferentWithGLCompany' AND SetupName = 'IsSupplierDifferentWithGLCompany' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsSupplierDifferentWithGLCompany', N'IsSupplierDifferentWithGLCompany', N'0', N'IsSupplierDifferentWithGLCompany', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmInHouseGuestBreakfast')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'frmInHouseGuestBreakfast', N'In House Guest Breakfast', N'In House Guest Breakfast', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(N'2018-10-04T14:24:13.480' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'ShortTermLoanBankAccountHeadId' AND SetupName = 'ShortTermLoanBankAccountHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'ShortTermLoanBankAccountHeadId', N'ShortTermLoanBankAccountHeadId', N'0', N'ShortTermLoanBankAccountHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'LongTermLoanBankAccountHeadId' AND SetupName = 'LongTermLoanBankAccountHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'LongTermLoanBankAccountHeadId', N'LongTermLoanBankAccountHeadId', N'0', N'LongTermLoanBankAccountHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'PageWiseActivityLogDetailsSetup')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'PageWiseActivityLogDetailsSetup', N'Page Wise Activity Field Setup', N'Page Wise Activity Field Setup', N'aspx', N'/HMCommon', N'Page', NULL, 1, 1, CAST(0x0000A92301674CE8 AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsWorkHandoverEnableForLeaveApplication' AND SetupName = 'IsWorkHandoverEnableForLeaveApplication' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsWorkHandoverEnableForLeaveApplication', N'IsWorkHandoverEnableForLeaveApplication', N'0', N'IsWorkHandoverEnableForLeaveApplication', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'LeaveWorkHandoverInfo')
BEGIN
INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
	VALUES (18, N'LeaveWorkHandoverInfo', N'Leave Work Handover Info', N'Leave Work Handover Info', N'aspx', N'/Payroll', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBlankRegistrationButtonWillHide' AND SetupName = 'IsBlankRegistrationButtonWillHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBlankRegistrationButtonWillHide', N'IsBlankRegistrationButtonWillHide', N'0', N'IsBlankRegistrationButtonWillHide', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsReservationRoomListButtonWillHide' AND SetupName = 'IsReservationRoomListButtonWillHide' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsReservationRoomListButtonWillHide', N'IsReservationRoomListButtonWillHide', N'0', N'IsReservationRoomListButtonWillHide', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBankIntegratedWithAccounts' AND SetupName = 'IsBankIntegratedWithAccounts' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBankIntegratedWithAccounts', N'IsBankIntegratedWithAccounts', N'1', N'IsBankIntegratedWithAccounts', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DefaultFrontOfficeMealPlanHeadId' AND SetupName = 'DefaultFrontOfficeMealPlanHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DefaultFrontOfficeMealPlanHeadId', N'DefaultFrontOfficeMealPlanHeadId', N'0', N'DefaultFrontOfficeMealPlanHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'DefaultFrontOfficeMarketSegmentHeadId' AND SetupName = 'DefaultFrontOfficeMarketSegmentHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'DefaultFrontOfficeMarketSegmentHeadId', N'DefaultFrontOfficeMarketSegmentHeadId', N'0', N'DefaultFrontOfficeMarketSegmentHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmRoomCalender' AND PageName = 'Room Calender')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Room Calendar',
	PageDisplayCaption = 'Room Calendar'
	WHERE PageId = 'frmRoomCalender'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SMSAutoPosting' AND SetupName = 'IsRoomReservationSMSAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SMSAutoPosting', N'IsRoomReservationSMSAutoPostingEnable', N'0', N'IsRoomReservationSMSAutoPostingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmGuestPaymentTransfer')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'frmGuestPaymentTransfer', N'Guest Payment Transfer', N'Guest Payment Transfer', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(0x0000A96800DD53B5 AS DateTime), NULL, NULL)
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmServiceBillTransfer' AND PageName = 'Bill Transfer')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Guest Bill Transfer',
	PageDisplayCaption = 'Guest Bill Transfer'
	WHERE PageId = 'frmServiceBillTransfer'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmBillAdjustment' AND PageName = 'Bill Adjustment')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Guest Bill Adjustment',
	PageDisplayCaption = 'Guest Bill Adjustment'
	WHERE PageId = 'frmBillAdjustment'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'EmployeeSalarySummaryPostingAccountHeadId' AND SetupName = 'EmployeeSalarySummaryPostingAccountHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'EmployeeSalarySummaryPostingAccountHeadId', N'EmployeeSalarySummaryPostingAccountHeadId', N'10000', N'EmployeeSalarySummaryPostingAccountHeadId', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsReservationStatusWaitingEnable' AND SetupName = 'IsReservationStatusWaitingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsReservationStatusWaitingEnable', N'IsReservationStatusWaitingEnable', N'1', N'IsReservationStatusWaitingEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCompanyProjectWiseEmployeeSalaryProcessEnable' AND SetupName = 'IsCompanyProjectWiseEmployeeSalaryProcessEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCompanyProjectWiseEmployeeSalaryProcessEnable', N'IsCompanyProjectWiseEmployeeSalaryProcessEnable', N'0', N'IsCompanyProjectWiseEmployeeSalaryProcessEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPFEmpContbtnSetupEnableOnBasicForDependsOnGrossForSouthSudanTemp' AND SetupName = 'IsPFEmpContbtnSetupEnableOnBasicForDependsOnGrossForSouthSudanTemp' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPFEmpContbtnSetupEnableOnBasicForDependsOnGrossForSouthSudanTemp', N'IsPFEmpContbtnSetupEnableOnBasicForDependsOnGrossForSouthSudanTemp', N'0', N'IsPFEmpContbtnSetupEnableOnBasicForDependsOnGrossForSouthSudanTemp', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM PayrollAppraisalRatingScale WHERE RatingValue = -1)
BEGIN
	TRUNCATE TABLE PayrollAppraisalRatingScale
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'--- Please Select ---', 0, CAST(-1 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Failed', 1, CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Dissatisfactory', 0, CAST(2 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Moderately Satisfactory', 0, CAST(4 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Satisfactory', 0, CAST(6 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Excellent', 1, CAST(8 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
	INSERT [dbo].[PayrollAppraisalRatingScale] ([RatingScaleName], [IsRemarksMandatory], [RatingValue], [Remarks], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Outstanding', 1, CAST(10 AS Decimal(18, 0)), NULL, NULL, NULL, NULL, NULL)
END
GO
DELETE FROM SecurityMenuWiseLinks 
WHERE MenuWiseLinksId IN (
	SELECT MenuWiseLinksId 
	FROM (
			SELECT 
				MenuWiseLinksId, 
				MenuLinksId, 
				UserGroupId, 
				ROW_NUMBER() OVER(PARTITION BY MenuLinksId, UserGroupId ORDER BY MenuLinksId, UserGroupId) AS Rnk
			FROM SecurityMenuWiseLinks
	) tblInfo
WHERE Rnk <> 1)
GO
IF NOT EXISTS(SELECT * FROM SMContactDetailsTitle)
BEGIN
	TRUNCATE TABLE SMContactDetailsTitle
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Number', N'Official Mobile', 1, NULL, NULL, 1, CAST(N'2020-06-22T17:07:19.983' AS DateTime))
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Email', N'Info Email', 1, NULL, NULL, NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Fax', N'Office Fax', 1, NULL, NULL, NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Website', N'Website', 1, NULL, NULL, NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'SocialMedia', N'Facebook', 1, NULL, NULL, 1, CAST(N'2020-06-22T18:01:37.507' AS DateTime))
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Number', N'Corporate Sales', 1, NULL, NULL, NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Number', N'Personal Mobile', 1, 1, CAST(N'2020-06-22T17:11:30.367' AS DateTime), NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'Number', N'Office Land Phone', 1, 1, CAST(N'2020-06-22T17:14:25.487' AS DateTime), NULL, NULL)
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'SocialMedia', N'Skype', 1, 1, CAST(N'2020-06-22T18:01:10.277' AS DateTime), 1, CAST(N'2020-06-22T18:01:27.503' AS DateTime))
	INSERT [dbo].[SMContactDetailsTitle] ([TransectionType], [Title], [Status], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (N'SocialMedia', N'WhatsApp', 1, 1, CAST(N'2020-06-22T18:02:05.507' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'Production' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'Production', N'Production', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'CompanyPaymentReceive' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'CompanyPaymentReceive', N'Company Payment Receive', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'SupplierBillPayment' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'SupplierBillPayment', N'Supplier Bill Payment', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'SupplierCompanyBalanceTransfer' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'SupplierCompanyBalanceTransfer', N'Supplier Company Balance Transfer', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemAverageCostUpdateEnable' AND SetupName = 'IsItemAverageCostUpdateEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemAverageCostUpdateEnable', N'IsItemAverageCostUpdateEnable', N'0', N'IsItemAverageCostUpdateEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
--DELETE FROM InvItemStockInformation WHERE LocationId = 0
--DELETE FROM InvItemStockInformation WHERE CompanyId = 0
--DELETE FROM InvItemStockInformation WHERE ProjectId = 0
--UPDATE InvItemStockInformation SET ColorId = 0 WHERE ColorId IS NULL
--UPDATE InvItemStockInformation SET SizeId = 0 WHERE SizeId IS NULL
--UPDATE InvItemStockInformation SET StyleId = 0 WHERE StyleId IS NULL

--DELETE FROM InvItemStockInformation 
--WHERE StockId IN (
--	SELECT StockId 
--	FROM (
--			SELECT 
--				StockId,
--				ISNULL(CompanyId, 0) as CompanyId,
--				ISNULL(ProjectId, 0) as ProjectId,
--				LocationId, 
--				ItemId, 
--				ISNULL(ColorId, 0) as ColorId,
--				ISNULL(SizeId, 0) as SizeId,
--				ISNULL(StyleId, 0) as StyleId, 
--				ROW_NUMBER() OVER(PARTITION BY ISNULL(CompanyId, 0), ISNULL(ProjectId, 0), LocationId, ItemId ORDER BY ISNULL(CompanyId, 0), ISNULL(ProjectId, 0), LocationId, ItemId) AS Rnk
--			FROM InvItemStockInformation
--	) tblInfo
--WHERE Rnk <> 1)
--GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BillingType' AND FieldValue = 'Product' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BillingType', N'Product', N'Product', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'BillingType' AND FieldValue = 'Service' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'BillingType', N'Service', N'Service', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillingTypeEnable' AND SetupName = 'IsBillingTypeEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillingTypeEnable', N'IsBillingTypeEnable', N'0', N'IsBillingTypeEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'BankSalaryInstructionFormat' AND SetupName = 'BankSalaryInstructionFormat' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'BankSalaryInstructionFormat', N'BankSalaryInstructionFormat', N'1', N'BankSalaryInstructionFormat', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemReceiveOverheadExpenseEnable' AND SetupName = 'IsItemReceiveOverheadExpenseEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemReceiveOverheadExpenseEnable', N'IsItemReceiveOverheadExpenseEnable', N'0', N'IsItemReceiveOverheadExpenseEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCRMCompanyBranchCodeEnable' AND SetupName = 'IsCRMCompanyBranchCodeEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCRMCompanyBranchCodeEnable', N'IsCRMCompanyBranchCodeEnable', N'0', N'IsCRMCompanyBranchCodeEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsAccountNodesBreakdownDetailsLedgerEnable' AND SetupName = 'IsAccountNodesBreakdownDetailsLedgerEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsAccountNodesBreakdownDetailsLedgerEnable', N'IsAccountNodesBreakdownDetailsLedgerEnable', N'0', N'IsAccountNodesBreakdownDetailsLedgerEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM HotelRoomReservation)
BEGIN
	UPDATE CommonSetup SET SetupValue = '0' WHERE TypeName = 'IsRoomOverbookingEnable' AND SetupName = 'IsRoomOverbookingEnable'
END
GO
IF NOT EXISTS (SELECT * FROM SMCompanyTypeInformation)
BEGIN
	SET IDENTITY_INSERT [dbo].[SMCompanyTypeInformation] ON
	INSERT [dbo].[SMCompanyTypeInformation] ([Id], [TypeName], [Description], [IsLocalOrForeign], [Status], [IsDeleted], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'Local', NULL, 1, 1, 0, 1, CAST(N'2019-05-09T14:10:26.043' AS DateTime), NULL, NULL)
	INSERT [dbo].[SMCompanyTypeInformation] ([Id], [TypeName], [Description], [IsLocalOrForeign], [Status], [IsDeleted], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'Foreign', NULL, 2, 1, 0, 1, CAST(N'2019-05-09T14:10:33.810' AS DateTime), NULL, NULL)
	INSERT [dbo].[SMCompanyTypeInformation] ([Id], [TypeName], [Description], [IsLocalOrForeign], [Status], [IsDeleted], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (3, N'Both (Local & Foreign)', NULL, 3, 1, 0, 1, CAST(N'2019-05-09T14:10:40.737' AS DateTime), NULL, NULL)
	SET IDENTITY_INSERT [dbo].[SMCompanyTypeInformation] OFF
		
	UPDATE HotelGuestCompany SET CompanyType = 1

	DELETE FROM SecurityMenuWiseLinks
	WHERE MenuLinksId IN (SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'CompanyTypeInformation')
	AND UserGroupId <> 1

	UPDATE SecurityMenuLinks SET ActiveStat = 0 WHERE PageId = 'CompanyTypeInformation'
END
GO
IF NOT EXISTS (SELECT LedgerMasterId FROM GLLedgerMaster WHERE dbo.FnDate(VoucherDate) <= dbo.FnDate('2022-10-31') AND IsModulesTransaction = 1)
BEGIN
	UPDATE GLLedgerMaster SET IsModulesTransaction = 0 WHERE dbo.FnDate(VoucherDate) <= dbo.FnDate('2022-10-31')
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsItemTransactionLogEnable' AND SetupName = 'IsItemTransactionLogEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsItemTransactionLogEnable', N'IsItemTransactionLogEnable', N'0', N'IsItemTransactionLogEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportBCRoomSalesRevenue' )
BEGIN
	DELETE SecurityMenuWiseLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportBCRoomSalesRevenue')
	DELETE SecurityMenuLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportBCRoomSalesRevenue')
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportBCDivisionRevenue' )
BEGIN
	DELETE SecurityMenuWiseLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportBCDivisionRevenue')
	DELETE SecurityMenuLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportBCDivisionRevenue')
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportRoomSalesRevenue' )
BEGIN
	DELETE SecurityMenuWiseLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportRoomSalesRevenue')
	DELETE SecurityMenuLinks WHERE MenuLinksId IN(SELECT MenuLinksId FROM SecurityMenuLinks WHERE PageId = 'frmReportRoomSalesRevenue')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleType WHERE ModuleType = 'Airline Ticketing' )
BEGIN
	INSERT [dbo].[CommonModuleType] ([ModuleType]) VALUES ( N'Airline Ticketing')
END
GO
IF NOT EXISTS(SELECT * FROM CommonModuleName WHERE ModuleName = 'Airline Ticketing' )
BEGIN
	INSERT [dbo].[CommonModuleName] ( [TypeId], [ModuleName], [GroupName], [ModulePath], [IsReportType], [ActiveStat]) VALUES ( 18, N'Airline Ticketing', N'Airline Ticketing', N'/AirTicketing/', 0, 1)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuGroup WHERE MenuGroupName = 'Airline Ticketing')
BEGIN
	 INSERT [dbo].[SecurityMenuGroup] ([MenuGroupName], [GroupDisplayCaption], [DisplaySequence], [GroupIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'Airline Ticketing', N'Airline Ticketing', 1, N'icon-group', 1, NULL, NULL, NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'AirlineTicket' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'AirlineTicket', N'Airline Ticket', 1)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmAirlineTicketInfo')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (40, N'frmAirlineTicketInfo', N'Airline Ticket Info', N'Airline Ticket Info', N'aspx', N'/AirTicketing', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmNutrientInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmNutrientInformation', N'Nutrient Information', N'Nutrient Information', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollEmployeeRepotingToEnableForApproval' AND SetupName = 'IsPayrollEmployeeRepotingToEnableForApproval' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollEmployeeRepotingToEnableForApproval', N'IsPayrollEmployeeRepotingToEnableForApproval', N'1', N'IsPayrollEmployeeRepotingToEnableForApproval', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmNutritionValue')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmNutritionValue', N'Nutrition Value', N'Nutrition Value', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsProjectCodeAutoGenerate' AND SetupName = 'IsProjectCodeAutoGenerate' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsProjectCodeAutoGenerate', N'IsProjectCodeAutoGenerate', N'1', N'PC', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportItemRecipe')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Ingredients Information',
	PageDisplayCaption = 'Ingredients Information'
	WHERE PageId = 'frmReportItemRecipe'
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'RecipeModifierType')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Ingredients Modifier',
	PageDisplayCaption = 'Ingredients Modifier'
	WHERE PageId = 'RecipeModifierType'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmNutrientRequiredValues')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmNutrientRequiredValues', N'Nutrient Required Values', N'Nutrient Required Values', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportNutrientInformation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 11, N'frmReportNutrientInformation', N'Nutrient Information', N'Nutrient Information', N'aspx', N'/Inventory/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmSupplierCompanyBalanceTransfer')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Accounts Balance Transfer',
	PageDisplayCaption = 'Accounts Balance Transfer'
	WHERE PageId = 'frmSupplierCompanyBalanceTransfer'
END
GO
IF EXISTS (SELECT FieldId  FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'SupplierCompanyBalanceTransfer' AND [Description] = 'Supplier Company Balance Transfer')
BEGIN
	UPDATE CommonCustomFieldData
	SET Description = 'Accounts Balance Transfer'
	WHERE FieldType = 'InnboardFeatures'
	AND FieldValue = 'SupplierCompanyBalanceTransfer'
	AND [Description] = 'Supplier Company Balance Transfer'
END
GO
IF NOT EXISTS(SELECT * FROM InvItemStockInformationLog WHERE ISNULL(AverageCost, 0) > 0)
BEGIN
	UPDATE isil
	SET isil.AverageCost = ISNULL(ii.AverageCost, 0)
	FROM InvItemStockInformationLog isil
	INNER JOIN InvItem ii ON isil.ItemId = ii.ItemId
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'InternationalTicketTypeAccountsPostingHeadId' AND SetupName = 'InternationalTicketTypeAccountsPostingHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'InternationalTicketTypeAccountsPostingHeadId', N'InternationalTicketTypeAccountsPostingHeadId', N'5000', N'International Ticket Type Accounts Posting Head Id', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS(SELECT * FROM PayrollEducationLevel WHERE LevelName = 'Maters')
BEGIN
	UPDATE PayrollEducationLevel SET LevelName = 'Masters' WHERE LevelName = 'Maters'
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmInvIngredientNNutrientInfo')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (10, N'frmInvIngredientNNutrientInfo', N'Ingredient And Nutrient Information', N'Ingredient And Nutrient Information', N'aspx', N'/Inventory', N'Page', NULL, 1, 1, CAST(0x0000A5F301668BDC AS DateTime), NULL, NULL)
END
GO
IF EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageDisplayCaption <> PageName)
BEGIN
	UPDATE SecurityMenuLinks SET PageName = PageDisplayCaption WHERE PageDisplayCaption <> PageName
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCompanyAccountApproval')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (22, N'frmCompanyAccountApproval', N'Company Account Approval', N'Company Account Approval', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2023-02-27T12:47:39.633' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'RawMaterialsCategoryIdList' AND SetupName = 'RawMaterialsCategoryIdList' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'RawMaterialsCategoryIdList', N'RawMaterialsCategoryIdList', N'0', N'RawMaterialsCategoryIdList', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'FinishedGoodsCategoryIdList' AND SetupName = 'FinishedGoodsCategoryIdList' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'FinishedGoodsCategoryIdList', N'FinishedGoodsCategoryIdList', N'0', N'FinishedGoodsCategoryIdList', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'Overtime' AND SetupName = 'Overtime Setup' )
BEGIN
	UPDATE CommonSetup
	SET TypeName = 'OvertimeProcessDependsOn',
		SetupName = 'OvertimeProcessDependsOn',
		SetupValue = 'Gross'
	WHERE TypeName = 'Overtime' AND SetupName = 'Overtime Setup'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsOvertimeAutoApprovalProcessEnable' AND SetupName = 'IsOvertimeAutoApprovalProcessEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsOvertimeAutoApprovalProcessEnable', N'IsOvertimeAutoApprovalProcessEnable', N'0', N'O FOR No, 1 FOR Yes', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollOvertimeAllowanceTimesBenefit' AND SetupName = 'PayrollOvertimeAllowanceTimesBenefit' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollOvertimeAllowanceTimesBenefit', N'PayrollOvertimeAllowanceTimesBenefit', N'1', N'Ex: (Basic/208) * Variable', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollOvertimeEffectiveFor' AND SetupName = 'PayrollOvertimeEffectiveFor' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollOvertimeEffectiveFor', N'PayrollOvertimeEffectiveFor', N'All', N'Ex: All/ EmpGrade~1,2,3,4,5', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollAttendanceBonusHeadId' AND SetupName = 'PayrollAttendanceBonusHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollAttendanceBonusHeadId', N'PayrollAttendanceBonusHeadId', N'0', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsPayrollAttendancePartWillShowOnSalarySheet' AND SetupName = 'IsPayrollAttendancePartWillShowOnSalarySheet' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsPayrollAttendancePartWillShowOnSalarySheet', N'IsPayrollAttendancePartWillShowOnSalarySheet', N'0', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'CompanyAdvanceAdjustment' )
BEGIN
	INSERT [dbo].[CommonCustomFieldData] ([FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'CompanyAdvanceAdjustment', N'Company Advance Adjustment', 1)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmCostCenterSelectionForSalesOrder')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (22, N'frmCostCenterSelectionForSalesOrder', N'Sales Order', N'Sales Order', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-04T12:47:39.633' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmSalesOrderSearch')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (22, N'frmSalesOrderSearch', N'Sales Order Search', N'Sales Order Search', N'aspx', N'/SalesAndMarketing', N'Page', NULL, 1, 1, CAST(N'2019-03-04T12:47:39.633' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'InnboardFeatures' AND FieldValue = 'SalesOrder' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'InnboardFeatures', N'SalesOrder', N'Sales Order', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollProvidentFundTitleText' AND SetupName = 'PayrollProvidentFundTitleText' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollProvidentFundTitleText', N'PayrollProvidentFundTitleText', N'PF', N'PayrollProvidentFundTitleText', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
BEGIN
	DECLARE @PayrollProvidentFundTitleText VARCHAR(300)
	SELECT @PayrollProvidentFundTitleText = ISNULL(SetupValue, 'PF')
	FROM CommonSetup 
	WHERE TypeName = 'PayrollProvidentFundTitleText' AND SetupName = 'PayrollProvidentFundTitleText'
	UPDATE SecurityMenuGroup SET MenuGroupName = @PayrollProvidentFundTitleText, GroupDisplayCaption = @PayrollProvidentFundTitleText WHERE MenuGroupId = 27
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Approval', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Approval' WHERE PageId = 'frmEmpPFApproval'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Opening Balance', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Opening Balance' WHERE PageId = 'frmPFOpeningBalance'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Member List', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Member List' WHERE PageId = 'frmReportPFMemberSchedule'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Statement', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Statement' WHERE PageId = 'frmReportPFStatement'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Monthly Balance', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Monthly Balance' WHERE PageId = 'frmReportPFMonthlyBalance'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Loan Collection', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Loan Collection' WHERE PageId = 'frmReportPFLoanCollection'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Reports', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Reports' WHERE PageId = 'frmReportPFReports'
	UPDATE SecurityMenuLinks SET PageName = @PayrollProvidentFundTitleText + ' Configuration', PageDisplayCaption = @PayrollProvidentFundTitleText + ' Configuration' WHERE PageId = 'frmPFConfiguration'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollEmpAttendanceLateGraceMinutes' AND SetupName = 'PayrollEmpAttendanceLateGraceMinutes' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollEmpAttendanceLateGraceMinutes', N'PayrollEmpAttendanceLateGraceMinutes', N'0', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollAttendanceAbsentHeadId' AND SetupName = 'PayrollAttendanceAbsentHeadId' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollAttendanceAbsentHeadId', N'PayrollAttendanceAbsentHeadId', N'0', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmGroupRoomReservation')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 4, N'frmGroupRoomReservation', N'Group Reservation', N'Group Reservation', N'aspx', N'/HotelManagement', N'Page', NULL, 1, 1, CAST(N'2019-05-06T12:31:29.947' AS DateTime), NULL, NULL)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'PayrollEmployeeRosterDaySetup' AND SetupName = 'PayrollEmployeeRosterDaySetup' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'PayrollEmployeeRosterDaySetup', N'PayrollEmployeeRosterDaySetup', N'7', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'GroupRoomReservationTermsAndConditions')
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'GroupRoomReservationTermsAndConditions', N'...', N'...', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'GroupReservationBackDayLimit' AND SetupName = 'GroupReservationBackDayLimit' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'GroupReservationBackDayLimit', N'GroupReservationBackDayLimit', N'300', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'AboutUs' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'AboutUs', N'About Us', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'User' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'User', N'User', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'GuestRoomReservation' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'GuestRoomReservation', N'Guest', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'MemberRoomReservation' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'MemberRoomReservation', N'Member', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'RoomReservation' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'RoomReservation', N'Room Reservation', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonCustomFieldData WHERE FieldType = 'MobileAppsFeatures' AND FieldValue = 'BecomeAMember' )
BEGIN
 INSERT [dbo].[CommonCustomFieldData] ( [FieldType], [FieldValue], [Description], [ActiveStat]) VALUES ( N'MobileAppsFeatures', N'BecomeAMember', N'Become a Member', 1)
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportAccountComparison')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Comparative Accounts',
	PageDisplayCaption = 'Comparative Accounts'
	WHERE PageId = 'frmReportAccountComparison'
END
GO
IF NOT EXISTS(SELECT * FROM HotelRoomStatusPossiblePathHead WHERE PossiblePath = '/HotelManagement/GuestBillSplit.aspx' )
BEGIN
	INSERT [dbo].[HotelRoomStatusPossiblePathHead] ([PossiblePath], [DisplayText], [ActiveStat]) VALUES (N'/HotelManagement/GuestBillSplit.aspx', N'Bill Split', 1)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'MembershipMemberCodePrefix' AND SetupName = 'MembershipMemberCodePrefix' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'MembershipMemberCodePrefix', N'MembershipMemberCodePrefix', N'MN', N'', 1, CAST(N'2019-04-04T11:40:11.130' AS DateTime), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'CurrencyExchangeInvoiceFormat' AND SetupName = 'CurrencyExchangeInvoiceFormat' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'CurrencyExchangeInvoiceFormat', N'CurrencyExchangeInvoiceFormat', N'1', N'', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM SecurityMenuLinks WHERE PageId = 'frmReportPickupDropForecastRevenueInfo')
BEGIN
	INSERT [dbo].[SecurityMenuLinks] ([ModuleId], [PageId], [PageName], [PageDisplayCaption], [PageExtension], [PagePath], [PageType], [LinkIconClass], [ActiveStat], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( 5, N'frmReportPickupDropForecastRevenueInfo', N'Pick Up/ Drop Off Forecast Revenue', N'Pick Up/ Drop Off Forecast Revenue', N'aspx', N'/HotelManagement/Reports', N'Report', NULL, 1, 1, CAST(0x0000A9E400FC82A1 AS DateTime), 1, CAST(0x0000A9E400FD2C2F AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmCurrencyTransaction')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'Currency Exchange',
	PageDisplayCaption = 'Currency Exchange'
	WHERE PageId = 'frmCurrencyTransaction'
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBanquetInternalMeetingEmailSendEnable' AND SetupName = 'IsBanquetInternalMeetingEmailSendEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBanquetInternalMeetingEmailSendEnable', N'IsBanquetInternalMeetingEmailSendEnable', N'0', N'', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF EXISTS (SELECT PageId  FROM SecurityMenuLinks WHERE PageId = 'frmReportInHouseGuestLedger')
BEGIN
	UPDATE SecurityMenuLinks
	SET PageName = 'In House Guest Report',
	PageDisplayCaption = 'In House Guest Report',
	PageId = 'frmReportInHouseGuestInfo'
	WHERE PageId = 'frmReportInHouseGuestLedger'
END
GO
IF NOT EXISTS(SELECT * FROM CommonPaymentMode WHERE PaymentMode = 'SSLCOMMERZ')
BEGIN
	INSERT [dbo].[CommonPaymentMode] ([AncestorId], [PaymentMode], [DisplayName], [PaymentCode], [Hierarchy], [Lvl], [HierarchyIndex], [PaymentAccountsPostingId], [ReceiveAccountsPostingId], [ActiveStat]) VALUES (1, N'SSLCOMMERZ', N'SSLCOMMERZ', N'ssl', N'.20.', 0, N'.00020.', 31, 31, 1)
END
GO
IF EXISTS(SELECT * FROM HotelSalesSummary WHERE ISNULL(TotalRoomQuantity, 0) = 0 )
BEGIN
	DECLARE @WithoutTotalPMDummyRoom INT
	SELECT @WithoutTotalPMDummyRoom = COUNT(RoomId)
	FROM   HotelRoomNumber
	WHERE ISNULL(IsPMDummyRoom, 0) = 0
	
	UPDATE HotelSalesSummary
	SET TotalRoomQuantity = @WithoutTotalPMDummyRoom
	WHERE ISNULL(TotalRoomQuantity, 0) = 0
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsGroupCompanyMultipleBillPaymentReceiveEnable' AND SetupName = 'IsGroupCompanyMultipleBillPaymentReceiveEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsGroupCompanyMultipleBillPaymentReceiveEnable', N'IsGroupCompanyMultipleBillPaymentReceiveEnable', N'0', N'IsGroupCompanyMultipleBillPaymentReceiveEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsBillingInvoiceDueSectionEnable' AND SetupName = 'IsBillingInvoiceDueSectionEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsBillingInvoiceDueSectionEnable', N'IsBillingInvoiceDueSectionEnable', N'1', N'IsBillingInvoiceDueSectionEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsMemberPasswordPanalEnable' AND SetupName = 'IsMemberPasswordPanalEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsMemberPasswordPanalEnable', N'IsMemberPasswordPanalEnable', N'0', N'IsMemberPasswordPanalEnable', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCompanyAndProjectEnableOnCompanyBillReceive' AND SetupName = 'IsCompanyAndProjectEnableOnCompanyBillReceive' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCompanyAndProjectEnableOnCompanyBillReceive', N'IsCompanyAndProjectEnableOnCompanyBillReceive', N'0', N'IsCompanyAndProjectEnableOnCompanyBillReceive', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'RoomReservationSMSDocumentsMessage' AND SetupName = 'RoomReservationSMSDocumentsMessage' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'RoomReservationSMSDocumentsMessage', N'RoomReservationSMSDocumentsMessage', N'DocumentsMessage', N'Please provide all of your NID/ Driving License/ Passport Copy at the time of check-in.', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SMSAutoPosting' AND SetupName = 'IsRoomGuestCheckOutSMSAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SMSAutoPosting', N'IsRoomGuestCheckOutSMSAutoPostingEnable', N'0', N'1: Yes, 0: No', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SMSAutoPosting' AND SetupName = 'IsMemberRegistrationSMSAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SMSAutoPosting', N'IsMemberRegistrationSMSAutoPostingEnable', N'0', N'1: Yes, 0: No', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'SMSAutoPosting' AND SetupName = 'IsMemberActivationConfirmationSMSAutoPostingEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'SMSAutoPosting', N'IsMemberActivationConfirmationSMSAutoPostingEnable', N'0', N'1: Yes, 0: No', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO
IF NOT EXISTS(SELECT * FROM CommonSetup WHERE TypeName = 'IsCurrentCurrencyExchangeRateEnable' AND SetupName = 'IsCurrentCurrencyExchangeRateEnable' )
BEGIN
	INSERT [dbo].[CommonSetup] ([TypeName], [SetupName], [SetupValue], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES ( N'IsCurrentCurrencyExchangeRateEnable', N'IsCurrentCurrencyExchangeRateEnable', N'1', N'1: Yes, 0: No', 1, CAST(N'2018-03-21T11:40:11.130' AS DateTime), 0, CAST(N'2018-05-17T17:57:09.330' AS DateTime))
END
GO