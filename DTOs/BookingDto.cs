using System.ComponentModel.DataAnnotations;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.DTOs
{
	public class BookingDto
	{
		public int Id { get; set; }
		public int RoomId { get; set; }
		public string RoomNumber { get; set; } = string.Empty;
		public string RoomName { get; set; } = string.Empty;
		public string BookerName { get; set; } = string.Empty;
		public string BookerEmail { get; set; } = string.Empty;
		public string? BookerPhone { get; set; }
		public string Purpose { get; set; } = string.Empty;
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public BookingStatus Status { get; set; }
		public string? RejectionReason { get; set; }
		public DateTime CreatedAt { get; set; }
	}

	public class CreateBookingDto
	{
		[Required(ErrorMessage = "Room ID is required")]
		public int RoomId { get; set; }

		[Required(ErrorMessage = "Booker name is required")]
		[MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
		public string BookerName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
		public string BookerEmail { get; set; } = string.Empty;

		[Phone(ErrorMessage = "Invalid phone number format")]
		public string? BookerPhone { get; set; }

		[Required(ErrorMessage = "Purpose is required")]
		[MaxLength(500, ErrorMessage = "Purpose cannot exceed 500 characters")]
		public string Purpose { get; set; } = string.Empty;

		[Required(ErrorMessage = "Start time is required")]
		public DateTime StartTime { get; set; }

		[Required(ErrorMessage = "End time is required")]
		public DateTime EndTime { get; set; }
	}

	public class UpdateBookingDto
	{
		[Required(ErrorMessage = "Room ID is required")]
		public int RoomId { get; set; }

		[Required(ErrorMessage = "Booker name is required")]
		[MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
		public string BookerName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
		public string BookerEmail { get; set; } = string.Empty;

		[Phone(ErrorMessage = "Invalid phone number format")]
		public string? BookerPhone { get; set; }

		[Required(ErrorMessage = "Purpose is required")]
		[MaxLength(500, ErrorMessage = "Purpose cannot exceed 500 characters")]
		public string Purpose { get; set; } = string.Empty;

		[Required(ErrorMessage = "Start time is required")]
		public DateTime StartTime { get; set; }

		[Required(ErrorMessage = "End time is required")]
		public DateTime EndTime { get; set; }
	}

	public class UpdateBookingStatusDto
	{
		[Required(ErrorMessage = "Status is required")]
		public BookingStatus Status { get; set; }

		public string? RejectionReason { get; set; }
	}
}
