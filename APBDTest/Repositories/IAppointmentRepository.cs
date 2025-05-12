using APBDTest.Models.DTOs;

namespace APBDTest.Repositories;

public interface IAppointmentRepository
{
    
    // No time on the test so had to go with single repostiory. Normally separate repository for each.

    public Task<AppointmentDTO?> Find(int id);

    public Task<DoctorDTO?> FindDoctor(int id);

    public Task<PatientDTO?> FindPatient(int id);

    public Task<List<ServiceDTO>> FindServices(int appointmentId);

    public Task<string> AddAppointment(AppointmentNewRequestDTO request);

}