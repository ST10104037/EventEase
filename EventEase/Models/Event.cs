namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty; // Required
        public DateTime EventDate { get; set; }

        // Make these nullable with ?
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public int VenueId { get; set; }

        // Navigation property should be nullable to avoid validation issues during Create
        public Venue? Venue { get; set; }
    }
}