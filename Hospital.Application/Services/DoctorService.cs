using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public Task<IEnumerable<Patient>> GetMyPatientsAsync(int doctorId)
    {
        return _doctorRepository.GetPatientsByDoctorIdAsync(doctorId);
    }

    public Task<IEnumerable<Patient>> GetMyActivePatientsAsync(int doctorId)
    {
        return _doctorRepository.GetActivePatientsByDoctorIdAsync(doctorId);
    }
}