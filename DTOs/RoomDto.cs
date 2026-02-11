using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Api.DTOs
{
	public class RoomDto
	{
		public int Id { get; set; }
		public string RoomNumber { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public int Capacity { get; set; }
		public string? Facilities { get; set; }
		public bool IsAvailable { get; set; }
		public DateTime CreatedAt { get; set; }
	}

	public class CreateRoomDto
	{
		[Required(ErrorMessage = "Room number is required")]
		[MaxLength(20, ErrorMessage = "Room number cannot exceed 20 characters")]
		public string RoomNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Room name is required")]
		[MaxLength(100, ErrorMessage = "Room name cannot exceed 100 characters")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Capacity is required")]
		[Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000")]
		public int Capacity { get; set; }

		public string? Facilities { get; set; }
		public bool IsAvailable { get; set; } = true;
	}

	public class UpdateRoomDto
	{
		[Required(ErrorMessage = "Room number is required")]
		[MaxLength(20, ErrorMessage = "Room number cannot exceed 20 characters")]
		public string RoomNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Room name is required")]
		[MaxLength(100, ErrorMessage = "Room name cannot exceed 100 characters")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Capacity is required")]
		[Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000")]
		public int Capacity { get; set; }

		public string? Facilities { get; set; }
		public bool IsAvailable { get; set; }
	}
}
