using Hospital.ConsoleApp.MenuActions;

namespace Hospital.ConsoleApp.Menus;

public class MainMenuHandler
{
    private readonly IMenuActions _menuActions;

    public MainMenuHandler(IMenuActions menuActions)
    {
        _menuActions = menuActions;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            PrintMenu();
            Console.Write("Select option: ");
            var option = Console.ReadLine()?.Trim();

            switch (option)
            {
                case "1":
                    await _menuActions.LoginAsync();
                    break;
                case "2":
                    await _menuActions.RegisterAsync();
                    break;
                case "3":
                    await _menuActions.ShowPatientsAsync();
                    break;
                case "4":
                    await _menuActions.ScheduleAppointmentAsync();
                    break;
                case "5":
                    await _menuActions.TransferPatientAsync();
                    break;
                case "6":
                    await _menuActions.ProcessPatientsAsync();
                    break;
                case "7":
                    Console.WriteLine("Bye.");
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===== Hospital Menu =====");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Show Patients");
        Console.WriteLine("4. Schedule Appointment");
        Console.WriteLine("5. Transfer Patient");
        Console.WriteLine("6. Process Patients");
        Console.WriteLine("7. Exit");
    }
}

