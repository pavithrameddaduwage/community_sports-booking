namespace CommunitySportsBooking.Models.ViewModels
{
    public class FacilitySearchViewModel
    {
        public string? FacilityType { get; set; }
        public string? Location { get; set; }
        public string? SearchTerm { get; set; }
        public string? SearchDate { get; set; }
        public string? SearchTime { get; set; }

        public List<Facility> Facilities { get; set; } = new List<Facility>();
        public List<string> AvailableTypes { get; set; } = new List<string>();
        public List<string> AvailableLocations { get; set; } = new List<string>();
    }
}
