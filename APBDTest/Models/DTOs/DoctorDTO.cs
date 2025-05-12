using System.ComponentModel.DataAnnotations;

namespace APBDTest.Models.DTOs;

public class DoctorDTO
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    [StringLength(7)]
    public required string Pwz { get; set; }
}