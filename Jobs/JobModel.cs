using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Axon_Job_App.Features.Candidates;

using Axon_Job_App.Helpers;

namespace Axon_Job_App.Features.Jobs;

public class Job : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [ForeignKey(nameof(Client))]
    public long ClientId { get; set; }

    public virtual Axon_Job_App.Features.Clients.Client Client { get; set; } = null!;

    [Required]
    public string Title { get; set; } = null!;

    public string? TemporaryType { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public JobType JobType { get; set; }


    [Column(TypeName = "varchar(20)")]
    public JobStatus Status { get; set; } = JobStatus.Draft;


    [Required]
    [Column(TypeName = "varchar(20)")]
    public PaymentType PaymentType { get; set; }
    

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaymentAmount { get; set; }

    [Required]
    [Column(TypeName = "text[]")]
    public string[] Duties { get; set; } = Array.Empty<string>();

    [Required]
    [Column(TypeName = "text[]")]
    public string[] Requirements { get; set; } = Array.Empty<string>();

    [Required]
    public string JobHours { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    public bool Published { get; set; } = false;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public int NumberOfRoles { get; set; }

    public string? WorkingHours { get; set; }

    public virtual ICollection<JobAssignment> Assignments { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


}

public class JobAssignment
{
    public long JobId { get; set; }
    public virtual Job Job { get; set; } = null!;

    public long CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "varchar(20)")]
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;

}


