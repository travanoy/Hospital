using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(int id);
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task AddAsync(Doctor doctor);
}