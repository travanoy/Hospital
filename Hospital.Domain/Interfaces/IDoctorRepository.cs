using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(int id);
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Patient>> GetActivePatientsByDoctorIdAsync(int doctorId);
    Task AddAsync(Doctor doctor);
}