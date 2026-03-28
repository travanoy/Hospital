namespace Hospital.Domain.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllActiveAsync();
    Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis);
    Task AddAsync(Patient patient);
    Task UpdateStatusAsync(int patientId, bool isActive);
}
