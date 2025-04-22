namespace Axon_Job_App.Features.Jobs;

public enum JobStatus
{
    Draft,
    Published,
    Closed,
    Archived
}

public enum JobType
{
    FullTime,
    PartTime,
    Contract,
    Temporary,
    Internship
}

public enum PaymentType
{
    Hourly,
    Daily,
    Weekly,
    Monthly,
    ProjectBased
}

public enum AssignmentStatus
{
    Active,
    Completed,
    Terminated,
    OnHold
}
