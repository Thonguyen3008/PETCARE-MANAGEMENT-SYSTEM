using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareSystem.API.Data;
using PetCareSystem.API.Models;

namespace PetCareSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly PetCareDBContext _context;
        public AppointmentsController(PetCareDBContext context)
        {
            _context = context;
        }

        // API lấy danh sách tất cả các cuộc hẹn, bao gồm thông tin về thú cưng và nhân viên chăm sóc liên quan đến mỗi cuộc hẹn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Pet) // Bao gồm thông tin về thú cưng
                .ThenInclude(p => p.Owner) // Bao gồm thông tin về chủ nuôi của thú cưng
                .Include(a => a.Staff) // Bao gồm thông tin về nhân viên chăm sóc
                .ToListAsync();
            return appointments;
        }

        //API tạo mới cuộc hẹn và có chức năng kiểm tra xem có trùng lịch hẹn với cùng một bác sĩ thú y hay không
        [HttpPost]
        public async Task<ActionResult<Appointments>> CreateAppointment(Appointments appointment)
        {
            bool isSlotTaken = await _context.Appointments.AnyAsync(a =>
                a.StaffID == appointment.StaffID &&
                a.AppointmentDate == appointment.AppointmentDate &&
                a.Status != "Canceled"); // hủy bỏ cuộc hẹn đã đặt trước đó sẽ không bị coi là trùng lịch

            if (isSlotTaken)
            {
                return BadRequest("The selected time slot is already booked.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentID }, appointment);
        }
    }
}