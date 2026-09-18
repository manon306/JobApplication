namespace JobApplication.DataModel.Entities
{
    public class job
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime? ClosedAt { get; set; }
        public string? RecruiterId { get; set; }
        public ApplicationUser? Recruiter { get; set; }
        public string? ClosedByID { get; set; }
        public ApplicationUser? ClosedBy { get; set; }

    }
}
