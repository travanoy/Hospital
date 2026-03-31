namespace Hospital.Application.Interfaces;

public interface IApointmentService
{
    Task ScheduleAppointmentAsync(int patientId, int doctorId, DateTime scheduledAt);
    Task TransferPatientAsync(int patientId, int newDoctorId, DateTime scheduledAt);
}