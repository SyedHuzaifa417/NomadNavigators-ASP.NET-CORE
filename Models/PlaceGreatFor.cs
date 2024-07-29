using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

[PrimaryKey("PlaceId", "GreatFor")]
[Table("PlaceGreatFor")]
public partial class PlaceGreatFor
{
    [Key]
    [Column("placeId")]
    public int PlaceId { get; set; }

    [Key]
    [Column("greatFor")]
    [StringLength(100)]
    public string GreatFor { get; set; } = null!;

    [ForeignKey("PlaceId")]
    [InverseProperty("PlaceGreatFors")]
    public virtual Place Place { get; set; } = null!;
}
