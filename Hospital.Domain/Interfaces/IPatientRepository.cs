namespace Hospital.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
    Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at);
    Task AddAsync(Appointment appointment);
    Task UpdateStatusAsync(int appointmentId, string status);
}