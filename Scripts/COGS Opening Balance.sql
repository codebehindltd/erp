
--SET IDENTITY_INSERT [dbo].[InvCogsClosing] ON
INSERT [dbo].[InvCogsClosing] ([CogsClosingDate], [LocationId], [Opening], [Purchase], [Overhead], [Closing], [CogsAmount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (GETDATE()-1, 1, 0, 0, 0.0000, 0, 0.0000, 1, CAST(0x0000A6D60110EB72 AS DateTime), 1, CAST(0x0000A6D601118398 AS DateTime))
INSERT [dbo].[InvCogsClosing] ([CogsClosingDate], [LocationId], [Opening], [Purchase], [Overhead], [Closing], [CogsAmount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (GETDATE(), 1, 0, 0, 0.0000, 0, 0.0000, 1, CAST(0x0000A6D60110EB72 AS DateTime), 1, CAST(0x0000A6D601118398 AS DateTime))
--SET IDENTITY_INSERT [dbo].[InvCogsClosing] OFF
