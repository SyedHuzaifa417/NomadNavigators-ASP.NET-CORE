using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NomadNavigator_BE_.Models
{
    [Table("Review")]
    public partial class Review
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("rating")]
        public int Rating { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("date", TypeName = "datetime")]
        public DateTime? Date { get; set; }

        [Column("image")]
        public byte[]? Image { get; set; }

        // Not mapped to the database
        [NotMapped]
        public string? ImageBase64
        {
            get => Image != null ? Convert.ToBase64String(Image) : null;
            set => Image = !string.IsNullOrEmpty(value) ? Convert.FromBase64String(value) : null;
        }

    }
}
