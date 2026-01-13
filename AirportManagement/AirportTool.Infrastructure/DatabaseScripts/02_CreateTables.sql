USE AirportManagement;

IF OBJECT_ID('dbo.Ticket', 'U') IS NOT NULL DROP TABLE dbo.Ticket;
IF OBJECT_ID('dbo.Booking', 'U') IS NOT NULL DROP TABLE dbo.Booking;
IF OBJECT_ID('dbo.FlightSchedule', 'U') IS NOT NULL DROP TABLE dbo.FlightSchedule;
IF OBJECT_ID('dbo.Flight', 'U') IS NOT NULL DROP TABLE dbo.Flight;
IF OBJECT_ID('dbo.Gate', 'U') IS NOT NULL DROP TABLE dbo.Gate;
IF OBJECT_ID('dbo.FlightStatus', 'U') IS NOT NULL DROP TABLE dbo.FlightStatus;
IF OBJECT_ID('dbo.BookingStatus', 'U') IS NOT NULL DROP TABLE dbo.BookingStatus;
IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL DROP TABLE dbo.[User];
IF OBJECT_ID('dbo.Aircraft', 'U') IS NOT NULL DROP TABLE dbo.Aircraft;
IF OBJECT_ID('dbo.Airline', 'U') IS NOT NULL DROP TABLE dbo.Airline;
IF OBJECT_ID('dbo.Airport', 'U') IS NOT NULL DROP TABLE dbo.Airport;
IF OBJECT_ID('dbo.Address', 'U') IS NOT NULL DROP TABLE dbo.Address;

CREATE TABLE [Address]
(
    Id          INT IDENTITY(1,1),
    Country     NVARCHAR(80) NOT NULL,
    City        NVARCHAR(80) NOT NULL,
    Street      NVARCHAR(120) NOT NULL
);

CREATE TABLE [Airport]
(
    Id          INT IDENTITY(1,1),
    IATACode    NCHAR(3) NOT NULL,
    Name        NVARCHAR(120) NOT NULL,
    TimeZone    NVARCHAR(64) NOT NULL,
    AddressId   INT NOT NULL
);

CREATE TABLE [Airline]
(
    Id          INT IDENTITY(1,1),
    IATACode    NCHAR(2) NOT NULL,
    Name        NVARCHAR(100) NOT NULL
);

CREATE TABLE [Aircraft]
(
    Id              INT IDENTITY(1,1),
    TailNumber      NVARCHAR(10) NOT NULL,
    Model           NVARCHAR(60) NOT NULL,
    SeatCapacity    INT NOT NULL
);

CREATE TABLE [Gate]
(
    Id          INT IDENTITY(1,1),
    AirportId   INT NOT NULL,
    Code        NVARCHAR(10) NOT NULL
);

CREATE TABLE [Flight]
(
    Id                      INT IDENTITY(1,1),
    AirlineId               INT NOT NULL,
    FlightNumber            NVARCHAR(8) NOT NULL,
    OriginAirportId         INT NOT NULL,
    DestinationAirportId    INT NOT NULL,
    IsActive                BIT NOT NULL,
    DefaultAircraftId       INT NULL
);

CREATE TABLE [FlightStatus]
(
    Id      INT IDENTITY(1,1),
    Status  NVARCHAR(50) NOT NULL
);

CREATE TABLE [FlightSchedule]
(
    Id                      INT IDENTITY(1,1),
    FlightId                INT NOT NULL,
    ScheduledDepartureUtc   DATETIME2(0) NOT NULL,
    ScheduledArrivalUtc     DATETIME2(0) NOT NULL,
    GateId                  INT NULL,
    AssignedAircraftId      INT NULL,
    FlightStatusId          INT NOT NULL
);

CREATE TABLE [User]
(
    Id      INT IDENTITY(1,1),
    Name    NVARCHAR(100) NOT NULL,
    Email   NVARCHAR(120) NOT NULL
);

CREATE TABLE [BookingStatus]
(
    Id      INT IDENTITY(1,1),
    Status  NVARCHAR(50) NOT NULL
);

CREATE TABLE [Booking]
(
    Id                  BIGINT IDENTITY(1,1),
    UserId              INT NOT NULL,
    BookingStatusId     INT NOT NULL,
    CreatedUtc          DATETIME2(0) NOT NULL,
    ConfirmationCode    NVARCHAR(8) NOT NULL,
    Quantity            INT NOT NULL
);

CREATE TABLE [Ticket]
(
    Id                  BIGINT IDENTITY(1,1),
    BookingId           BIGINT NOT NULL,
    FlightScheduleId    INT NOT NULL,
    FareClass           NVARCHAR(2) NOT NULL,
    BasePrice           DECIMAL(10,2) NOT NULL,
    Taxes               DECIMAL(10,2) NOT NULL,
    TotalPrice          DECIMAL(10,2) NOT NULL,
    Currency            NCHAR(3) NOT NULL,
    IsRefundable        BIT NOT NULL,
    SeatInventory       INT NOT NULL,
    SeatNumber          NVARCHAR(10) NULL,
    PassengerFullName   NVARCHAR(120) NULL,
    PassengerEmail      NVARCHAR(120) NULL
);
GO
