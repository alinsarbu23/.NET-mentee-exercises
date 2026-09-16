USE AirportManagement;
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Ticket_Booking')
BEGIN
    ALTER TABLE dbo.Ticket DROP CONSTRAINT FK_Ticket_Booking;
END
GO

ALTER TABLE dbo.Ticket ALTER COLUMN BookingId BIGINT NULL;
GO

ALTER TABLE dbo.Ticket
ADD CONSTRAINT FK_Ticket_Booking
FOREIGN KEY (BookingId) REFERENCES dbo.Booking(Id);
GO

IF COL_LENGTH('dbo.Booking', 'FlightScheduleId') IS NULL
    ALTER TABLE dbo.Booking ADD FlightScheduleId INT NULL;

IF COL_LENGTH('dbo.Booking', 'TicketId') IS NULL
    ALTER TABLE dbo.Booking ADD TicketId BIGINT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Booking_FlightSchedule')
BEGIN
    ALTER TABLE dbo.Booking
    ADD CONSTRAINT FK_Booking_FlightSchedule
    FOREIGN KEY (FlightScheduleId) REFERENCES dbo.FlightSchedule(Id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Booking_Ticket')
BEGIN
    ALTER TABLE dbo.Booking
    ADD CONSTRAINT FK_Booking_Ticket
    FOREIGN KEY (TicketId) REFERENCES dbo.Ticket(Id);
END
GO
