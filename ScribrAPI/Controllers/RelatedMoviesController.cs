using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScribrAPI.Model;

namespace ScribrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatedMoviesController : ControllerBase
    {
        private readonly MovieDBContext _context;

        public RelatedMoviesController(MovieDBContext context)
        {
            _context = context;
        }

        // GET: api/RelatedMovies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelatedMovie>>> GetRelatedMovie()
        {
            return await _context.RelatedMovie.ToListAsync();
        }

        // GET: api/RelatedMovies/5
        [HttpGet("GetRelatedMovies{id}")]
        public async Task<ActionResult<IEnumerable<RelatedMovie>>> GetRelatedMovie(int id)
        {

            var relatedMovies = await _context.RelatedMovie.Select(related => new RelatedMovie
            {
                RealtedMovieId = related.RealtedMovieId,
                MovieId = related.MovieId,
                RelatedMovieTitle = related.RelatedMovieTitle,
                RelatedImdblink = related.RelatedImdblink
            }).ToListAsync();

            relatedMovies.RemoveAll(mo => mo.MovieId != id);

            return relatedMovies;

            //var relatedMovie = await _context.RelatedMovie.FindAsync(id);

            //if (relatedMovie == null)
            // {
            //    return NotFound();
            //}

            //return relatedMovie;
        }

        // PUT: api/RelatedMovies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelatedMovie(int id, RelatedMovie relatedMovie)
        {
            if (id != relatedMovie.RealtedMovieId)
            {
                return BadRequest();
            }

            _context.Entry(relatedMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelatedMovieExists(id))
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

        // POST: api/RelatedMovies
        [HttpPost]
        public async Task<ActionResult<RelatedMovie>> PostRelatedMovie(RelatedMovie relatedMovie)
        {
            _context.RelatedMovie.Add(relatedMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelatedMovie", new { id = relatedMovie.RealtedMovieId }, relatedMovie);
        }

        // DELETE: api/RelatedMovies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RelatedMovie>> DeleteRelatedMovie(int id)
        {
            var relatedMovie = await _context.RelatedMovie.FindAsync(id);
            if (relatedMovie == null)
            {
                return NotFound();
            }

            _context.RelatedMovie.Remove(relatedMovie);
            await _context.SaveChangesAsync();

            return relatedMovie;
        }

        private bool RelatedMovieExists(int id)
        {
            return _context.RelatedMovie.Any(e => e.RealtedMovieId == id);
        }
    }
}
