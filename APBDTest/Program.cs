using APBDTest.Repositories;
using APBDTest.Services;

namespace APBDTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IAppointmentRepository, SqlAppoinmentRepository>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        
        builder.Services.AddControllers();
        
        var app = builder.Build();

        // if (app.Environment.IsDevelopment())
        // {
        //     //
        // }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}