using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

public class ApointmentService : IApointmentService
{
    private readonly IApointmentRepository _apointmentRepository;
    private readonly IPatientRepository _patientRepository;

    public ApointmentService(IApointmentRepository apointmentRepository, IPatientRepository patientRepository)
    {
        _apointmentRepository = apointmentRepository;
        _patientRepository = patientRepository;
    }

    public async Task ScheduleAppointmentAsync(int patientId, int doctorId, DateTime scheduledAt)
    {
        if (patientId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(patientId), "PatientId must be greater than 0.");
        }

        if (doctorId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorId), "DoctorId must be greater than 0.");
        }

        if (scheduledAt == default)
        {
            throw new ArgumentException("ScheduledAt is required.", nameof(scheduledAt));
        }

        var patient = await _patientRepository.GetByIdAsync(patientId);
        if (patient is null)
        {
            throw new InvalidOperationException($"Patient {patientId} not found.");
        }

        var hasConflict = await _apointmentRepository.DoctorHasAppointmentAsync(doctorId, scheduledAt);
        if (hasConflict)
        {
            throw new InvalidOperationException("Doctor already has an appointment at this time.");
        }

        var apointment = new Apointment
        {
            PatientId = patientId,
            DoctorId = doctorId,
            ScheduledAt = scheduledAt,
            Status = "Scheduled"
        };

        await _apointmentRepository.AddAsync(apointment);
    }

    public async Task TransferPatientAsync(int patientId, int newDoctorId, DateTime scheduledAt)
    {
        if (patientId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(patientId), "PatientId must be greater than 0.");
        }

        if (newDoctorId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newDoctorId), "NewDoctorId must be greater than 0.");
        }

        if (scheduledAt == default)
        {
            throw new ArgumentException("ScheduledAt is required.", nameof(scheduledAt));
        }

        var patientApointments = await _apointmentRepository.GetByPatientIdAsync(patientId);
        var activeApointment = patientApointments
            .Where(a => string.Equals(a.Status, "Scheduled", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.ScheduledAt)
            .FirstOrDefault();

        if (activeApointment is not null)
        {
            await _apointmentRepository.UpdateStatusAsync(activeApointment.Id, "Transferred");
        }

        await ScheduleAppointmentAsync(patientId, newDoctorId, scheduledAt);
    }
}