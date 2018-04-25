INSERT [dbo].[AspNetUsers] ([Id], [Domain], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FullName], [IsSubscribed]) 
VALUES (N'732c0209-271d-4009-8af0-97003c2b719e', 'VessiFlowers.Web', N'vessi.flowers@gmail.com', 0, N'AGcfvyJN73VKSca/ybApsSeykvzF1wfI2uIjvP2xAKSBBvYhaLXidhwhnQ3LgcLEZA==', N'2e6d8808-90dd-4e7a-9e4e-6ea9320e5bd9', NULL, 0, 0, NULL, 0, 0, N'vessi.flowers@gmail.com', N'Василена Янчева', 1)

INSERT [dbo].[AspNetRoles] (Id, Name) VALUES ('732c0209-271d-4009-8af0-97003c2b719e', 'Admin')

INSERT [dbo].[AspNetUserRoles] (UserId, RoleId) VALUES ('732c0209-271d-4009-8af0-97003c2b719e', '732c0209-271d-4009-8af0-97003c2b719e')