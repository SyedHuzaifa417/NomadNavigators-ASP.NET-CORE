using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

public partial class Flight
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("from")]
    [StringLength(100)]
    public string From { get; set; } = null!;

    [Column("to")]
    [StringLength(100)]
    public string To { get; set; } = null!;

    [Column("company")]
    [StringLength(100)]
    public string Company { get; set; } = null!;

    [Column("date")]
    public DateOnly Date { get; set; }

    public TimeOnly DepartureTime { get; set; }

    public TimeOnly ArrivalTime { get; set; }

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column("returnDate")]
    public DateOnly? ReturnDate { get; set; }

    public TimeOnly? ReturnDepartureTime { get; set; }

    public TimeOnly? ReturnArrivalTime { get; set; }

    [StringLength(100)]
    public string? ReturnCompany { get; set; }

    [StringLength(100)]
    public string? ReturnFrom { get; set; }

    [StringLength(100)]
    public string? ReturnTo { get; set; }

    [InverseProperty("Flight")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
