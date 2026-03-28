namespace Hospital.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Specialization { get; set; }
    public int RoomNumber { get; set; }
}