using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareSystem.API.Data;
using PetCareSystem.API.Models;

namespace PetCareSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly PetCareDBContext _context;

        public InventoryController(PetCareDBContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
        {
            var inventoryItems = await _context.Inventory
                .Include(i => i.Product) // Bao gồm thông tin về sản phẩm liên quan đến mục tồn kho
                .ToListAsync();
            return inventoryItems;
        }

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventoryItem = await _context.Inventory //xài firstordefault để lấy một mục tồn kho cụ thể dựa trên BatchID
                .Include(i => i.Product) // Bao gồm thông tin về sản phẩm liên quan đến mục tồn kho
                .FirstOrDefaultAsync(i => i.BatchID == id);

            // Nếu không tìm thấy mục tồn kho với BatchID đã cho, trả về lỗi 404 Not Found
            if (inventoryItem == null)
            {
                return NotFound(new { message = $"Inventory item with ID {id} not found." });
            }

            return inventoryItem;
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(Inventory inventoryItem)
        {
            _context.Inventory.Add(inventoryItem); // Thêm vào bộ nhớ tạm
            await _context.SaveChangesAsync(); // Lưu chính thức vào database

            // Trả về thông tin của mục tồn kho vừa được tạo, cùng với mã trạng thái 201 Created
            return CreatedAtAction("GetInventory", new { id = inventoryItem.BatchID }, inventoryItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, Inventory inventoryItem)
        {
            if (id != inventoryItem.BatchID)
            {
                return BadRequest(new { message = "Batch ID not match!" });
            }

            _context.Entry(inventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Lưu các thay đổi vào database. Nếu có xung đột về dữ liệu (concurrency conflict), 
                // sẽ ném ra DbUpdateConcurrencyException
            }
            catch (DbUpdateConcurrencyException) // Bắt lỗi khi có xung đột về dữ liệu
            {
                if (!await _context.Inventory.AnyAsync(i => i.BatchID == id))
                {
                    return NotFound(new { message = $"Inventory item with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok (new { message = "Inventory item updated successfully." });
        }
    // Trừ kho tự động khi có cuộc hẹn được đặt và liên kết với mục tồn kho cụ thể, hoặc khi có mục tồn kho được cập nhật để phản ánh việc sử dụng sản phẩm trong quá trình điều trị thú cưng. Điều này giúp đảm bảo rằng số lượng tồn kho luôn được cập nhật chính xác và phản ánh đúng tình trạng hiện tại của kho hàng.
        [HttpPut("use-inventory/{batchId}")]
        public async Task<IActionResult> UseInventory(int batchId)
        {
            var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.BatchID == batchId);
        if (inventoryItem == null)
        {
            return NotFound(new { message = $"Inventory item with ID {batchId} not found." });
        }

        // Giả sử có một logic để trừ kho, ví dụ: inventoryItem.Quantity -= 1;
        // Ở đây, bạn cần thêm logic cụ thể để trừ kho dựa trên nhu cầu của ứng dụng

        _context.Entry(inventoryItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Inventory item updated successfully." });
    }
        [HttpPut("deduct-inventory/{batchId}")]
        public async Task<IActionResult> DeductInventory(int batchId, [FromBody]int quantityToDeduct)
        {
            var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.BatchID == batchId);
            if (inventoryItem == null)
            {
                return NotFound(new { message = $"Inventory item with ID {batchId} not found." });
            }

            if (inventoryItem.Quantity < quantityToDeduct) //  Kiểm tra nếu số lượng tồn kho hiện tại nhỏ hơn số lượng cần trừ
            {
                return BadRequest(new { message = "Not enough inventory to deduct the requested quantity." });
            }

            inventoryItem.Quantity -= quantityToDeduct; // Trừ số lượng tồn kho

            _context.Entry(inventoryItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Inventory item updated successfully.", remainingQuantity = inventoryItem.Quantity });
        }
    }
}