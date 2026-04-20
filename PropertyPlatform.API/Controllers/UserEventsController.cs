using Microsoft.AspNetCore.Mvc;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class TrackEventRequest
        {
            public Guid ListingId { get; set; }
            public string EventType { get; set; } = string.Empty; // View, Click, Contact
            public Guid? UserId { get; set; }
        }

        [HttpPost("track")]
        public async Task<IActionResult> TrackEvent([FromBody] TrackEventRequest request)
        {
            var userEvent = new UserEvent
            {
                ListingId = request.ListingId,
                EventType = request.EventType,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserEvents.Add(userEvent);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("analytics/{listingId}")]
        public IActionResult GetListingAnalytics(Guid listingId)
        {
            var events = _context.UserEvents
                .Where(e => e.ListingId == listingId)
                .AsEnumerable()
                .GroupBy(e => e.EventType)
                .Select(g => new
                {
                    EventType = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(events);
        }
    }
}