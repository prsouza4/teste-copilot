# Users API

A simple .NET 8 REST API demonstrating CRUD operations for Users using Minimal API style.

## Features

- Full CRUD operations for User entity
- In-memory storage (no database required)
- Input validation
- Swagger/OpenAPI documentation
- Proper HTTP status codes

## User Entity

| Field | Type | Description |
|-------|------|-------------|
| Id | Guid | Unique identifier (auto-generated) |
| Name | string | User's name (required) |
| Email | string | User's email (required, validated) |
| CreatedAt | DateTime | Creation timestamp (auto-generated) |

## Endpoints

| Method | Endpoint | Description | Status Codes |
|--------|----------|-------------|--------------|
| GET | `/api/users` | List all users | 200 |
| GET | `/api/users/{id}` | Get user by ID | 200, 404 |
| POST | `/api/users` | Create a new user | 201, 400 |
| PUT | `/api/users/{id}` | Update a user | 200, 400, 404 |
| DELETE | `/api/users/{id}` | Delete a user | 204, 404 |

## Requirements

- .NET 8 SDK

## How to Run

```bash
# Navigate to the project folder
cd exemplos/Users.API

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The API will start at `http://localhost:5000` (or the port specified in your environment).

## Swagger UI

Access the Swagger documentation at:
- `http://localhost:5000/swagger`

## Example Requests

### Create a User

```bash
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "email": "john@example.com"}'
```

### Get All Users

```bash
curl http://localhost:5000/api/users
```

### Get User by ID

```bash
curl http://localhost:5000/api/users/{id}
```

### Update a User

```bash
curl -X PUT http://localhost:5000/api/users/{id} \
  -H "Content-Type: application/json" \
  -d '{"name": "John Updated", "email": "john.updated@example.com"}'
```

### Delete a User

```bash
curl -X DELETE http://localhost:5000/api/users/{id}
```

## Project Structure

```
Users.API/
├── Models/
│   └── User.cs          # User entity and DTOs
├── Program.cs           # Application entry point and endpoints
├── Users.API.csproj     # Project file
└── README.md            # This file
```
