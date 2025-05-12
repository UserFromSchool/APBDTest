using APBDTest.Repositories;
using APBDTest.Models.DTOs;

namespace APBDTest.Services;

public class AppointmentService(IAppointmentRepository appointmentRepository) : IAppointmentService
{

    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<AppointmentInfoReponseDTO> GetAppointmentInfo(int appointmentId)
    {

        var appointment = await _appointmentRepository.Find(appointmentId);
        if (appointment is null)
        {
            return new AppointmentInfoReponseDTO
            {
                Message = "No such appointment",
                Body = new {}
            };
        }
        
        var doctor = await _appointmentRepository.FindDoctor(appointment.DoctorId);
        var patient = await _appointmentRepository.FindPatient(appointment.PatientId);
        var services = await _appointmentRepository.FindServices(appointmentId);

        return new AppointmentInfoReponseDTO
        {
            Message = "Success",
            Body = new
            {
                Doctor = doctor,
                Patient = patient,
                Services = services,
            }
        };
    }

    public async Task<string> AddAppointment(AppointmentNewRequestDTO request)
    {
        return await _appointmentRepository.AddAppointment(request);
    }

}