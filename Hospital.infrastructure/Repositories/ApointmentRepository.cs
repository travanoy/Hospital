using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.infrastructure.Repositories;

public class ApointmentRepository : IApointmentRepository
{
    private readonly AppDbContext _context;

    public ApointmentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Apointment>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Apointments
            .AsNoTracking()
            .Where(a => a.PatientId == patientId)
            .ToListAsync();
    }
    

    public async Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at)
    {
        return await _context.Apointments
            .AsNoTracking()
            .AnyAsync(a => a.DoctorId == doctorId && a.ScheduledAt == at);
    }

    public async Task AddAsync(Apointment apointment)
    {
        _context.Apointments.Add(apointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int apointmentId, string status)
    {
        await _context.Apointments
            .Where(a => a.Id == apointmentId)
            .ExecuteUpdateAsync(s => s.SetProperty(a => a.Status, status));
    }
}