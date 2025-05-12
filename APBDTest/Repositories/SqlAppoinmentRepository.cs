using APBDTest.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBDTest.Repositories;

public class SqlAppoinmentRepository(IConfiguration configuration) : IAppointmentRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<AppointmentDTO?> Find(int id)
    {
        AppointmentDTO? appointment = null;
        using (var connection = new SqlConnection(_connectionString))
        using (var command =
               new SqlCommand("SELECT * FROM Appointment WHERE appointment_id = @AppointmentId", connection))
        {
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@AppointmentId", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    appointment = new AppointmentDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("appointment_id")),
                        DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                        PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                        DateAt = reader.GetDateTime(reader.GetOrdinal("date"))
                    };
                }
            }
            
        }
        return appointment;
    }

    public async Task<DoctorDTO?> FindDoctor(int id)
    {
        DoctorDTO? doctor = null;
        using (var connection = new SqlConnection(_connectionString))
        using (var command =
               new SqlCommand("SELECT * FROM Doctor WHERE doctor_id = @DoctorId", connection))
        {
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@DoctorId", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    doctor = new DoctorDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        Pwz = reader.GetString(reader.GetOrdinal("pwz"))
                    };
                }
            }
            
        }
        return doctor;
    }

    public async Task<PatientDTO?> FindPatient(int id)
    {
        PatientDTO? patient = null;
        using (var connection = new SqlConnection(_connectionString))
        using (var command =
               new SqlCommand("SELECT * FROM Patient WHERE patient_id = @PatientId", connection))
        {
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@PatientId", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    patient = new PatientDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("patient_id")),
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"))
                    };
                }
            }
            
        }
        return patient;
    }

    public async Task<List<ServiceDTO>> FindServices(int appointmentId)
    {
        var services = new List<ServiceDTO>();
        using (var connection = new SqlConnection(_connectionString))
        using (var command =
               new SqlCommand("SELECT name, service_fee FROM Appointment_Service INNER JOIN Service ON Service.service_id = Appointment_Service.service_id WHERE appointment_id = @AppointmentId", connection))
        {
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    services.Add(new ServiceDTO
                    {
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        ServiceFee = reader.GetDecimal(reader.GetOrdinal("service_fee"))
                    });
                }
            }
        }
        return services;
    }

    public async Task<string> AddAppointment(AppointmentNewRequestDTO request)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new SqlCommand("SELECT * FROM APPOINTMENT WHERE appointment_id = @AppointmentId",
                       connection))
            {
                var found = false;
                command.Parameters.AddWithValue("@AppointmentId", request.AppointmentId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    return "Appointment already exists.";
                }
            }

            using (var command = new SqlCommand("SELECT * FROM PATIENT WHERE patient_id = @Id",
                       connection))
            {
                var found = false;
                command.Parameters.AddWithValue("@Id", request.PatientId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return "Patient does not exist.";
                }
            }
            
            
            using (var command = new SqlCommand("SELECT * FROM PATIENT WHERE pwz = @Pwz",
                       connection))
            {
                var found = false;
                command.Parameters.AddWithValue("@Pwz", request.Pwz);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return "Doctor with given PWZ does not exist.";
                }
            }
            
            var ids = new List<int>();
            using (var command = new SqlCommand("SELECT name, service_id FROM Service",
                       connection))
            {
                var names = new List<string>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        names.Add(reader.GetString(reader.GetOrdinal("name")));
                        ids.Add(reader.GetInt32(reader.GetOrdinal("service_id")));
                    }
                }

                var foundServices = request.Services.FindAll(name => names.Contains(name.Name));
                if (foundServices.Count != request.Services.Count)
                {
                    return "Some or all service names does not exists.";
                }
            }
            
            for (var i = 0; i < ids.Count; i++)
            {
                using (var command =
                       new SqlCommand(
                           "INSERT INTO APPOINTMENT (appointment_id, service_id, service_fee) VALUES (@appointment_id, @service_id, @fee)",
                           connection))
                {
                    // If I had time I would add transatioon with begintranstinAsync and command.Transaction set (try-cathc)
                    command.Parameters.AddWithValue("@appointment_id", request.AppointmentId);
                    command.Parameters.AddWithValue("@service_id", ids[i]);
                    command.Parameters.AddWithValue("@fee", request.Services[i].ServiceFee);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        
        return "Success";
    }
    
}