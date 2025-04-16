using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpSkillApi.Models
{
    public enum ApplicationStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public partial class ApplicationStatus
    {
        public int ApplicationStatusId { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public ApplicationStatusEnum StatusEnum
        {
            get => (ApplicationStatusEnum)Status;
            set => Status = (int)value;
        }

        public virtual ICollection<VolunteeringApplication> VolunteeringApplications { get; set; } = new List<VolunteeringApplication>();
        public virtual ICollection<VolunteeringJob> VolunteeringJobs { get; set; } = new List<VolunteeringJob>();
        public virtual ICollection<WorkerApplication> WorkerApplications { get; set; } = new List<WorkerApplication>();
    }
}