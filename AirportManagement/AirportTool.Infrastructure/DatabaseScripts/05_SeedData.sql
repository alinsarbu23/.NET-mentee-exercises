USE AirportManagement;

INSERT INTO BookingStatus(Status)
VALUES ('Active'), ('Cancelled');

INSERT INTO FlightStatus(Status)
VALUES ('Planned'), ('Boarding'), ('Departed'), ('Cancelled'), ('Delayed');

INSERT INTO [Address](Country, City, Street)
VALUES
('Romania', 'Bucharest', 'Otopeni Airport Street'),
('United Kingdom', 'London', 'Heathrow Airport Street');

INSERT INTO [Airport](IATACode, Name, TimeZone, AddressId)
VALUES
('OTP', 'Henri Coanda Intl Airport', 'Europe/Bucharest', 1),
('LHR', 'London Heathrow Airport', 'Europe/London', 2);

INSERT INTO [Airline](IATACode, Name)
VALUES
('RO', 'TAROM'),
('BA', 'British Airways');

INSERT INTO [Aircraft](TailNumber, Model, SeatCapacity)
VALUES
('YR-BGA', 'Boeing 737', 180),
('G-BAAA', 'Airbus A320', 160);

INSERT INTO [Gate](AirportId, Code)
VALUES
(1, 'A1'),
(1, 'A2'),
(2, 'B1'),
(2, 'B2');

INSERT INTO [User](Name, Email)
VALUES
('Jane Doe', 'jane@example.com'),
('John Smith', 'john@example.com');

INSERT INTO [Flight](AirlineId, FlightNumber, OriginAirportId, DestinationAirportId, IsActive, DefaultAircraftId)
VALUES
(1, 'RO391', 1, 2, 1, 1),
(2, 'BA888', 2, 1, 1, 2);

INSERT INTO [FlightSchedule](FlightId, ScheduledDepartureUtc, ScheduledArrivalUtc, GateId, AssignedAircraftId, FlightStatusId)
VALUES
(1, '2025-12-20T06:30:00', '2025-12-20T08:25:00', 1, 1, 1),
(2, '2025-12-20T10:00:00', '2025-12-20T11:45:00', 3, 2, 1);

INSERT INTO [Booking](UserId, BookingStatusId, ConfirmationCode, Quantity)
VALUES
(1, 1, 'AB12CD', 2),
(2, 1, 'EF34GH', 1);

INSERT INTO [Ticket](
    BookingId, FlightScheduleId, FareClass,
    BasePrice, Taxes, TotalPrice,
    Currency, IsRefundable, SeatInventory,
    SeatNumber, PassengerFullName, PassengerEmail)
VALUES
(1, 1, 'Y', 100.00, 20.00, 120.00, 'EUR', 1, 178, '12A', 'Jane Doe', 'jane@example.com'),
(1, 1, 'Y', 100.00, 20.00, 120.00, 'EUR', 1, 177, '12B', 'John Doe', 'john.doe@example.com'),
(2, 2, 'Y', 90.00, 18.00, 108.00, 'EUR', 0, 159, '10C', 'John Smith', 'john@example.com');
GO
