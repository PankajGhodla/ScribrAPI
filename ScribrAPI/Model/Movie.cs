using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScribrAPI.Model
{
    public partial class Movie
    {
        public Movie()
        {
            RelatedMovie = new HashSet<RelatedMovie>();
        }

        public int MovieId { get; set; }
        [Required]
        [StringLength(255)]
        public string MovieTitle { get; set; }
        [Required]
        [Column("PosterURL")]
        [StringLength(255)]
        public string PosterUrl { get; set; }
        [Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }
        public int MovieLength { get; set; }
        [Required]
        [Column("IMDBLink")]
        [StringLength(255)]
        public string Imdblink { get; set; }
        [Required]
        [StringLength(8000)]
        public string Discription { get; set; }
        [Required]
        [StringLength(255)]
        public string Genres { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }

        [InverseProperty("Movie")]
        public virtual ICollection<RelatedMovie> RelatedMovie { get; set; }
    }
}
