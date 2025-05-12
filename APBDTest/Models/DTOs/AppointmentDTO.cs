namespace APBDTest.Models.DTOs;

public class AppointmentDTO
{
    public required int Id { get; set; }
    public required int DoctorId { get; set; }
    public required int PatientId { get; set; }
    public required DateTime DateAt { get; set; }
}