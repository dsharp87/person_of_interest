using Microsoft.EntityFrameworkCore;


namespace person_of_interest.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext (DbContextOptions<ProjectContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
    }
}