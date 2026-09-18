namespace JobApplication.DataModel.Entities
{
    public class Candidate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? CvURL { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
