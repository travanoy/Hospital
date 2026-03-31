namespace Hospital.ConsoleApp.MenuActions;

public interface IMenuActions
{
    bool IsLoggedIn { get; }
    bool IsAdmin { get; }
    bool IsDoctor { get; }
    int? CurrentUserId { get; }
    string? CurrentUsername { get; }

    Task<bool> LoginAsync();
    Task<bool> RegisterAsync();
    Task LogoutAsync();
    Task ShowPatientsAsync();
    Task ScheduleAppointmentAsync();
    Task TransferPatientAsync();
    Task ProcessPatientsAsync();
}

