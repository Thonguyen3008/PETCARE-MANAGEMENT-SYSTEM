using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareSystem.API.Data;
using PetCareSystem.API.Models;

namespace PetCareSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly PetCareDBContext _context;
        public CustomerController(PetCareDBContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customer/5
        // Lấy thông tin chi tiết của một khách hàng theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Pets) // Bao gồm thông tin về thú cưng của khách hàng
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound(new { message = $"Customer with ID {id} not found." });
            }

            return customer;
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomer(Customers customer)
        {
            _context.Customers.Add(customer); //them vao bo nho tam
            await _context.SaveChangesAsync(); // luu chinh thuc vao database

            // Trả về thông tin của khách hàng vừa được tạo, cùng với mã trạng thái 201 Created
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerID }, customer);
        }
    }
    
}