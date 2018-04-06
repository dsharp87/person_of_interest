// Database interface file

using Microsoft.EntityFrameworkCore;


namespace person_of_interest.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext (DbContextOptions<ProjectContext> options) : base(options) {}
        public DbSet<User> users { get; set; }

        public DbSet<Quiz> quizes { get; set; }

        public DbSet<Question> questions { get; set; }

        public DbSet<QuizResult> quiz_results { get; set; }
    }
}