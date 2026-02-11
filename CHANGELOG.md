# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-02-05

### Added

- Initial project setup with ASP.NET Core 8.0
- Database context and Entity Framework Core configuration
- Room entity with full CRUD operations
  - Create, Read, Update, Delete endpoints
  - Pagination and search functionality
  - Soft delete implementation
- Booking entity with full CRUD operations
  - Create, Read, Update, Delete endpoints
  - Booking conflict detection
  - Status management (Pending, Approved, Rejected, Cancelled)
  - Booking history with filters
- Database migrations and seeders
  - Initial database schema
  - Sample room data
  - Sample booking data
- API documentation with Swagger/OpenAPI
- CORS configuration for frontend integration
- Input validation with Data Annotations
- DTOs for request/response handling
- Environment configuration with .env support
- README.md with comprehensive documentation
- .gitignore for .NET projects

### Features

- Room Management
  - List rooms with pagination (default 10 per page)
  - Search rooms by room number or name
  - View room details
  - Create new rooms with validation
  - Update existing rooms
  - Soft delete rooms (preserves data)
- Booking Management
  - Create bookings with conflict detection
  - View booking details
  - Update booking information
  - Change booking status
  - Filter bookings by status, room, or search term
  - Soft delete bookings
  - Booking history tracking

### Technical Details

- Database: SQL Server with EF Core
- Validation: Data Annotations
- Architecture: Repository Pattern
- API Style: RESTful
- Documentation: Swagger UI

## [Unreleased]

### Planned

- Authentication and authorization
- Email notifications for booking status
- Calendar view integration
- Export booking reports
- User role management
- Booking analytics dashboard
