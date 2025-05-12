namespace APBDTest.Models.DTOs;

public class AppointmentNewRequestDTO
{
    public required int AppointmentId { get; set; }
    public required int PatientId { get; set; }
    public required string Pwz { get; set; }
    public required List<ServiceDTO> Services { get; set; }
}