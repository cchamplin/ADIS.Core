CREATE TABLE [adis].[ADIS_USER_AVAILABILITY](
	[AVAILABILITY_GU] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[USER_GU] [uniqueidentifier] NULL,
	[ALLOW] [bit] NOT NULL,
	[START_TIME] [time](7) NOT NULL,
	[DAY] [tinyint] NULL,
	[END_TIME] [time](7) NOT NULL,
	[START_DATE] [date] NULL,
	[END_DATE] [date] NULL);