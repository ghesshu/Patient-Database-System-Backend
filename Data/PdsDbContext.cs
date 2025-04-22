using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AxonPDS.Data;

public class PdsDbContext(DbContextOptions<PdsDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    static PdsDbContext()
    {
        Batteries_V2.Init(); // Proper initialization for SQLitePCL
    }

    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<MedicineRecord> MedicineRecords { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientInfo> PatientInfos { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<TreatmentRecord> TreatmentRecords { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship Declarations
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.PatientInfo)
            .WithOne(pi => pi.Patient)
            .HasForeignKey<PatientInfo>(p => p.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Records)
            .WithOne(r => r.Patient)
            .HasForeignKey(r => r.PatientId);

        modelBuilder.Entity<TreatmentRecord>()
            .HasKey(tr => new { tr.RecordId, tr.TreatmentId });

        modelBuilder.Entity<Record>()
            .HasMany(r => r.TreatmentRecords)
            .WithOne(rt => rt.Record)
            .HasForeignKey(rt => rt.RecordId);

        modelBuilder.Entity<Treatment>()
            .HasMany(t => t.TreatmentRecords)
            .WithOne(tr => tr.Treatment)
            .HasForeignKey(tr => tr.TreatmentId);

        modelBuilder.Entity<MedicineRecord>()
            .HasKey(mr => new { mr.RecordId, mr.MedicineId });

        modelBuilder.Entity<Record>()
            .HasMany(r => r.MedicineRecords)
            .WithOne(mr => mr.Record)
            .HasForeignKey(mr => mr.RecordId);

        modelBuilder.Entity<Medicine>()
            .HasMany(m => m.MedicineRecords)
            .WithOne(mr => mr.Medicine)
            .HasForeignKey(mr => mr.MedicineId);

        // Auto Adition And Deletion of Medicine records in Record Table
        // modelBuilder.Entity<MedicineRecord>()
        //     .HasOne(m => m.Record)
        //     .WithMany(r => r.MedicineRecords)
        //     .HasForeignKey(m => m.RecordId)
        //     .OnDelete(DeleteBehavior.Cascade);

        


        // Set Up Indexes
         modelBuilder.Entity<Patient>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.Phonenumber)
            .IsUnique();
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>().Where(e => e.Entity is not null))
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>().Where(e => e.Entity is not null))
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}