using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.DTOs;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public BookingsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Bookings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings(
			[FromQuery] string? search = null,
			[FromQuery] BookingStatus? status = null,
			[FromQuery] int? roomId = null,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			var query = _context.Bookings.Include(b => b.Room).AsQueryable();

			// Search filter
			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(b =>
					b.BookerName.Contains(search) ||
					b.BookerEmail.Contains(search) ||
					b.Purpose.Contains(search)
				);
			}

			// Status filter
			if (status.HasValue)
			{
				query = query.Where(b => b.Status == status.Value);
			}

			// Room filter
			if (roomId.HasValue)
			{
				query = query.Where(b => b.RoomId == roomId.Value);
			}

			var totalItems = await query.CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			var bookings = await query
				.OrderByDescending(b => b.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(b => new BookingDto
				{
					Id = b.Id,
					RoomId = b.RoomId,
					RoomNumber = b.Room.RoomNumber,
					RoomName = b.Room.Name,
					BookerName = b.BookerName,
					BookerEmail = b.BookerEmail,
					BookerPhone = b.BookerPhone,
					Purpose = b.Purpose,
					StartTime = b.StartTime,
					EndTime = b.EndTime,
					Status = b.Status,
					RejectionReason = b.RejectionReason,
					CreatedAt = b.CreatedAt
				})
				.ToListAsync();

			Response.Headers.Add("X-Total-Count", totalItems.ToString());
			Response.Headers.Add("X-Total-Pages", totalPages.ToString());
			Response.Headers.Add("X-Current-Page", page.ToString());

			return Ok(bookings);
		}

		// GET: api/Bookings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<BookingDto>> GetBooking(int id)
		{
			var booking = await _context.Bookings
				.Include(b => b.Room)
				.FirstOrDefaultAsync(b => b.Id == id);

			if (booking == null)
			{
				return NotFound(new { message = "Booking not found" });
			}

			var bookingDto = new BookingDto
			{
				Id = booking.Id,
				RoomId = booking.RoomId,
				RoomNumber = booking.Room.RoomNumber,
				RoomName = booking.Room.Name,
				BookerName = booking.BookerName,
				BookerEmail = booking.BookerEmail,
				BookerPhone = booking.BookerPhone,
				Purpose = booking.Purpose,
				StartTime = booking.StartTime,
				EndTime = booking.EndTime,
				Status = booking.Status,
				RejectionReason = booking.RejectionReason,
				CreatedAt = booking.CreatedAt
			};

			return Ok(bookingDto);
		}

		// POST: api/Bookings
		[HttpPost]
		public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto createBookingDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Validate dates
			if (createBookingDto.StartTime >= createBookingDto.EndTime)
			{
				return BadRequest(new { message = "End time must be after start time" });
			}

			if (createBookingDto.StartTime < DateTime.Now)
			{
				return BadRequest(new { message = "Cannot book for past dates" });
			}

			// Check if room exists
			var room = await _context.Rooms.FindAsync(createBookingDto.RoomId);
			if (room == null)
			{
				return NotFound(new { message = "Room not found" });
			}

			// Check for conflicting bookings
			var hasConflict = await _context.Bookings.AnyAsync(b =>
				b.RoomId == createBookingDto.RoomId &&
				b.Status != BookingStatus.Rejected &&
				b.Status != BookingStatus.Cancelled &&
				(
					(createBookingDto.StartTime >= b.StartTime && createBookingDto.StartTime < b.EndTime) ||
					(createBookingDto.EndTime > b.StartTime && createBookingDto.EndTime <= b.EndTime) ||
					(createBookingDto.StartTime <= b.StartTime && createBookingDto.EndTime >= b.EndTime)
				)
			);

			if (hasConflict)
			{
				return Conflict(new { message = "Room is already booked for the selected time slot" });
			}

			var booking = new Booking
			{
				RoomId = createBookingDto.RoomId,
				BookerName = createBookingDto.BookerName,
				BookerEmail = createBookingDto.BookerEmail,
				BookerPhone = createBookingDto.BookerPhone,
				Purpose = createBookingDto.Purpose,
				StartTime = createBookingDto.StartTime,
				EndTime = createBookingDto.EndTime,
				Status = BookingStatus.Pending,
				CreatedAt = DateTime.UtcNow
			};

			_context.Bookings.Add(booking);
			await _context.SaveChangesAsync();

			// Reload with room data
			booking = await _context.Bookings
				.Include(b => b.Room)
				.FirstAsync(b => b.Id == booking.Id);

			var bookingDto = new BookingDto
			{
				Id = booking.Id,
				RoomId = booking.RoomId,
				RoomNumber = booking.Room.RoomNumber,
				RoomName = booking.Room.Name,
				BookerName = booking.BookerName,
				BookerEmail = booking.BookerEmail,
				BookerPhone = booking.BookerPhone,
				Purpose = booking.Purpose,
				StartTime = booking.StartTime,
				EndTime = booking.EndTime,
				Status = booking.Status,
				CreatedAt = booking.CreatedAt
			};

			return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
		}

		// PUT: api/Bookings/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto updateBookingDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var booking = await _context.Bookings.FindAsync(id);

			if (booking == null)
			{
				return NotFound(new { message = "Booking not found" });
			}

			// Validate dates
			if (updateBookingDto.StartTime >= updateBookingDto.EndTime)
			{
				return BadRequest(new { message = "End time must be after start time" });
			}

			// Check if room exists
			var room = await _context.Rooms.FindAsync(updateBookingDto.RoomId);
			if (room == null)
			{
				return NotFound(new { message = "Room not found" });
			}

			// Check for conflicting bookings (excluding current booking)
			var hasConflict = await _context.Bookings.AnyAsync(b =>
				b.Id != id &&
				b.RoomId == updateBookingDto.RoomId &&
				b.Status != BookingStatus.Rejected &&
				b.Status != BookingStatus.Cancelled &&
				(
					(updateBookingDto.StartTime >= b.StartTime && updateBookingDto.StartTime < b.EndTime) ||
					(updateBookingDto.EndTime > b.StartTime && updateBookingDto.EndTime <= b.EndTime) ||
					(updateBookingDto.StartTime <= b.StartTime && updateBookingDto.EndTime >= b.EndTime)
				)
			);

			if (hasConflict)
			{
				return Conflict(new { message = "Room is already booked for the selected time slot" });
			}

			booking.RoomId = updateBookingDto.RoomId;
			booking.BookerName = updateBookingDto.BookerName;
			booking.BookerEmail = updateBookingDto.BookerEmail;
			booking.BookerPhone = updateBookingDto.BookerPhone;
			booking.Purpose = updateBookingDto.Purpose;
			booking.StartTime = updateBookingDto.StartTime;
			booking.EndTime = updateBookingDto.EndTime;
			booking.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// PATCH: api/Bookings/5/status
		[HttpPatch("{id}/status")]
		public async Task<IActionResult> UpdateBookingStatus(int id, UpdateBookingStatusDto statusDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var booking = await _context.Bookings.FindAsync(id);

			if (booking == null)
			{
				return NotFound(new { message = "Booking not found" });
			}

			booking.Status = statusDto.Status;
			booking.RejectionReason = statusDto.RejectionReason;
			booking.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Bookings/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			var booking = await _context.Bookings.FindAsync(id);

			if (booking == null)
			{
				return NotFound(new { message = "Booking not found" });
			}

			// Soft delete
			booking.IsDeleted = true;
			booking.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
