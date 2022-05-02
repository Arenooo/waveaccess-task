#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WaveaccessTask.Data;
using WaveaccessTask.Models;

namespace WaveaccessTask.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int NameSortingCount;
        private static int DateSortingCount;
        private static int RatingSortingCount;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }
       
        public IActionResult Sort(SortBy sorting)
        {
            List<Movie> list = null;

            switch (sorting)
            {
                case SortBy.Name:
                    list = (++NameSortingCount % 2 == 0 ? _context.Movie.Select(m => m).OrderByDescending(m => m.Title) : _context.Movie.Select(m => m).OrderBy(m => m.Title)).ToList();
                    break;
                case SortBy.Date:
                    list = (++DateSortingCount % 2 == 0 ? _context.Movie.Select(m => m).OrderByDescending(m => m.ReleaseDate) : _context.Movie.Select(m => m).OrderBy(m => m.ReleaseDate)).ToList();
                    break;
                case SortBy.Rating:
                    list = (++RatingSortingCount % 2 == 0 ? _context.Movie.Select(m => m).OrderByDescending(m => m.Rating) : _context.Movie.Select(m => m).OrderBy(m => m.Rating)).ToList();
                    break;
            }

            return View("Index", list);
        }

        public IActionResult Details(Guid id)
        {
            var movie = GetMovie(id);
            return View(movie);
        }
        
        [Authorize]
        public IActionResult Like(Guid id)
        {
            var movie = _context.Movie.Select(m => m).Where(movie => movie.Id == id).First();
            ++movie.Rating;
            _context.Movie.Update(movie);
            _context.SaveChanges();
            return RedirectToAction("Index", _context.Movie.Select(m => m).ToList());
        }

        public Movie GetMovie(Guid id)
        {
            var movie = _context.Movie.Select(m => m)?.Where(m => m.Id == id).First();

            if(movie == null)
                return null;

            var castIds = _context.ActorStarredInMovie.Select(a => a).Where(a => a.MovieId == id).Select(a => a.ActorId).ToList();
            var actors = _context.Actor.Select(a => a).Where(a => castIds.Contains(a.Id)).ToList();

            return new Movie
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                ActorCast = actors,
                Rating = movie.Rating
                //IsLiked = movie.MovieRatings.Any(s => s.UserId == CurrentUserId),
                //LikeCount = movie.MovieRatings.Count()
            };
        }

        public enum SortBy
        {
            Name,
            Date,
            Rating
        }
    }
}
