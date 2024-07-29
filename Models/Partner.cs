using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

[Index("Email", Name = "UQ__Partners__A9D10534216B2CE4", IsUnique = true)]
public partial class Partner
{
    [Key]
    [Column("partnerId")]
    public int PartnerId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("contact")]
    [StringLength(100)]
    public string Contact { get; set; } = null!;

    [StringLength(100)]
    public string AreaOfInterest { get; set; } = null!;

    [StringLength(100)]
    public string? Country { get; set; }
}
