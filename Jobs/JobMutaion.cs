using Axon_Job_App.Common;
using Axon_Job_App.Features.Clients;
using Cai;

namespace Axon_Job_App.Features.Jobs;

public class JobMutation
{
    [Mutation<CallResult<JobResponse>>]
    public record CreateJob(CreateJobRequest Input);

    [Mutation<CallResult<JobResponse>>]
    public record UpdateJob(long Id, UpdateJobRequest Input);

    [Mutation<CallResult>]
    public record DeleteJob(long Id);

    [Mutation<CallResult>]
    public record PublishJob(long Id);

    [Mutation<CallResult>]
    public record AssignJob(AssignJobRequest Input);

    [Mutation<CallResult>]
    public record UpdateAssignmentStatus(long JobId, long CandidateId, AssignmentStatus Status);
}