using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue Name is mandatory.")]
        [StringLength(100)]
        public string VenueName { get; set; }

        [Required(ErrorMessage = "Location is required for guests to find the venue.")]
        public string Location { get; set; }

        [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000.")]
        public int Capacity { get; set; }

        // Optional fields (Notice the ? after string)
        public string? ImageUrl { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}