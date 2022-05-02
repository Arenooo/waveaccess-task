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
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static int NameSortingCount;
        private static int RatingSortingCount;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actor.ToListAsync());
        }

        public IActionResult Details(Guid id)
        {
            var actor = GetActor(id);
            return View(actor);
        }

        public Actor GetActor(Guid id)
        {
            var actor = _context.Actor.Select(m => m).Where(a => a.Id == id)?.First();

            if (actor == null)
                return null;

            var moviesStarredInIds = _context.ActorStarredInMovie.Select(a => a).Where(a => a.ActorId == id).Select(a => a.MovieId).ToList();
            var movies = _context.Movie.Select(m => m).Where(m => moviesStarredInIds.Contains(m.Id)).ToList();

            return new Actor
            {
                Id = actor.Id,
                Name = actor.Name,
                MoviesStarredIn = movies,
                Rating = actor.Rating
            };
        }

        [Authorize]
        public IActionResult Like(Guid id)
        {
            var actor = _context.Actor.Select(a => a).Where(actor => actor.Id == id).First();
            ++actor.Rating;
            _context.Actor.Update(actor);
            _context.SaveChanges();
            return RedirectToAction("Index", _context.Actor.Select(m => m).ToList());
        }

        public async Task<IActionResult> Movies(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.ActorStarredInMovie.Select(s => s).Where(movie => movie.ActorId == id).Select(movie => movie.MovieId).ToListAsync();
            if (movies == null)
            {
                return NotFound();
            }

            var names = await _context.Movie.Select(movie => movie).Where(movie => movies.Contains(movie.Id)).Select(mov => mov.Title).ToListAsync();

            return View(names);
        }

        public IActionResult Sort(SortBy sorting)
        {
            List<Actor> list = null;

            switch (sorting)
            {
                case SortBy.Name:
                    list = (++NameSortingCount % 2 == 0 ? _context.Actor.Select(m => m).OrderByDescending(m => m.Name) : _context.Actor.Select(m => m).OrderBy(m => m.Name)).ToList();
                    break;
                case SortBy.Rating:
                    list = (++RatingSortingCount % 2 == 0 ? _context.Actor.Select(m => m).OrderByDescending(m => m.Rating) : _context.Actor.Select(m => m).OrderBy(m => m.Rating)).ToList();
                    break;
            }

            return View("Index", list);
        }


        public enum SortBy
        {
            Name,
            Rating
        }
    }

    
}
