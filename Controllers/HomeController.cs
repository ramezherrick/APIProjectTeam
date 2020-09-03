using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIProject.Models;
using Microsoft.Extensions.Configuration;

namespace APIProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly FavoriteDbContext _context;

        private readonly MovieDAL _movieDal;
        //for hiding apikey
        private readonly string _apiKey;

        public HomeController(IConfiguration configuration, FavoriteDbContext context)
        {
            //for hiding api key
            _apiKey = configuration.GetSection("ApiKeys")["MovieAPIkey"];
            _movieDal = new MovieDAL(_apiKey);
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchPage()
        {
            //var userMovie = await _movieDal.GetSearch();
            return View();
        }

        public async Task<IActionResult> SearchResults(string name)
        {
            List<Result> userMovie = await _movieDal.GetSearch(name);
            return View(userMovie);
        }
        public async Task<IActionResult> AddToFavoritesAsync (int id)
        {
            Result foundMovie = await _movieDal.GetMovie(id);

            if (foundMovie != null)
            {
                Favorite f = new Favorite();
                //f.Id = foundMovie.id;
                f.Title = foundMovie.title;
                f.PosterPath = foundMovie.poster_path;
                f.ReleaseDate = foundMovie.release_date;
                _context.Favorite.Add(f);

                _context.SaveChanges();
            }

            return RedirectToAction("DisplayFavorites");
        }
        public IActionResult DisplayFavorites()
        {
            var favList = _context.Favorite.ToList();

            return View(favList);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
