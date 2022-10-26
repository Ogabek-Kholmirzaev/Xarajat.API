using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xarajat.API.Data;
using Xarajat.API.Entities;
using Xarajat.API.Helpers;

namespace Xarajat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly XarajatDbContext _context;

        public RoomsController(XarajatDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRooms()
        {
            var rooms = _context.Rooms.Select(ConvertToGetRoomModel).ToList();

            if (rooms == null)
                return NotFound();

            return Ok(rooms);
        }

        [HttpPost]
        public IActionResult GetRooms(CreateRoomModel createRoomModel)
        {
            var room = new Room()
            {
                Name = createRoomModel.Name,
                Status = RoomStatus.Created,
                Key = RandomGenerator.GetRandomString(),
                AdminId = 2 // login user idsi
            };

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return Ok(ConvertToGetRoomModel(room));
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _context.Rooms.Include(r=>r.Admin).FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            return Ok(ConvertToGetRoomModel(room));
        }

        [HttpPut]
        public IActionResult UpdateRoomModel(int id, UpdateRoomModel updateRoomModel)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            room.Name = updateRoomModel.Name;
            room.Status = updateRoomModel.Status;

            _context.SaveChanges();

            return Ok(ConvertToGetRoomModel(room));
        }

        [HttpDelete]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return Ok();
        }

        private GetRoomModel ConvertToGetRoomModel(Room room)
        {
            return new GetRoomModel
            {
                Id = room.Id,
                Key = room.Key,
                Name = room.Name,
                Status = room.Status,
                Admin = ConvertToGetUser(room.Admin)
            };
        }

        private GetUser ConvertToGetUser(User user)
        {
            return new GetUser
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
