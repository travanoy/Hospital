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
            if (!_menuActions.IsLoggedIn)
            {
                PrintAuthMenu();
                Console.Write("Select option: ");
                var authOption = Console.ReadLine()?.Trim();

                switch (authOption)
                {
                    case "1":
                        await _menuActions.LoginAsync();
                        break;
                    case "2":
                        await _menuActions.RegisterAsync();
                        break;
                    case "3":
                        Console.WriteLine("Bye.");
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }

                Console.WriteLine();
                continue;
            }

            PrintRolePanel();
            Console.Write("Select option: ");
            var option = Console.ReadLine()?.Trim();

            var shouldExit = await HandleRoleOptionAsync(option);
            if (shouldExit)
            {
                Console.WriteLine("Bye.");
                return;
            }

            Console.WriteLine();
        }
    }

    private static void PrintAuthMenu()
    {
        Console.WriteLine("===== Authentication =====");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
    }

    private void PrintRolePanel()
    {
        if (_menuActions.IsAdmin)
        {
            Console.WriteLine($"===== Admin Panel ({_menuActions.CurrentUsername}) =====");
            Console.WriteLine("1. Show Patients");
            Console.WriteLine("2. Schedule Appointment");
            Console.WriteLine("3. Transfer Patient");
            Console.WriteLine("4. Process Patients");
            Console.WriteLine("5. Logout");
            Console.WriteLine("6. Exit");
            return;
        }

        if (_menuActions.IsDoctor)
        {
            Console.WriteLine($"===== Doctor Panel ({_menuActions.CurrentUsername}) =====");
            Console.WriteLine("1. Show My Patients");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Exit");
            return;
        }

        Console.WriteLine("===== User Panel =====");
        Console.WriteLine("1. Show Patients");
        Console.WriteLine("2. Logout");
        Console.WriteLine("3. Exit");
    }

    private async Task<bool> HandleRoleOptionAsync(string? option)
    {
        if (_menuActions.IsAdmin)
        {
            switch (option)
            {
                case "1":
                    await _menuActions.ShowPatientsAsync();
                    return false;
                case "2":
                    await _menuActions.ScheduleAppointmentAsync();
                    return false;
                case "3":
                    await _menuActions.TransferPatientAsync();
                    return false;
                case "4":
                    await _menuActions.ProcessPatientsAsync();
                    return false;
                case "5":
                    await _menuActions.LogoutAsync();
                    return false;
                case "6":
                    return true;
                default:
                    Console.WriteLine("Unknown option.");
                    return false;
            }
        }

        if (_menuActions.IsDoctor)
        {
            switch (option)
            {
                case "1":
                    await _menuActions.ShowPatientsAsync();
                    return false;
                case "2":
                    await _menuActions.LogoutAsync();
                    return false;
                case "3":
                    return true;
                default:
                    Console.WriteLine("Unknown option.");
                    return false;
            }
        }

        switch (option)
        {
            case "1":
                await _menuActions.ShowPatientsAsync();
                return false;
            case "2":
                await _menuActions.LogoutAsync();
                return false;
            case "3":
                return true;
            default:
                Console.WriteLine("Unknown option.");
                return false;
        }
    }
}

