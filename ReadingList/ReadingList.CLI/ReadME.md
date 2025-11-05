# ReadingList — .NET 8 Console App

A simple console app that manages a reading list from CSV files and shows basic stats. Built using Clean Architecture.

---

## Requirements

* Import one or more CSV files (UTF-8):

  ```csv
  Id,Title,Author,Year,Pages,Genre,Finished(yes/no),Rating(0-5)
  ```
* Commands:

  ```
  import <file1> [file2...]
  list all
  filter finished
  by author <text>
  top rated <n>
  mark finished <id>
  rate <id> <0-5>
  stats
  export json|csv <path>
  help
  exit
  ```

---

## Architecture

```
ReadingList/
  ├── ReadingList.Cli/            # Console UI (Program, BooksMenu)
  ├── ReadingList.Application/    # Use cases & interfaces
  ├── ReadingList.Domain/         # Entities, Result<T>, extensions
  └── ReadingList.Infrastructure/ # Repo + Import/Export implementations
```

**Dependencies:**

```
CLI → Application → Domain
Infrastructure -> Application
```
---

## Layers Overview

|-------------------------------------------------------------------------------------|
| Layer              | Responsibility                                                 |
| ------------------ | -------------------------------------------------------------- |
| **CLI**            | Reads user commands, calls `ReadingList.Application.Service`   |
| **Application**    | Core logic (import, list, stats, update)                       |
| **Domain**         | Models and generics (`Book`, `Result<T>`)                      |
| **Infrastructure** | File I/O, repositories, CSV handling                           |
|-------------------------------------------------------------------------------------|