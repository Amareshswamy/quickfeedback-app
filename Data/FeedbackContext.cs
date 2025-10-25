using Microsoft.EntityFrameworkCore;
using QuickFeedback.Models;

namespace QuickFeedback.Data
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext(DbContextOptions<FeedbackContext> options) : base(options)
        {
        }

        public DbSet<FeedbackEntry> FeedbackEntries { get; set; }
    }
}
