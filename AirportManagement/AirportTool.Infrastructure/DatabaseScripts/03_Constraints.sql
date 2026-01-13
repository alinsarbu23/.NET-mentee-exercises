USE AirportManagement;

ALTER TABLE [Address]
ADD CONSTRAINT PK_Address PRIMARY KEY (Id);

ALTER TABLE [Airport]
ADD CONSTRAINT PK_Airport PRIMARY KEY (Id),
    CONSTRAINT UQ_Airport_IATACode UNIQUE (IATACode),
    CONSTRAINT FK_Airport_Address FOREIGN KEY (AddressId) REFERENCES [Address](Id);

ALTER TABLE [Airline]
ADD CONSTRAINT PK_Airline PRIMARY KEY (Id),
    CONSTRAINT UQ_Airline_IATACode UNIQUE (IATACode);

ALTER TABLE [Aircraft]
ADD CONSTRAINT PK_Aircraft PRIMARY KEY (Id),
    CONSTRAINT UQ_Aircraft_TailNumber UNIQUE (TailNumber),
    CONSTRAINT CK_Aircraft_SeatCapacity CHECK (SeatCapacity > 0);

ALTER TABLE [Gate]
ADD CONSTRAINT PK_Gate PRIMARY KEY (Id),
    CONSTRAINT FK_Gate_Airport FOREIGN KEY (AirportId) REFERENCES [Airport](Id),
    CONSTRAINT UQ_Gate_Airport_Code UNIQUE (AirportId, Code);

ALTER TABLE [Flight]
ADD CONSTRAINT PK_Flight PRIMARY KEY (Id),
    CONSTRAINT FK_Flight_Airline FOREIGN KEY (AirlineId) REFERENCES [Airline](Id),
    CONSTRAINT FK_Flight_OriginAirport FOREIGN KEY (OriginAirportId) REFERENCES [Airport](Id),
    CONSTRAINT FK_Flight_DestinationAirport FOREIGN KEY (DestinationAirportId) REFERENCES [Airport](Id),
    CONSTRAINT FK_Flight_DefaultAircraft FOREIGN KEY (DefaultAircraftId) REFERENCES [Aircraft](Id),
    CONSTRAINT CK_Flight_OriginNotDestination CHECK (OriginAirportId <> DestinationAirportId),
    CONSTRAINT DF_Flight_IsActive DEFAULT (1) FOR IsActive;

ALTER TABLE [FlightStatus]
ADD CONSTRAINT PK_FlightStatus PRIMARY KEY (Id);

ALTER TABLE [FlightSchedule]
ADD CONSTRAINT PK_FlightSchedule PRIMARY KEY (Id),
    CONSTRAINT FK_FlightSchedule_Flight FOREIGN KEY (FlightId) REFERENCES [Flight](Id),
    CONSTRAINT FK_FlightSchedule_Gate FOREIGN KEY (GateId) REFERENCES [Gate](Id),
    CONSTRAINT FK_FlightSchedule_AssignedAircraft FOREIGN KEY (AssignedAircraftId) REFERENCES [Aircraft](Id),
    CONSTRAINT FK_FlightSchedule_FlightStatus FOREIGN KEY (FlightStatusId) REFERENCES [FlightStatus](Id),
    CONSTRAINT CK_FlightSchedule_ArrivalAfterDeparture CHECK (ScheduledArrivalUtc > ScheduledDepartureUtc);

ALTER TABLE [User]
ADD CONSTRAINT PK_User PRIMARY KEY (Id),
    CONSTRAINT UQ_User_Email UNIQUE (Email);

ALTER TABLE [BookingStatus]
ADD CONSTRAINT PK_BookingStatus PRIMARY KEY (Id);

ALTER TABLE [Booking]
ADD CONSTRAINT PK_Booking PRIMARY KEY (Id),
    CONSTRAINT FK_Booking_User FOREIGN KEY (UserId) REFERENCES [User](Id),
    CONSTRAINT FK_Booking_Status FOREIGN KEY (BookingStatusId) REFERENCES [BookingStatus](Id),
    CONSTRAINT UQ_Booking_ConfirmationCode UNIQUE (ConfirmationCode),
    CONSTRAINT CK_Booking_Quantity CHECK (Quantity > 0),
    CONSTRAINT DF_Booking_CreatedUtc DEFAULT (SYSUTCDATETIME()) FOR CreatedUtc;

ALTER TABLE [Ticket]
ADD CONSTRAINT PK_Ticket PRIMARY KEY (Id),
    CONSTRAINT FK_Ticket_Booking FOREIGN KEY (BookingId) REFERENCES [Booking](Id),
    CONSTRAINT FK_Ticket_FlightSchedule FOREIGN KEY (FlightScheduleId) REFERENCES [FlightSchedule](Id),
    CONSTRAINT CK_Ticket_BasePrice CHECK (BasePrice >= 0),
    CONSTRAINT CK_Ticket_Taxes CHECK (Taxes >= 0),
    CONSTRAINT CK_Ticket_SeatInventory CHECK (SeatInventory >= 0),
    CONSTRAINT CK_Ticket_TotalPrice CHECK (TotalPrice = BasePrice + Taxes),
    CONSTRAINT DF_Ticket_IsRefundable DEFAULT (0) FOR IsRefundable;
GO
