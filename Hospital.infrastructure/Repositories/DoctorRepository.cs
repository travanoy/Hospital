using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    public Task<Doctor?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Doctor>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Doctor doctor)
    {
        throw new NotImplementedException();
    }
}