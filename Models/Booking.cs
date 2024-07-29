using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

public partial class Booking
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bookerName")]
    [StringLength(100)]
    public string BookerName { get; set; } = null!;

    [Column("bookerEmail")]
    [StringLength(100)]
    public string BookerEmail { get; set; } = null!;

    [Column("placeId")]
    public int PlaceId { get; set; }

    [Column("restaurantId")]
    public int? RestaurantId { get; set; }

    [Column("flightId")]
    public int FlightId { get; set; }

    [Column("bookingDate")]
    public DateOnly BookingDate { get; set; }

    [Column("bookingTime")]
    public TimeOnly BookingTime { get; set; }

    [Column("numberOfPersons")]
    public int NumberOfPersons { get; set; }

    [Column("totalPrice", TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("Bookings")]
    public virtual Flight Flight { get; set; } = null!;

    [ForeignKey("PlaceId")]
    [InverseProperty("Bookings")]
    public virtual Place Place { get; set; } = null!;

    [ForeignKey("RestaurantId")]
    [InverseProperty("Bookings")]
    public virtual Restaurant? Restaurant { get; set; }
}

public class BookingDto
{
    public int Id { get; set; }
    public string BookerName { get; set; }
    public string BookerEmail { get; set; }
    public int PlaceId { get; set; }
    public int? RestaurantId { get; set; }
    public int FlightId { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingTime { get; set; }
    public int NumberOfPersons { get; set; }
    public decimal TotalPrice { get; set; }

    // Include only necessary properties from related entities
    public string PlaceName { get; set; }
    public string FlightFrom { get; set; }
    public string FlightTo { get; set; }
    public string RestaurantName { get; set; }
}