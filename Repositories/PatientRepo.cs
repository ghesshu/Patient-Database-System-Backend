using AxonPDS.Data;
using AxonPDS.DTOs.Patient;
using AxonPDS.DTOs.Record;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class PatientRepo(PdsDbContext pdsDbContext) : IPatient
{
    private readonly PdsDbContext service = pdsDbContext;

    private static int CalcAge (DateTime dob){
        var today = DateTime.UtcNow;
        int age = today.Year - dob.Year;

         // Adjust if birthday has not occurred yet this year
        if (dob.Date > today.Date.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    // Get Patient By Id
    public async Task<PatientResponseDto?> GetPatient(Guid id)
{
    // Fetch the patient with records and related data
    var patient = await service.Patients
        .Include(p => p.Records)
            .ThenInclude(r => r.TreatmentRecords)
        .Include(p => p.Records)
            .ThenInclude(r => r.MedicineRecords)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (patient is null)
    {
        return null;
    }

    // Fetch the patient's info separately
    var patientInfo = await service.PatientInfos
        .FirstOrDefaultAsync(pi => pi.PatientId == patient.Id);

    return new PatientResponseDto
    {
        Id = patient.Id,
        FullName = patient.Fullname,
        CreatedAt = patient.CreatedAt,
        Gender = patient.Gender,
        Age = CalcAge(patient.Dob),
        Phonenumber = patient.Phonenumber,
        Email = patient.Email,
        Address = patient.Address,
        Dob = patient.Dob,
        Info = patientInfo != null ? new PatientInfoDto
        {
            BloodGroup = patientInfo.BloodGroup,
            Weight = patientInfo.Weight,
            Height = patientInfo.Height,
            Allergies = patientInfo.Allergies,
            Habits = patientInfo.Habits,
            MedicalHistory = patientInfo.Medicalhistory
        } : null,
        Records = patient.Records.Select(r => new RecordResponseDto
        {
            Id = r.Id,
            PatientId = r.PatientId,
            Complains = r.Complains,
            Diagnosis = r.Diagnosis,
            VitalSigns = r.VitalSigns,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            Treatment = r.TreatmentRecords.Select(tr => tr.TreatmentId.ToString()).ToArray(),
            Medicines = r.MedicineRecords.Select(mr => new MedicineRecordItem
            {
                Medicine = mr.MedicineId.ToString(),
                Instruction = mr.Instruction,
                Quantity = mr.Quantity
            }).ToArray()
        }).ToList<RecordResponseDto?>()
    };
}
    // Get All Patients
    public async Task<(List<PatientResponseDto>, int)> GetAllPatients( string gender = "all", string? dateRange = null, string sort = "newest", int page = 1, int limit = 20, string search = "")
    {
        var query = service.Patients.AsQueryable();

        // Filter by Gender
        if(gender != "all" && Enum.TryParse<Gender>(gender, true, out var genderEnum))
        {
            query = query.Where(p => p.Gender == genderEnum);
        }

        // Handle Date Range Filtering 
        if(!string.IsNullOrEmpty(dateRange))
        {
             var dates = dateRange.Split(" - ");
                if (dates.Length == 2 && DateTime.TryParse(dates[0], out var fromDate) && DateTime.TryParse(dates[1], out var toDate))
                {
                    toDate = toDate.Date.AddDays(1).AddTicks(-1); // Set end of the day
                    query = query.Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate);
                }
        }

        // Handle search
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(p => searchWords.All(word => p.Fullname.Contains(word)));
        }

        // Handle sorting
        query = sort == "oldest" ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);

        // Pagination
        int totalPatients = await query.CountAsync();
        var patients = await query.Skip((page - 1) * limit)
        .Take(limit)
        .Select(p => new PatientResponseDto 
        {
            Id = p.Id,
            FullName = p.Fullname,
            CreatedAt = p.CreatedAt,
            Gender = p.Gender,
            Age = CalcAge(p.Dob)
        })
        .ToListAsync();

        return (patients, totalPatients);

    }

    // Create a new patient.
    public async Task<Patient?> CreatePatient(CreatePatientDto patientDto)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            Fullname = patientDto.Fullname,
            Phonenumber = patientDto.Phonenumber,
            Emergency = patientDto.Emergency,
            Gender = patientDto.Gender,
            Dob = patientDto.Dob,
            Email = patientDto.Email,
            Address = patientDto.Address,
        };

        await service.Patients.AddAsync(patient);
        var changes = await service.SaveChangesAsync();
        
        return changes > 0 ? patient : null; // Returns true if at least one record was saved
    }


    // Update an existing patient.
    public async Task<bool> UpdatePatient(Guid Id, UpdatePatientDto patient)
    {
        var existingPatient = await service.Patients.FindAsync(Id);
        if (existingPatient == null)
        {
            return false; // Patient not found
        }

        // Update fields
        existingPatient.Fullname = patient.Fullname;
        existingPatient.Phonenumber = patient.Phonenumber;
        existingPatient.Email = patient.Email;
        existingPatient.Address = patient.Address;
        existingPatient.Emergency = patient.Emergency;
        existingPatient.Gender = patient.Gender;
        existingPatient.Dob = patient.Dob;

        await service.SaveChangesAsync();
        return true; // Update successful
    }


    // Delete a patient by ID.
    public async Task<bool> DeletePatient(Guid id)
    {
        var patient = await service.Patients.FindAsync(id);
        if (patient == null)
        {
            return false; // Patient not found
        }

        service.Patients.Remove(patient);
        await service.SaveChangesAsync();
        return true; // Deletion successful
    }

    // Check if Patients Has Existing Records
    public async Task<bool> HasRecords(Guid patientId)
    {
        return await service.Patients
            .Where(p => p.Id == patientId)
            .Select(p => p.Records.Any())
            .FirstOrDefaultAsync();
    }
    
    // Check if Patient exist
    public async Task<bool> IsDuplicateAsync(string email, string phoneNumber)
    {
        return await service.Patients
            .AnyAsync(p => p.Email == email || p.Phonenumber == phoneNumber);
    }
}
