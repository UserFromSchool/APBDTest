
using APBDTest.Models.DTOs;

namespace APBDTest.Services;

public interface IAppointmentService
{

    public Task<AppointmentInfoReponseDTO> GetAppointmentInfo(int appointmentId);

    public Task<string> AddAppointment(AppointmentNewRequestDTO request);

}