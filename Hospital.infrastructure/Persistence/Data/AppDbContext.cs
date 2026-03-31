using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.infrastructure.Persistence.Data;

public class AppDbContext : DbContext
{
    private const string ConnectionStringEnvName = "HOSPITAL_DB_CONNECTION";
    private const string DefaultConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=HospitalDb;Trusted_Connection=True;TrustServerCertificate=True";

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public static string GetConnectionString()
    {
        return Environment.GetEnvironmentVariable(ConnectionStringEnvName) ?? DefaultConnectionString;
    }

    public DbSet<Apointment> Apointments => Set<Apointment>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<User> Users => Set<User>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apointment>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ScheduledAt).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(32).IsRequired();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Specialization).HasMaxLength(120).IsRequired();
            entity.Property(x => x.RoomNumber).IsRequired();
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.DateOfBirth).IsRequired();
            entity.Property(x => x.Diagnosis).HasMaxLength(400).IsRequired();
            entity.Property(x => x.IsActive).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Username).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Password).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(32).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}