using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseBookingSystem.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }

        [Required(ErrorMessage = "Event type name is required.")]
        [StringLength(100, ErrorMessage = "Event type name cannot exceed 100 characters.")]
        [Display(Name = "Event Type")]
        public string EventTypeName { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}