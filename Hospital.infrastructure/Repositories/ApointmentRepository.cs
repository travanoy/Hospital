using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.infrastructure.Repositories;

public class ApointmentRepository : IApointmentRepository
{
    public Task<IEnumerable<Apointment>> GetByPatientIdAsync(int patientId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Apointment apointment)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStatusAsync(int apointmentId, string status)
    {
        throw new NotImplementedException();
    }
}