using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

public partial class Restaurant
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("placeId")]
    public int? PlaceId { get; set; }

    [Column("restaurantName")]
    [StringLength(100)]
    public string RestaurantName { get; set; } = null!;

    [Column("rating", TypeName = "decimal(2, 1)")]
    public decimal Rating { get; set; }

    [Column("speciality")]
    [StringLength(100)]
    public string Speciality { get; set; } = null!;

    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column("img", TypeName = "image")]
    public byte[]? Img { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    [InverseProperty("Restaurant")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("PlaceId")]
    [InverseProperty("Restaurants")]
    public virtual Place? Place { get; set; }
}
