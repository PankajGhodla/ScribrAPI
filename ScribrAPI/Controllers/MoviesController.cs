using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScribrAPI.Model;
using ScribrAPI.Helper;
using ScribrAPI.DAL;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace ScribrAPI.Controllers
{
    public class URLDTO
    {
        public String URL { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly MovieDBContext _context;
        private Movie newMovie;
        private string movieID;

        private IMovieRepository movieRepository;
        private readonly IMapper _mapper;
        public MoviesController(MovieDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.movieRepository = new MovieRepository(new MovieDBContext());

        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movie.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody]URLDTO URL)
        {
           
            String movieID = Helper.MovieHelper.GetMovieFromLink(URL.URL);
            Movie newMovie = Helper.MovieHelper.GetMovieFromID(movieID);
            
            
            _context.Movie.Add(newMovie);
            await _context.SaveChangesAsync();


            // We are creating a new context so that loading realted movies don't block the api.
            MovieDBContext tempContext = new MovieDBContext();
            RelatedMoviesController realtedMoviesController = new RelatedMoviesController(tempContext);

            //// This will be executed in the background.
            Task addCaptions = Task.Run(async () =>
            {
                List<RelatedMovie> relatedMovies = Helper.MovieHelper.GetRelatedMovies(movieID);

                for (int i = 0; i < relatedMovies.Count; i++)
                {
                    RelatedMovie relatedMovie = relatedMovies.ElementAt(i);
                    relatedMovie.MovieId = newMovie.MovieId;
                    // posting related movies to the database
                    await realtedMoviesController.PostRelatedMovie(relatedMovie);
                }
            });

            return CreatedAtAction("GetMovie", new { id = newMovie.MovieId }, newMovie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        [HttpGet("SearchByMovie/{movieTitle}")]
        public async Task<ActionResult<IEnumerable<Movie>>> Search(String movieTitle)
        {
            var movies = await _context.Movie.Select(movie => new Movie
            {
                MovieId = movie.MovieId,
                MovieTitle = movie.MovieTitle,
                PosterUrl = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate,
                MovieLength = movie.MovieLength,
                Imdblink = movie.Imdblink,
                Discription = movie.Discription,
                Genres = movie.Genres,
                IsFavourite = movie.IsFavourite,
                //RelatedMovie = movie.RelatedMovie
            }).ToListAsync();

            // only keeping the movies that matches the input string
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            movies.RemoveAll(mo => !mo.MovieTitle.Contains(movieTitle, comp));
            return movies;
        }

        [HttpPatch("update/{id}")]
        public MovieDTO Patch(int id, [FromBody]JsonPatchDocument<MovieDTO> moviePatch)
        {
            //get the original movie object from the DB
            Movie originVideo = movieRepository.GetMovieByID(id);
           
            MovieDTO movieDTO = _mapper.Map<MovieDTO>(originVideo);
            
            moviePatch.ApplyTo(movieDTO);
            
            _mapper.Map(movieDTO, originVideo);
            //update the movie in DB
            _context.Update(originVideo);
            _context.SaveChanges();
            return movieDTO;
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
    }
}
