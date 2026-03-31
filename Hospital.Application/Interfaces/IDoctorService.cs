using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<Patient>> GetMyPatientsAsync(int doctorId);
    Task<IEnumerable<Patient>> GetMyActivePatientsAsync(int doctorId);
}