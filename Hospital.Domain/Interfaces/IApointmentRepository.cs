using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

public interface IApointmentRepository
{
    Task<IEnumerable<Apointment>> GetByPatientIdAsync(int patientId);
    Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at);
    Task AddAsync(Apointment apointment);
    Task UpdateStatusAsync(int apointmentId, string status);
}
