namespace Hospital.Application.DTOs;

public class PatientSummaryDto
{
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int TotalAppointments { get; set; }
    public int CompletedAppointments { get; set; }
}

