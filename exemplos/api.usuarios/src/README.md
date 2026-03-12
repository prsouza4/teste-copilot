# Usuario Microservice

## Overview

This microservice handles user management, including CRUD operations for user data.

### Features

- User registration with validation for CPF and email
- Full CRUD operations
- CQRS pattern
- Result pattern for consistent API responses
- Global exception handling
- SQL Server database
- RabbitMQ integration
- Logging with Serilog

### Requirements

- .NET 10
- SQL Server
- RabbitMQ
- Aspire for local development

### Getting Started

1. Clone the repository.
2. Configure the database connection string in `appsettings.json`.
3. Run the migrations: `dotnet ef database update`.
4. Start the application: `dotnet run`.
5. Use Swagger to test the API endpoints.

### Endpoints

- `POST /api/users`: Create a new user.
- `GET /api/users/{id}`: Get user by ID.
- `PUT /api/users/{id}`: Update user.
- `DELETE /api/users/{id}`: Delete user.
