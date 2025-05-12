namespace APBDTest.Models.DTOs;

public class ServiceDTO
{
    public required string Name { get; set; }
    public required decimal ServiceFee { get; set; } // From link between tables, no base fee
}