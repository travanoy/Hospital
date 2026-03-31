using Hospital.Application.DTOs;
using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetAllActiveAsync();
    Task<PatientSummaryDto> GetPatientSummaryAsync(int patientId);
    Task ProcessPatientsAsync(IEnumerable<int> patientIds);
}