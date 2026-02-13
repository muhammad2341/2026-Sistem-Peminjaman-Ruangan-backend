using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.DTOs;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        [Authorize(Policy = "BorrowerOrAdmin")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms(
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Rooms.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.RoomNumber.Contains(search) ||
                    r.Name.Contains(search)
                );
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var rooms = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Facilities = r.Facilities,
                    IsAvailable = r.IsAvailable,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalItems.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            Response.Headers.Add("X-Current-Page", page.ToString());

            return Ok(rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        [Authorize(Policy = "BorrowerOrAdmin")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Name = room.Name,
                Capacity = room.Capacity,
                Facilities = room.Facilities,
                IsAvailable = room.IsAvailable,
                CreatedAt = room.CreatedAt
            };

            return Ok(roomDto);
        }

        // POST: api/Rooms
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto createRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if room number already exists
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNumber == createRoomDto.RoomNumber);

            if (existingRoom != null)
            {
                return Conflict(new { message = "Room number already exists" });
            }

            var room = new Room
            {
                RoomNumber = createRoomDto.RoomNumber,
                Name = createRoomDto.Name,
                Capacity = createRoomDto.Capacity,
                Facilities = createRoomDto.Facilities,
                IsAvailable = createRoomDto.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var roomDto = new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Name = room.Name,
                Capacity = room.Capacity,
                Facilities = room.Facilities,
                IsAvailable = room.IsAvailable,
                CreatedAt = room.CreatedAt
            };

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, roomDto);
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto updateRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            // Check if new room number already exists (excluding current room)
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNumber == updateRoomDto.RoomNumber && r.Id != id);

            if (existingRoom != null)
            {
                return Conflict(new { message = "Room number already exists" });
            }

            room.RoomNumber = updateRoomDto.RoomNumber;
            room.Name = updateRoomDto.Name;
            room.Capacity = updateRoomDto.Capacity;
            room.Facilities = updateRoomDto.Facilities;
            room.IsAvailable = updateRoomDto.IsAvailable;
            room.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            // Soft delete
            room.IsDeleted = true;
            room.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
