using JobApplication.DataModel.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.infrastructure.Persistence
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<DataModel.Entities.Application> Applications { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<job> Jobs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<job>()
                .HasOne(j => j.Recruiter)
                .WithMany()
                .HasForeignKey(j => j.RecruiterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<job>()
                .HasOne(j => j.ClosedBy)
                .WithMany()
                .HasForeignKey(j => j.ClosedByID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
