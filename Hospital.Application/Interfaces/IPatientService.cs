using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetAllActiveAsync();
    Task ProcessPatientsAsync(IEnumerable<int> patientIds);
}