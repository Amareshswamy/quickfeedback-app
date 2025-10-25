using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFeedback.Data;
using QuickFeedback.Models;

namespace QuickFeedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackContext _context;

        public FeedbackController(FeedbackContext context)
        {
            _context = context;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackEntry>>> GetFeedbackEntries()
        {
            // Return newest first
            return await _context.FeedbackEntries
                                 .OrderByDescending(f => f.CreatedAt)
                                 .ToListAsync();
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackEntry>> PostFeedbackEntry(FeedbackEntry feedbackEntry)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(feedbackEntry.Name) ||
                string.IsNullOrWhiteSpace(feedbackEntry.Message))
            {
                return BadRequest("Name and Message are required.");
            }

            feedbackEntry.CreatedAt = DateTime.UtcNow; // Ensure server-side timestamp

            _context.FeedbackEntries.Add(feedbackEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedbackEntries), new { id = feedbackEntry.Id }, feedbackEntry);
        }
    }
}
