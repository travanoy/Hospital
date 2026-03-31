using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private static readonly SemaphoreSlim UpdateStatusLock = new(1, 1);
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Patient>> GetAllActiveAsync()
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.Diagnosis.Contains(diagnosis))
            .ToListAsync();
    }

    public async Task AddAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int patientId, bool isActive)
    {
        await UpdateStatusLock.WaitAsync();
        try
        {
            await _context.Patients
                .Where(p => p.Id == patientId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsActive, isActive));
        }
        finally
        {
            UpdateStatusLock.Release();
        }
    }
}