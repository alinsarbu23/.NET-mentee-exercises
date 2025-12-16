﻿# Airport Management Tool — .NET 8 Web API

## Description

A beginner-friendly ASP.NET Core 8 Web API for airport operations.
The system allows airport staff to manage flights, schedules, aircraft, gates, and tickets, while clients can search flights, check availability, and create bookings.
The API uses SQL Server + EF Core 8, Repository + Unit of Work pattern, DTOs and validations.
The project follows Clean Architecture principles, separating Domain, Application, Infrastructure and WebApi layers.

---

## Requirements

* Staff use-cases:

```
    - Manage flights (CRUD)
    - Manage flight schedules (CRUD)
    - Import schedules from JSON (bulk upsert, per-row validation)
    - Manage gates (CRUD)
    - Manage aircraft (CRUD)
    - Manage tickets for each flight schedule (CRUD)
    - View statistics: number of flights per day in next 7 days
```

* Client use-cases:
```
    - Search flights by origin + destination + date
    - Get upcoming flights
    - View ticket availability and prices
    - Create a booking (with confirmation code)
    - Retrieve booking by code
    - Cancel booking
```

---

## Bussiness Rules

```
    - Flight origin != destination.
    - FlightSchedule ArrivalUtc > DepartureUtc.
    - No gate overlaps for the same gate and time interval.
    - Prevent overbooking: Quantity <= Ticket.SeatInventory.
    - On booking create → decrement SeatInventory.
    - On booking cancel → restore SeatInventory.
    - Ticket prices must be positive, taxes non-negative.
    - ConfirmationCode must be unique.
```

## Architecture

```
AirportManagement/
  ├── AirportTool.Domain/         # Entities and resources
  ├── AirportTool.Application/    # Mappers, services and interfaces
  ├── AirportTool.Infrastructure/ # EF Core DbContext, repositories, UoW, Data and SQL scripts
  └── AirportTool.WebApi/         # Controllers, startup and DI
```

## Dependencies

```
WebApi → Application → Domain
Infrastructure → Application + Domain
```
---

## Layers Overview

|-----------------------------------------------------------------------------------------------|
| Layer                   | Responsibility                                                      |
| ----------------------------------------------------------------------------------------------|
| **Domain**              | Pure business entities and enums, no EF Core.                       |
| **Application**         | Business logic: services, validators, use-cases, DTO mapping.       |
| **Infrastructure**      | EF Core DbContext, repositories, migrations, SQL scripts.           |
| **WebApi**              | REST endpoints, dependency injection, exception handling, swagger.  |
| ----------------------------------------------------------------------------------------------|      