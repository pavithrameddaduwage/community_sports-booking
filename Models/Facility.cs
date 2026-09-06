using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunitySportsBooking.Models
{
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }

        [Required(ErrorMessage = "Facility name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Facility type is required")]
        [StringLength(50)]
        public string FacilityType { get; set; } = string.Empty; // Tennis, Soccer, Basketball, Badminton, Swimming

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000)]
        public decimal HourlyRate { get; set; }

        public int Capacity { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
