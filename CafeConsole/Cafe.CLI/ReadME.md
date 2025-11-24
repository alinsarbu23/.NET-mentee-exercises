﻿# ReadingList — .NET 8 Console App

## Description

Build a small console ordering app for a café. Users pick a base drink (espresso, tea, hot chocolate), add optional extras (milk, syrup, extra shot), and then the app calculates the final price using a configurable pricing policy (e.g., Regular vs HappyHour). The app prints a receipt and publishes simple “events” (e.g., “OrderPlaced”) for logging or analytics.
This brief emphasizes SOLID, loose coupling, and practical use of Strategy, Factory, Decorator, and Observer patterns.

---

## Requirements

* Funcitonal flow

```
   1. Choose base beverage:
         1) Espresso ($2.50)
         2) Tea ($2.00)
         3) Hot Chocolate ($3.00)
    2. Add-ons (0..N):
         1) Milk (+$0.40)
         2) Syrup (+$0.50, ask for flavor)
         3) Extra shot (+$0.80)
         0) Done
    3. Choose pricing policy:
         1) Regular
         2) HappyHour (-20%)
    4. Show receipt: itemized description, subtotal, discount (if any), total      
    5. Publish OrderPlaced event:
         → ConsoleOrderLogger
         → InMemoryOrderAnalytics
    6. Ask to place another order or exit
```

* Non-Functional:
```
    1. Follow SOLID principles (especially SRP, OCP, DIP).
    2. Keep domain logic pure, no Console or I/O in domain layer.
    3. Use manual dependency wiring in Program.cs (no DI container).
    4. Handle invalid input gracefully — never crash.
    5. Allow currency configuration (default $).
    6. Cover core logic with xUnit + Moq unit tests.

```

---

## Architecture

```
CafeConsole/
  ├── Cafe.Domain/          # Core domain: beverages, pricing, events, factories
  ├── Cafe.Application/     # Event publisher, orchestration services
  ├── Cafe.Infrastructure/  # Factories and observers (console, analytics)
  ├── Cafe.ConsoleUI/       # Console app (Program.cs, menu, composition root)
  └── Cafe.Tests/           # Unit tests (xUnit + Moq)
```

## Dependencies

```
ConsoleUI → Application → Domain
Infrastructure ↔ Application
Tests → Domain (+ Infrastructure)
```
---

## Layers Overview

|-----------------------------------------------------------------------------------------------|
| Layer                   | Responsibility                                                      |
| --------------------------------------------------------------------------------------------- |
| **ConsoleUI**           | Handles menu flow, reads input/output, wires dependencies manually. |
| **Application**         | Publishes events and orchestrates business operations.              |
| **Domain**              | Pure business logic: models, interfaces, and design patterns.       |
| **Infrastructure**      | Concrete implementations (factory, observers).                      |
| **Tests**               | Unit tests for domain and infrastructure logic.                     |
| ----------------------------------------------------------------------------------------------|                                                                     |