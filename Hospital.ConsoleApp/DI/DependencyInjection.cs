using Hospital.Application.Interfaces;
using Hospital.Application.Services;
using Hospital.ConsoleApp.MenuActions;
using Hospital.ConsoleApp.Menus;
using Hospital.Domain.Interfaces;
using Hospital.infrastructure.Persistence.Data;
using Hospital.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.ConsoleApp.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddHospitalDependencies(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost;Database=HospitalDb;Trusted_Connection=True;TrustServerCertificate=True"));

        services.AddScoped<IApointmentService, ApointmentService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        

        services.AddScoped<IApointmentRepository, ApointmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMenuActions, MenuActionsHandler>();
        services.AddScoped<MainMenuHandler>();

        return services;
    }
}

