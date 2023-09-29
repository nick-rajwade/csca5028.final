USE [store_db]
GO

/****** Object: Table [dbo].[Stores] Script Date: 9/25/2023 2:13:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Stores];


GO
DROP TABLE [dbo].[StoreLocation];


GO

CREATE TABLE [dbo].[StoreLocation]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	store_address VARCHAR(100) NOT NULL,
	store_city VARCHAR(50) NOT NULL,
	store_state VARCHAR(50) NOT NULL,
	store_zip VARCHAR(10) NOT NULL,
	store_country VARCHAR(50) NOT NULL,
	lat DECIMAL(9,6) NOT NULL,
	long DECIMAL(9,6) NOT NULL
	PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Stores]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	store_name NVARCHAR(50) NOT NULL,
	healthcheckurl NVARCHAR(200) NOT NULL,
	healthcheckinterval INT NOT NULL,
	store_location_id INT NOT NULL, -- FK to StoreLocation.Id
	CONSTRAINT FK_Stores_StoreLocation FOREIGN KEY (store_location_id) REFERENCES StoreLocation(Id)
);

-- Path: components/dbscripts/dbo.StoreLocation.sql

--generate 10 store locations across the united states
INSERT INTO [dbo].[StoreLocation] (store_address, store_city, store_state, store_zip, store_country, lat, long)
VALUES ('123 Main St', 'New York', 'NY', '10001', 'USA', 40.7128, -74.0060),
('456 Main St', 'Los Angeles', 'CA', '90001', 'USA', 34.0522, -118.2437),
('789 Main St', 'Chicago', 'IL', '60007', 'USA', 41.8781, -87.6298),
('101 Main St', 'Houston', 'TX', '77001', 'USA', 29.7604, -95.3698),
('112 Main St', 'Phoenix', 'AZ', '85001', 'USA', 33.4484, -112.0740),
('131 Main St', 'Philadelphia', 'PA', '19019', 'USA', 39.9526, -75.1652),
('415 Main St', 'San Antonio', 'TX', '78201', 'USA', 29.4241, -98.4936),
('161 Main St', 'San Diego', 'CA', '92101', 'USA', 32.7157, -117.1611),
('718 Main St', 'Dallas', 'TX', '75201', 'USA', 32.7767, -96.7970),
('919 Main St', 'San Jose', 'CA', '95101', 'USA', 37.3382, -121.8863);

--generate 10 stores
INSERT INTO [dbo].[Stores] (store_name, healthcheckurl, healthcheckinterval, store_location_id)
VALUES ('New York Store', 'new-york-pos', 5, 1),
('Los Angeles Store', 'la-pos', 5, 2),
('Chicago Store', 'chicago-pos', 5, 3),
('Houston Store', 'houston-pos', 5, 4),
('Phoenix Store', 'phoenix-pos', 5, 5),
('Philadelphia Store', 'philly-pos', 5, 6),
('San Antonio Store', 'sanantonio-pos', 5, 7),
('San Diego Store', 'sandiego-pos', 5, 8),
('Dallas Store', 'dallas-pos', 5, 9),
('San Jose Store', 'sanjose-pos', 5, 10);

CREATE TABLE [dbo].[Sales]
(
	[saleId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[store_id] UNIQUEIDENTIFIER NOT NULL,
	[loyalty_card] bit NOT NULL,
	[payment_type] varchar(10) NOT NULL CHECK (payment_type IN ('Cash', 'Creditcard', 'Check')),
	[total_items] INT NOT NULL,
	[total_price] DECIMAL(10,2) NOT NULL,
	[json_item_list] varchar(max) NOT NULL,
	[created_at] DATETIME NOT NULL,
	[updated_at] DATETIME NOT NULL DEFAULT GETDATE(),
	[CC_AUTH] varchar(7) NULL CHECK (CC_AUTH IN ('AUTH','DECLINE')),
	[CC_AUTH_CODE] varchar(10) NULL);
GO;

