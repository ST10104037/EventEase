namespace EventEase.Models;

public class Venue
{
    public int VenueId { get; set; }
    public string VenueName { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public string ImageUrl { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public bool IsAvailable { get; set; } = true;
}



