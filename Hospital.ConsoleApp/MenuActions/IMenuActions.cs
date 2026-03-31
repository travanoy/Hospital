namespace Hospital.ConsoleApp.MenuActions;

public interface IMenuActions
{
    Task LoginAsync();
    Task RegisterAsync();
    Task ShowPatientsAsync();
    Task ScheduleAppointmentAsync();
    Task TransferPatientAsync();
    Task ProcessPatientsAsync();
}

