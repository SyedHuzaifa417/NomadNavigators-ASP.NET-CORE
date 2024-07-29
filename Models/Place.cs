using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

public partial class Place
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("country")]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column("img")]
    public byte[] Img { get; set; } = null!;

    [NotMapped]
    public string ImageBase64
    {
        get => Img != null ? Convert.ToBase64String(Img) : null;
        set => Img = !string.IsNullOrEmpty(value) ? Convert.FromBase64String(value) : null;
    }

    [Column("popularity")]
    public int Popularity { get; set; }

    [Column("description")]
    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [Column("latitude", TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column("longitude", TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [InverseProperty("Place")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Place")]
    public virtual ICollection<PlaceGreatFor> PlaceGreatFors { get; set; } = new List<PlaceGreatFor>();

    [InverseProperty("Place")]
    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
