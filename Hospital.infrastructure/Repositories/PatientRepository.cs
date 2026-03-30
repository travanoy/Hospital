using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    public Task<Patient?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Patient>> GetAllActiveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Patient patient)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStatusAsync(int patientId, bool isActive)
    {
        throw new NotImplementedException();
    }
}