using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

public class PatientService : IPatientService
{
    private const int MaxConcurrentTasks = 4;
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public Task<IEnumerable<Patient>> GetAllActiveAsync()
    {
        return _patientRepository.GetAllActiveAsync();
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