namespace Hospital.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Diagnosis { get; set; }
    public bool IsActive { get; set; }
}
