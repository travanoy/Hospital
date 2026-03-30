using Hospital.Application.Interfaces;
using Hospital.ConsoleApp.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddHospitalDependencies();

        using var serviceProvider = services.BuildServiceProvider();

        var patientService = serviceProvider.GetRequiredService<IPatientService>();
        Console.WriteLine($"DI configured: {patientService.GetType().Name}");
    }
}