using ScribrAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScribrAPI.DAL
{
    public interface IMovieRepository : IDisposable
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovieByID(int MovieId);
        void InsertMovie(Movie movie);
        void DeleteMovie(int MovieId);
        void UpdateMovie(Movie movie);
        void Save();
    }
}
