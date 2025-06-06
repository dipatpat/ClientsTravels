# ClientsTravels API

ClientsTravels is an ASP.NET Core Web API project designed for managing travel agency operations. It allows for retrieving trips, assigning clients to trips, and deleting clients with proper validation.

---

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- Docker (optional)
- Swagger (OpenAPI)

---

## Features

- View paginated list of available trips with clients and countries.
- Assign a client to a trip, with validations:
  - Unique PESEL check
  - No duplicate registration
  - Trip must exist and be in the future
- Delete a client, only if they are not assigned to any trips.
- Follows Clean Architecture:
  - **Controller → Service → Repository → DBContext**

---

