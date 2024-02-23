USE master;

DROP DATABASE IF EXISTS CoffeeCatDB;

CREATE DATABASE CoffeeCatDB;

USE CoffeeCatDB;

DROP TABLE IF EXISTS customer;
DROP TABLE IF EXISTS shop;
DROP TABLE IF EXISTS area;
DROP TABLE IF EXISTS "table";
DROP TABLE IF EXISTS reservation;
DROP TABLE IF EXISTS reservationTable;
DROP TABLE IF EXISTS cat;
DROP TABLE IF EXISTS areaCat;
DROP TABLE IF EXISTS role;
DROP TABLE IF EXISTS staff;
DROP TABLE IF EXISTS promotion;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS bill;
DROP TABLE IF EXISTS billProduct;

CREATE TABLE customer (
    customerID INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50),
    phone VARCHAR(20),
    email VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(50) NOT NULL,
    status INT NOT NULL CHECK (status IN (0, 1, 2)) -- 0 = inactive, 1 = active, 2 = not-activated
);

CREATE TABLE shop (
	shopID INT IDENTITY(1, 1) PRIMARY KEY,
	name nvarchar(50),
	location nvarchar(100),
	description nvarchar(255),
	contactNumber varchar(20)
);

CREATE TABLE area (
	areaID INT IDENTITY(1, 1) PRIMARY KEY,
	location nvarchar(100) NOT NULL,
	shopID INT REFERENCES shop(shopID)
);

CREATE TABLE "table" (
	tableID INT IDENTITY(1, 1) PRIMARY KEY,
	status INT CHECK (status IN (0, 1)) NOT NULL, -- 0 = cancelled, 1 = booked
	areaID INT REFERENCES area(areaID)
);

CREATE TABLE reservation (
    reservationID INT IDENTITY(1, 1) PRIMARY KEY,
    bookingDay DATE NOT NULL,
	startTime TIME NOT NULL,
	endTime TIME NOT NULL,
    status INT CHECK (status IN (0, 1)) NOT NULL, -- 0 = cancelled, 1 = booked
	totalPrice DECIMAL(10, 2),
    customerID INT REFERENCES customer(customerID)
);

CREATE TABLE reservationTable (
	reservationTableID INT IDENTITY(1, 1) PRIMARY KEY,
    reservationID INT REFERENCES reservation(reservationID),
    tableID INT REFERENCES "table"(tableID)
);

CREATE TABLE cat (
    catID INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    gender INT CHECK (gender IN (0, 1)), -- 0 = meow, 1 = femeow
    breed NVARCHAR(50) NOT NULL,
	birthday date NOT NULL,
    healthStatus NVARCHAR(255) NOT NULL,
    shopID INT REFERENCES shop(shopID)
);

CREATE TABLE areaCat (
	areaCatID INT IDENTITY(1, 1) PRIMARY KEY,
	areaID INT REFERENCES area(areaID),
	catID INT REFERENCES cat(catID),
	date DATE NOT NULL
);

CREATE TABLE role (
    roleId INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
	description NVARCHAR(255)
);

CREATE TABLE staff (
    staffID INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    gender INT CHECK (gender IN (0, 1)) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(50) NOT NULL,
    status INT NOT NULL CHECK (status IN (0, 1)), -- 0 = inactive, 1 = active
    roleId INT REFERENCES role(roleId),
	shopID INT REFERENCES shop(shopID)
);

CREATE TABLE product (
    productId INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255),
    price DECIMAL(10, 2) NOT NULL,
    quantity INT NOT NULL,
	shopID INT REFERENCES shop(shopID)
);

CREATE TABLE promotion (
    promotionID INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255),
    promotionType INT CHECK (promotionType IN (0, 1)) NOT NULL, -- 0 = Flat, 1 = Percentage
    promotionAmount INT NOT NULL
);

CREATE TABLE bill (
    billID INT IDENTITY(1, 1) PRIMARY KEY,
    totalPrice DECIMAL(10, 2) NOT NULL,
	status INT CHECK (status IN (0, 1)) NOT NULL, -- 0 = on-going, 1 = paid
	paymentTime datetime NOT NULL,
    note NVARCHAR(200),
	reservationID INT REFERENCES reservation(reservationID),
	staffID INT REFERENCES staff(staffID),
	promotionID INT REFERENCES promotion(promotionID)
);

CREATE TABLE billProduct (
    billProductID INT PRIMARY KEY IDENTITY(1, 1),
    quantity INT NOT NULL,
    billID INT REFERENCES bill(billID),
    productID INT REFERENCES product(productID),
);

GO
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id =  OBJECT_ID(N'[dbo].[CalculateTotalPrice]'))
BEGIN
    DROP TRIGGER [dbo].[CalculateTotalPrice]
END

GO
CREATE OR ALTER TRIGGER CalculateTotalPrice
ON reservation
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @reservationID INT,
            @bookingDay DATE,
            @startTime TIME,
            @endTime TIME,
            @totalPrice DECIMAL(10, 2),
            @customerID INT

    SELECT @reservationID = reservationID,
           @bookingDay = bookingDay,
           @startTime = startTime,
           @endTime = endTime,
           @customerID = customerID
    FROM inserted

    DECLARE @bookingDurationInMinutes INT
    SET @bookingDurationInMinutes = DATEDIFF(MINUTE, @startTime, @endTime)

    -- Round up the booking duration to the nearest hour
    IF @bookingDurationInMinutes % 60 <> 0
        SET @bookingDurationInMinutes = (@bookingDurationInMinutes / 60 + 1) * 60

    -- Calculate the total price based on booking duration
    SET @totalPrice = @bookingDurationInMinutes / 60 * 50000.00

    -- Update the totalPrice for the reservation
    UPDATE reservation
    SET totalPrice = @totalPrice
    WHERE reservationID = @reservationID
END
