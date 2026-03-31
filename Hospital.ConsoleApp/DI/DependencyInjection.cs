using Hospital.Application.Interfaces;
using Hospital.Application.Services;
using Hospital.Domain.Interfaces;
using Hospital.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.ConsoleApp.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddHospitalDependencies(this IServiceCollection services)
    {
        services.AddScoped<IApointmentService, ApointmentService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        

        services.AddScoped<IApointmentRepository, ApointmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

