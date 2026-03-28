namespace Hospital.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // hash
    public string Role { get; set; } // Admin / Doctor
}