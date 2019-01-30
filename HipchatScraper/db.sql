USE master;

IF NOT EXISTS (   SELECT *
                  FROM   sys.databases
                  WHERE  name = 'Hipchat')
    CREATE DATABASE Hipchat;
GO

USE Hipchat;

IF NOT EXISTS (   SELECT *
                  FROM   sys.objects
                  WHERE  object_id = OBJECT_ID(N'[dbo].[Messages]')
                         AND type IN ( N'U' ))
    BEGIN
        CREATE TABLE [dbo].[Messages](
			[id] [VARCHAR](256) NOT NULL,
			[date] [DATETIMEOFFSET](7) NOT NULL,
			[from_id] [BIGINT] NOT NULL,
			[from_name] [VARCHAR](256) NOT NULL,
			[message] [NVARCHAR](MAX) NULL,
			[raw] [NVARCHAR](MAX) NULL,
			[file_name] [NVARCHAR](256) NULL,
			[file_size] [BIGINT] NULL,
			[file_url] [NVARCHAR](MAX) NULL,
			[room_id] [BIGINT] NULL
		 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
		(
			[id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    END


IF NOT EXISTS (   SELECT *
                  FROM   sys.objects
                  WHERE  object_id = OBJECT_ID(N'[dbo].[Users]')
                         AND type IN ( N'U' ))
    BEGIN
        CREATE TABLE dbo.Users
            (
                id bigint NOT NULL,
				name NVARCHAR(256) NOT NULL,
				mention_name NVARCHAR(256) NOT NULL,
                CONSTRAINT PK_Users
                    PRIMARY KEY CLUSTERED (id ASC)                    
            )
    END;


IF NOT EXISTS (   SELECT *
                  FROM   sys.objects
                  WHERE  object_id = OBJECT_ID(N'[dbo].[Rooms]')
                         AND type IN ( N'U' ))
    BEGIN
        CREATE TABLE dbo.Rooms
            (
                room_id BIGINT NOT NULL,
                name NVARCHAR(256) NOT NULL,
                privacy NVARCHAR(256) NOT NULL,
                messages_sent BIGINT NULL,
                last_active DATETIMEOFFSET(7) NULL,
                CONSTRAINT PK_rooms
                    PRIMARY KEY CLUSTERED (room_id ASC)
                    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY];
    END;