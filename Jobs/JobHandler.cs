using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.EntityFrameworkCore;
using Axon_Job_App.Common.Extensions;
using Axon_Job_App.Features.Clients;
using Axon_Job_App.Features.Jobs;

namespace Axon_Job_App.Features.Jobs;

public class JobHandler
{
    public async Task<CallResult<JobResponse>> Handle(
        JobMutation.CreateJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var clientExists = await db.Clients.AnyAsync(c => c.Id == command.Input.ClientId, ct);
            if (!clientExists)
                return CallResult<JobResponse>.error("Client not found");

            var job = new Job
            {
                ClientId = command.Input.ClientId,
                Title = command.Input.Title,
                TemporaryType = command.Input.TemporaryType,
                JobType = command.Input.JobType,
                PaymentType = command.Input.PaymentType,
                PaymentAmount = command.Input.PaymentAmount,
                Duties = command.Input.Duties,
                Requirements = command.Input.Requirements,
                JobHours = command.Input.JobHours,
                Location = command.Input.Location,
                StartDate = command.Input.StartDate,
                NumberOfRoles = command.Input.NumberOfRoles,
                WorkingHours = command.Input.WorkingHours
            };

            db.Jobs.Add(job);
            await db.SaveChangesAsync(ct);

            return CallResult<JobResponse>.ok(new JobResponse(
                job.Id,
                job.ClientId,
                job.Title,
                job.TemporaryType,
                job.JobType,
                job.Status,
                job.PaymentType,
                job.PaymentAmount,
                job.Duties,
                job.Requirements,
                job.JobHours,
                job.Location,
                job.Published,
                job.StartDate,
                job.NumberOfRoles,
                job.WorkingHours,
                job.CreatedAt,
                job.UpdatedAt
            ), "Job created successfully");
        }
        catch (Exception e)
        {
            return CallResult<JobResponse>.error(e.Message);
        }
    }

    public async Task<CallResult<JobResponse>> Handle(
        JobMutation.UpdateJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var job = await db.Jobs.FindAsync(new object?[] { command.Id }, ct);
            if (job == null)
                return CallResult<JobResponse>.error("Job not found");

            if (!string.IsNullOrEmpty(command.Input.Title))
                job.Title = command.Input.Title;

            if (!string.IsNullOrEmpty(command.Input.TemporaryType))
                job.TemporaryType = command.Input.TemporaryType;

            if (command.Input.JobType.HasValue)
                job.JobType = command.Input.JobType.Value;

            if (command.Input.PaymentType.HasValue)
                job.PaymentType = command.Input.PaymentType.Value;

            if (command.Input.PaymentAmount.HasValue)
                job.PaymentAmount = command.Input.PaymentAmount.Value;

            if (command.Input.Duties != null)
                job.Duties = command.Input.Duties;

            if (command.Input.Requirements != null)
                job.Requirements = command.Input.Requirements;

            if (!string.IsNullOrEmpty(command.Input.JobHours))
                job.JobHours = command.Input.JobHours;

            if (!string.IsNullOrEmpty(command.Input.Location))
                job.Location = command.Input.Location;

            if (command.Input.Published.HasValue)
                job.Published = command.Input.Published.Value;

            if (command.Input.StartDate.HasValue)
                job.StartDate = command.Input.StartDate.Value;

            if (command.Input.NumberOfRoles.HasValue)
                job.NumberOfRoles = command.Input.NumberOfRoles.Value;

            if (!string.IsNullOrEmpty(command.Input.WorkingHours))
                job.WorkingHours = command.Input.WorkingHours;

            job.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            return CallResult<JobResponse>.ok(new JobResponse(
                job.Id,
                job.ClientId,
                job.Title,
                job.TemporaryType,
                job.JobType,
                job.Status,
                job.PaymentType,
                job.PaymentAmount,
                job.Duties,
                job.Requirements,
                job.JobHours,
                job.Location,
                job.Published,
                job.StartDate,
                job.NumberOfRoles,
                job.WorkingHours,
                job.CreatedAt,
                job.UpdatedAt
            ), "Job updated successfully");
        }
        catch (Exception e)
        {
            return CallResult<JobResponse>.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.DeleteJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var job = await db.Jobs.FindAsync(new object?[] { command.Id }, ct);
            if (job == null)
                return CallResult.error("Job not found");

            db.Jobs.Remove(job);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Job deleted successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.PublishJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var job = await db.Jobs.FindAsync(new object?[] { command.Id }, ct);
            if (job == null)
                return CallResult.error("Job not found");

            job.Published = true;
            job.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Job published successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.AssignJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var jobExists = await db.Jobs.AnyAsync(j => j.Id == command.Input.JobId, ct);
            if (!jobExists)
                return CallResult.error("Job not found");

            var candidateExists = await db.Candidates.AnyAsync(c => c.Id == command.Input.CandidateId, ct);
            if (!candidateExists)
                return CallResult.error("Candidate not found");

            var assignment = new JobAssignment
            {
                JobId = command.Input.JobId,
                CandidateId = command.Input.CandidateId,
                Status = command.Input.Status
            };

            db.JobAssignments.Add(assignment);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Job assigned successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.UpdateAssignmentStatus command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            var assignment = await db.JobAssignments
                .FirstOrDefaultAsync(a => 
                    a.JobId == command.JobId && 
                    a.CandidateId == command.CandidateId, ct);

            if (assignment == null)
                return CallResult.error("Assignment not found");

            assignment.Status = command.Status;
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Assignment status updated successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
}