using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;

namespace Hospital.ConsoleApp.MenuActions;

public class MenuActionsHandler : IMenuActions
{
    public const string Admin = "Admin";
    public const string Doctor = "Doctor";

    private readonly IAuthService _authService;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly IApointmentService _apointmentService;

    private User? _currentUser;

    public bool IsLoggedIn => _currentUser is not null;
    public bool IsAdmin => HasRole(Admin);
    public bool IsDoctor => HasRole(Doctor);
    public int? CurrentUserId => _currentUser?.Id;
    public string? CurrentUsername => _currentUser?.Username;

    public MenuActionsHandler(
        IAuthService authService,
        IDoctorService doctorService,
        IPatientService patientService,
        IApointmentService apointmentService)
    {
        _authService = authService;
        _doctorService = doctorService;
        _patientService = patientService;
        _apointmentService = apointmentService;
    }

    public async Task<bool> LoginAsync()
    {
        Console.Write("Username: ");
        var username = (Console.ReadLine() ?? string.Empty).Trim();
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password are required.");
            return false;
        }

        var user = await _authService.LoginAsync(username, password);
        if (user is null)
        {
            Console.WriteLine("Invalid username or password.");
            return false;
        }

        _currentUser = user;
        Console.WriteLine($"Logged in as {_currentUser.Username} ({_currentUser.Role})");
        return true;
    }

    public async Task<bool> RegisterAsync()
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
            return false;
        }

        try
        {
            await _authService.RegisterAsync(username, password, role);
            var user = await _authService.LoginAsync(username, password);
            if (user is null)
            {
                Console.WriteLine("User registered, but auto-login failed.");
                return false;
            }

            _currentUser = user;
            Console.WriteLine($"User registered and logged in as {_currentUser.Username} ({_currentUser.Role}).");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register failed: {ex.Message}");
            return false;
        }
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        return Task.CompletedTask;
    }

    public async Task ShowPatientsAsync()
    {
        if (!EnsureLoggedIn())
        {
            return;
        }

        IEnumerable<Patient> patients;
        if (IsAdmin)
        {
            patients = await _patientService.GetAllActiveAsync();
        }
        else if (IsDoctor)
        {
            // Assumption: User.Id corresponds to Doctor.Id.
            patients = await _doctorService.GetMyActivePatientsAsync(CurrentUserId!.Value);
        }
        else
        {
            Console.WriteLine("Access denied.");
            return;
        }

        foreach (var patient in patients)
        {
            Console.WriteLine($"[{patient.Id}] {patient.FullName}, diagnosis={patient.Diagnosis}, active={patient.IsActive}");
        }
    }

    public async Task ScheduleAppointmentAsync()
    {
        if (!EnsureAdminAccess("Schedule Appointment"))
        {
            return;
        }

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
        if (!EnsureAdminAccess("Transfer Patient"))
        {
            return;
        }

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
        if (!EnsureAdminAccess("Process Patients"))
        {
            return;
        }

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

    private bool EnsureAdminAccess(string operationName)
    {
        if (!EnsureLoggedIn())
        {
            return false;
        }

        if (!IsAdmin)
        {
            Console.WriteLine($"Access denied for '{operationName}'. Admin role required.");
            return false;
        }

        return true;
    }

    private bool EnsureLoggedIn()
    {
        if (IsLoggedIn)
        {
            return true;
        }

        Console.WriteLine("Please login first.");
        return false;
    }

    private bool HasRole(string role)
    {
        return string.Equals(_currentUser?.Role, role, StringComparison.OrdinalIgnoreCase);
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

