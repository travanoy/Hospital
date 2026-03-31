using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

public class PatientService : IPatientService
{
    private const int MaxConcurrentTasks = 4;
    private readonly IApointmentRepository _apointmentRepository;
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository, IApointmentRepository apointmentRepository)
    {
        _patientRepository = patientRepository;
        _apointmentRepository = apointmentRepository;
    }

    public Task<IEnumerable<Patient>> GetAllActiveAsync()
    {
        return _patientRepository.GetAllActiveAsync();
    }

    public async Task<PatientSummaryDto> GetPatientSummaryAsync(int patientId)
    {
        if (patientId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(patientId), "PatientId must be greater than 0.");
        }

        var patientTask = _patientRepository.GetByIdAsync(patientId);
        var apointmentsTask = _apointmentRepository.GetByPatientIdAsync(patientId);

        await Task.WhenAll(patientTask, apointmentsTask);

        var patient = await patientTask;
        if (patient is null)
        {
            throw new KeyNotFoundException($"Patient with id {patientId} was not found.");
        }

        var apointments = (await apointmentsTask).ToArray();
        var completedCount = apointments.Count(a =>
            string.Equals(a.Status, "Completed", StringComparison.OrdinalIgnoreCase));

        return new PatientSummaryDto
        {
            PatientId = patient.Id,
            FullName = patient.FullName,
            DateOfBirth = patient.DateOfBirth,
            Diagnosis = patient.Diagnosis,
            IsActive = patient.IsActive,
            TotalAppointments = apointments.Length,
            CompletedAppointments = completedCount
        };
    }

    public async Task ProcessPatientsAsync(IEnumerable<int> patientIds)
    {
        ArgumentNullException.ThrowIfNull(patientIds);

        var ids = patientIds.ToArray();
        if (ids.Length == 0)
        {
            throw new ArgumentException("At least one patient id is required.", nameof(patientIds));
        }

        if (ids.Any(id => id <= 0))
        {
            throw new ArgumentException("Patient ids must be greater than 0.", nameof(patientIds));
        }

        using var semaphore = new SemaphoreSlim(MaxConcurrentTasks, MaxConcurrentTasks);
        var tasks = ids.Select(id => ProcessPatientAsync(id, semaphore));

        await Task.WhenAll(tasks);
    }

    private async Task ProcessPatientAsync(int patientId, SemaphoreSlim semaphore)
    {
        await semaphore.WaitAsync();
        try
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            var delay = Random.Shared.Next(200, 501);
            await Task.Delay(delay);

            if (patient is null)
            {
                Console.WriteLine($"Patient {patientId}: not found (delay {delay} ms)");
                return;
            }

            Console.WriteLine(
                $"Patient {patientId}: {patient.FullName}, diagnosis={patient.Diagnosis}, active={patient.IsActive} (delay {delay} ms)");
        }
        finally
        {
            semaphore.Release();
        }
    }
}