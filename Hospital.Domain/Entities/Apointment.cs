namespace Hospital.Domain.Entities;

public class Apointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } // Scheduled / Cancelled / Completed
}