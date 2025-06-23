using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Http; // Needed for IFormFile

public class Venue
{
    [Key]
    public int VenueId { get; set; }  

    [Required(ErrorMessage = "Venue name is required.")]
    [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters.")]
    public string VenueName { get; set; } = string.Empty; 

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters.")]
    public string Location { get; set; } = string.Empty;  

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000.")]
    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }  // Url to blob in Azure

    [NotMapped]
    public IFormFile ImageFile { get; set; }  // For file uploads

    // Navigation Properties 
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public Venue()
    {
        //constructor initialization
        VenueName = string.Empty;
        Location = string.Empty;
    }
}