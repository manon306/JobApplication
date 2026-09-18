using JobApplication.DataModel.Enums;

namespace JobApplication.DataModel.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public job job { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public DateTime AppliedAt { get; set; }
        public jobApplicayionStatus Status { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}
