using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;

namespace Hospital.ConsoleApp.MenuActions;

public class MenuActionsHandler : IMenuActions
{
    private readonly IAuthService _authService;
    private readonly IPatientService _patientService;
    private readonly IApointmentService _apointmentService;
    private User? _currentUser;

    public MenuActionsHandler(
        IAuthService authService,
        IPatientService patientService,
        IApointmentService apointmentService)
    {
        _authService = authService;
        _patientService = patientService;
        _apointmentService = apointmentService;
    }

    public async Task LoginAsync()
    {
        Console.Write("Username: ");
        var username = (Console.ReadLine() ?? string.Empty).Trim();
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password are required.");
            return;
        }

        var user = await _authService.LoginAsync(username, password);
        if (user is null)
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        _currentUser = user;
        Console.WriteLine($"Logged in as {_currentUser.Username} ({_currentUser.Role})");
    }

    public async Task RegisterAsync()
    {
        Console.Write("Username: ");
        var username = (Console.ReadLine() ?? string.Empty).Trim();
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? string.Empty;
        Console.Write("Role: ");
        var roleInput = Console.ReadLine();
        var role = string.IsNullOrWhiteSpace(roleInput) ? "Doctor" : roleInput.Trim();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password are required.");
            return;
        }

        try
        {
            await _authService.RegisterAsync(username, password, role);
            Console.WriteLine("User registered.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register failed: {ex.Message}");
        }
    }

    public async Task ShowPatientsAsync()
    {
        var patients = await _patientService.GetAllActiveAsync();

        foreach (var patient in patients)
        {
            Console.WriteLine($"[{patient.Id}] {patient.FullName}, diagnosis={patient.Diagnosis}, active={patient.IsActive}");
        }
    }

    public async Task ScheduleAppointmentAsync()
    {
        if (!TryReadPositiveInt("PatientId", out var patientId) || !TryReadPositiveInt("DoctorId", out var doctorId))
        {
            return;
        }

        if (!TryReadDateTimeOrNow(out var scheduledAt))
        {
            return;
        }

        try
        {
            await _apointmentService.ScheduleAppointmentAsync(patientId, doctorId, scheduledAt);
            Console.WriteLine("Appointment scheduled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Schedule failed: {ex.Message}");
        }
    }

    public async Task TransferPatientAsync()
    {
        if (!TryReadPositiveInt("PatientId", out var patientId) || !TryReadPositiveInt("NewDoctorId", out var newDoctorId))
        {
            return;
        }

        if (!TryReadDateTimeOrNow(out var scheduledAt))
        {
            return;
        }

        try
        {
            await _apointmentService.TransferPatientAsync(patientId, newDoctorId, scheduledAt);
            Console.WriteLine("Patient transferred.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transfer failed: {ex.Message}");
        }
    }

    public async Task ProcessPatientsAsync()
    {
        Console.Write("Patient IDs (comma-separated): ");
        var raw = Console.ReadLine() ?? string.Empty;
        var ids = raw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => int.TryParse(x, out var id) ? id : (int?)null)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Where(id => id > 0)
            .ToArray();

        if (ids.Length == 0)
        {
            Console.WriteLine("No valid IDs provided.");
            return;
        }

        await _patientService.ProcessPatientsAsync(ids);
    }

    private static bool TryReadPositiveInt(string field, out int value)
    {
        Console.Write($"{field}: ");
        var raw = Console.ReadLine();
        if (!int.TryParse(raw, out value) || value <= 0)
        {
            Console.WriteLine($"Invalid {field}. Must be a positive integer.");
            return false;
        }

        return true;
    }

    private static bool TryReadDateTimeOrNow(out DateTime value)
    {
        Console.Write("ScheduledAt (yyyy-MM-dd HH:mm, empty=now): ");
        var raw = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(raw))
        {
            value = DateTime.Now;
            return true;
        }

        if (DateTime.TryParse(raw, out var dateTime))
        {
            value = dateTime;
            return true;
        }

        Console.WriteLine("Invalid datetime format.");
        value = default;
        return false;
    }
}

