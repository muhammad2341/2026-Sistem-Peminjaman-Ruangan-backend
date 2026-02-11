# Room Booking System - Backend API

## Description

A RESTful API for managing campus room bookings built with ASP.NET Core. This system allows users to create, manage, and track room reservations with features like booking status management, conflict detection, and booking history.

## Features

- **Room Management**: Full CRUD operations for room entities
- **Booking System**: Create and manage room bookings with conflict detection
- **Status Management**: Approve, reject, or cancel bookings
- **Search & Filter**: Search rooms and bookings with pagination
- **Booking History**: Track all bookings with timestamps
- **Soft Delete**: Preserve data integrity with soft deletion
- **API Documentation**: Interactive Swagger UI

## Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server / PostgreSQL / MySQL
- **ORM**: Entity Framework Core
- **Documentation**: Swagger/OpenAPI
- **Architecture**: Repository Pattern, DTOs

## Prerequisites

- .NET SDK 8.0 or later
- SQL Server / PostgreSQL / MySQL
- Visual Studio 2022 or VS Code with C# extension

## Installation

### 1. Clone the repository

```bash
git clone https://github.com/<username>/2026-room-booking-backend.git
cd 2026-room-booking-backend/RoomBooking.Api
```

### 2. Install dependencies

```bash
dotnet restore
```

### 3. Configure database

Copy `.env.example` to `.env` and update the connection string:

```bash
cp .env.example .env
```

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RoomBookingDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

### 4. Apply migrations

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

The API will be available at `http://localhost:5000`

## Environment Variables

Required environment variables (see `.env.example`):

| Variable                   | Description                          | Example               |
| -------------------------- | ------------------------------------ | --------------------- |
| DATABASE_CONNECTION_STRING | Database connection string           | Server=localhost;...  |
| JWT_SECRET_KEY             | Secret key for JWT                   | your-secret-key       |
| ASPNETCORE_ENVIRONMENT     | Environment (Development/Production) | Development           |
| ASPNETCORE_URLS            | Application URLs                     | http://localhost:5000 |
| ALLOWED_ORIGINS            | CORS allowed origins                 | http://localhost:3000 |

## API Endpoints

### Rooms

- `GET /api/rooms` - Get all rooms (with pagination and search)
- `GET /api/rooms/{id}` - Get room by ID
- `POST /api/rooms` - Create new room
- `PUT /api/rooms/{id}` - Update room
- `DELETE /api/rooms/{id}` - Soft delete room

### Bookings

- `GET /api/bookings` - Get all bookings (with filters)
- `GET /api/bookings/{id}` - Get booking by ID
- `POST /api/bookings` - Create new booking
- `PUT /api/bookings/{id}` - Update booking
- `PATCH /api/bookings/{id}/status` - Update booking status
- `DELETE /api/bookings/{id}` - Soft delete booking

## Usage

### Access API Documentation

Navigate to `http://localhost:5000/swagger` to see interactive API documentation.

### Example API Calls

**Create a Room:**

```bash
curl -X POST http://localhost:5000/api/rooms \
	-H "Content-Type: application/json" \
	-d '{
		"roomNumber": "R101",
		"name": "Meeting Room A",
		"capacity": 10,
		"facilities": "Projector, Whiteboard",
		"isAvailable": true
	}'
```

**Create a Booking:**

```bash
curl -X POST http://localhost:5000/api/bookings \
	-H "Content-Type: application/json" \
	-d '{
		"roomId": 1,
		"bookerName": "John Doe",
		"bookerEmail": "john@example.com",
		"purpose": "Team Meeting",
		"startTime": "2026-02-10T09:00:00",
		"endTime": "2026-02-10T11:00:00"
	}'
```

## Project Structure

```
RoomBooking.Api/
├── Controllers/         # API controllers
├── Data/               # Database context and seeders
├── DTOs/               # Data Transfer Objects
├── Models/             # Entity models
├── Migrations/         # EF Core migrations
├── Program.cs          # Application entry point
└── appsettings.json    # Configuration
```

## Development

### Running Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName
```

### Running Tests

```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes using Conventional Commits
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Git Workflow

- `main` - Production-ready code
- `develop` - Development branch (default)
- `feature/*` - Feature branches
- `bugfix/*` - Bug fix branches

## Conventional Commits

We follow the Conventional Commits specification:

- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `chore:` - Maintenance tasks
- `refactor:` - Code refactoring

## License

MIT License

## Authors

- Your Name
- [GitHub Profile](https://github.com/yourusername)

## Acknowledgments

- PENS PBL 2026 Program
- PT. Sinergi Dimensi Informatika
