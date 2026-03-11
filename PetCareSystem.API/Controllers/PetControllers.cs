using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareSystem.API.Data;
using PetCareSystem.API.Models;

namespace PetCareSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetCareController : ControllerBase
    {
        private readonly PetCareDBContext _context;

        public PetCareController(PetCareDBContext context)
        {
            _context = context;
        }

        // GET: api/PetCare
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pets>>> GetPets()
        {
            return await _context.Pets.ToListAsync();
        }

        // GET: api/PetCare/5
        [HttpGet("{id}", Name = "GetPet")]
        public async Task<ActionResult<Pets>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound(new { message = $"Pet with ID {id} not found." });
            }

            return pet;
        }
        [HttpPost]
        public async Task<ActionResult<Pets>> PostPet(Pets pet)
        {
            _context.Pets.Add(pet); // Thêm vào bộ nhớ tạm
            await _context.SaveChangesAsync(); // Lưu chính thức vào database

            // Trả về thông tin của thú cưng vừa được tạo, cùng với mã trạng thái 201 Created
            return CreatedAtRoute("GetPet", new { id = pet.PetID}, pet);
        }
        
    }
}