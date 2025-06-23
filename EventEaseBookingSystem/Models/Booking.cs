using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Please select an event")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Please select a venue")]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Booking Date & Time")]
        public DateTime BookingDate { get; set; }

        // Navigation properties
        public virtual Event? Event { get; set; }
        public virtual Venue? Venue { get; set; }
    }
}