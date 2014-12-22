CREATE TABLE [adis].[ADIS_SECURABLE](
	[SECURABLE_GU] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[SECURITY_GROUP_GU] [nchar](10) NOT NULL,
	[SECURABLE_OBJECT_GU] [uniqueidentifier] NOT NULL,
	[ACCESS_TYPE] [smallint] NOT NULL,
	[SECURABLE_PARENT_GU] [uniqueidentifier] NULL,
	[TYPE] [nvarchar](50) NOT NULL);
 