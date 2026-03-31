using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _context.Doctors
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId)
    {
        if (doctorId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorId), "DoctorId must be greater than 0.");
        }

        return await _context.Apointments
            .AsNoTracking()
            .Where(ap => ap.DoctorId == doctorId)
            .Join(
                _context.Patients.AsNoTracking(),
                ap => ap.PatientId,
                p => p.Id,
                (ap, p) => p)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetActivePatientsByDoctorIdAsync(int doctorId)
    {
        var patients = await GetPatientsByDoctorIdAsync(doctorId);
        return patients.Where(p => p.IsActive).ToArray();
    }
}