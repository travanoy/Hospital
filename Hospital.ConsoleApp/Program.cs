using Hospital.ConsoleApp.DI;
using Hospital.ConsoleApp.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.ConsoleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddHospitalDependencies();

        using var serviceProvider = services.BuildServiceProvider();
        var menu = serviceProvider.GetRequiredService<MainMenuHandler>();
        await menu.RunAsync();
    }
}