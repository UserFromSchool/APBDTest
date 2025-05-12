using APBDTest.Models.DTOs;
using APBDTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController(IAppointmentService service) : Controller
{

    private readonly IAppointmentService _service = service;

    [HttpGet("/appointments/{appointmentId}")]
    public async Task<IActionResult> GetAppointment(int appointmentId)
    {
        var info = await _service.GetAppointmentInfo(appointmentId);
        if (info.Message != "Success")
        {
            return BadRequest(info.Message);
        }
        return Ok(info.Body);
    }

    [HttpPost("/appointments")]
    public async Task<IActionResult> AddAppointment([FromBody] AppointmentNewRequestDTO request)
    {
        
    }
    
}