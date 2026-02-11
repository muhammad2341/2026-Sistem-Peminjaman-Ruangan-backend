using System;
using System.Collections.Generic;
using System.Linq;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Data.Seeders
{
    public static class DatabaseSeeder
    {
        public static void SeedRooms(ApplicationDbContext context)
        {
            if (context.Rooms.Any())
                return;

            var rooms = new List<Room>
            {
                new Room
                {
                    RoomNumber = "R101",
                    Name = "Meeting Room A",
                    Capacity = 10,
                    Facilities = "Projector, Whiteboard, AC",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "R102",
                    Name = "Meeting Room B",
                    Capacity = 20,
                    Facilities = "Projector, Whiteboard, AC, Video Conference",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "R201",
                    Name = "Lecture Hall 1",
                    Capacity = 50,
                    Facilities = "Projector, Sound System, AC",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "R202",
                    Name = "Lecture Hall 2",
                    Capacity = 100,
                    Facilities = "Projector, Sound System, AC, Recording Equipment",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "LAB1",
                    Name = "Computer Lab 1",
                    Capacity = 30,
                    Facilities = "30 Computers, Projector, AC",
                    IsAvailable = true
                }
            };

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        public static void SeedBookings(ApplicationDbContext context)
        {
            if (context.Bookings.Any())
                return;

            var rooms = context.Rooms.ToList();
            if (!rooms.Any())
                return;

            var bookings = new List<Booking>
            {
                new Booking
                {
                    RoomId = rooms[0].Id,
                    BookerName = "John Doe",
                    BookerEmail = "john.doe@example.com",
                    BookerPhone = "081234567890",
                    Purpose = "Team Meeting",
                    StartTime = DateTime.Now.AddDays(1).AddHours(9),
                    EndTime = DateTime.Now.AddDays(1).AddHours(11),
                    Status = BookingStatus.Approved
                },
                new Booking
                {
                    RoomId = rooms[1].Id,
                    BookerName = "Jane Smith",
                    BookerEmail = "jane.smith@example.com",
                    Purpose = "Workshop on Web Development",
                    StartTime = DateTime.Now.AddDays(2).AddHours(13),
                    EndTime = DateTime.Now.AddDays(2).AddHours(17),
                    Status = BookingStatus.Pending
                },
                new Booking
                {
                    RoomId = rooms[2].Id,
                    BookerName = "Bob Wilson",
                    BookerEmail = "bob.wilson@example.com",
                    BookerPhone = "081987654321",
                    Purpose = "Guest Lecture",
                    StartTime = DateTime.Now.AddDays(-1).AddHours(10),
                    EndTime = DateTime.Now.AddDays(-1).AddHours(12),
                    Status = BookingStatus.Approved
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}
