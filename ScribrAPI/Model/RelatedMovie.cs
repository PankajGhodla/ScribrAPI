using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScribrAPI.Model
{
    public partial class RelatedMovie
    {
        public int RealtedMovieId { get; set; }
        public int? MovieId { get; set; }
        [Required]
        [StringLength(255)]
        public string RelatedMovieTitle { get; set; }
        [Required]
        [Column("RelatedIMDBLink")]
        [StringLength(255)]
        public string RelatedImdblink { get; set; }

        [ForeignKey("MovieId")]
        [InverseProperty("RelatedMovie")]
        public virtual Movie Movie { get; set; }
    }
}
