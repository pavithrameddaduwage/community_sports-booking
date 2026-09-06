using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunitySportsBooking.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [Required]
        public int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        [StringLength(50)]
        public string TimeSlot { get; set; } = string.Empty; // e.g. "09:00 - 10:00", "14:00 - 15:00"

        [StringLength(30)]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
