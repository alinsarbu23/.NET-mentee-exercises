USE AirportManagement;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Airport_IATACode'
      AND object_id = OBJECT_ID('dbo.Airport')
)
CREATE UNIQUE NONCLUSTERED INDEX IX_Airport_IATACode
    ON [Airport](IATACode);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Airline_IATACode'
      AND object_id = OBJECT_ID('dbo.Airline')
)
CREATE UNIQUE NONCLUSTERED INDEX IX_Airline_IATACode
    ON [Airline](IATACode);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Gate_Airport_Code'
      AND object_id = OBJECT_ID('dbo.Gate')
)
CREATE UNIQUE NONCLUSTERED INDEX IX_Gate_Airport_Code
    ON [Gate](AirportId, Code);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Flight_Airline_FlightNumber'
      AND object_id = OBJECT_ID('dbo.Flight')
)
CREATE NONCLUSTERED INDEX IX_Flight_Airline_FlightNumber
    ON [Flight](AirlineId, FlightNumber);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Flight_Origin_Destination'
      AND object_id = OBJECT_ID('dbo.Flight')
)
CREATE NONCLUSTERED INDEX IX_Flight_Origin_Destination
    ON [Flight](OriginAirportId, DestinationAirportId);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_FlightSchedule_Flight_Departure'
      AND object_id = OBJECT_ID('dbo.FlightSchedule')
)
CREATE NONCLUSTERED INDEX IX_FlightSchedule_Flight_Departure
    ON [FlightSchedule](FlightId, ScheduledDepartureUtc);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Ticket_FlightSchedule_FareClass'
      AND object_id = OBJECT_ID('dbo.Ticket')
)
CREATE NONCLUSTERED INDEX IX_Ticket_FlightSchedule_FareClass
    ON [Ticket](FlightScheduleId, FareClass);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Booking_ConfirmationCode'
      AND object_id = OBJECT_ID('dbo.Booking')
)
CREATE UNIQUE NONCLUSTERED INDEX IX_Booking_ConfirmationCode
    ON [Booking](ConfirmationCode);
GO
