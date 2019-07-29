using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScribrAPI.Model
{
    public partial class MovieDBContext : DbContext
    {
        public MovieDBContext()
        {
        }

        public MovieDBContext(DbContextOptions<MovieDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<RelatedMovie> RelatedMovie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=tcp:moviemsa.database.windows.net,1433;Initial Catalog=MovieDB;Persist Security Info=False;User ID=Pankaj;Password=Ghodla0014;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Discription).IsUnicode(false);

                entity.Property(e => e.Genres).IsUnicode(false);

                entity.Property(e => e.Imdblink).IsUnicode(false);

                entity.Property(e => e.MovieTitle).IsUnicode(false);

                entity.Property(e => e.PosterUrl).IsUnicode(false);
            });

            modelBuilder.Entity<RelatedMovie>(entity =>
            {
                entity.HasKey(e => e.RealtedMovieId)
                    .HasName("PK__RelatedM__857CCEED42163EE7");

                entity.Property(e => e.RelatedImdblink).IsUnicode(false);

                entity.Property(e => e.RelatedMovieTitle).IsUnicode(false);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.RelatedMovie)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("MovieId");
            });
        }
    }
}
