using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

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
                f.UserId = FindUserId();
                _context.Favorite.Add(f);

                _context.SaveChanges();
            }

            return RedirectToAction("DisplayFavorites");
        }
        public IActionResult DeleteFavorite(int id)
        {
            var foundMovie = _context.Favorite.Find(id);

            if (foundMovie != null)
            {
                _context.Favorite.Remove(foundMovie);
                _context.SaveChanges();
            }

            return RedirectToAction("DisplayFavorites");
        }
        [Authorize]
        public IActionResult DisplayFavorites()
        {
            string id = FindUserId();

            //put current users favorite 
            var favList = _context.Favorite.Where(x => x.UserId == id).ToList();


            return View(favList);
        }
        //this is a method to make stephen happy
        public string FindUserId()
        {
            return _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
        }
        public async Task<IActionResult> MovieDetailsAsync(int id)
        {
           // Result foundMovie = await _movieDal.GetMovie(id);

            Video video = await _movieDal.GetVideo(id);

            return View(video);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
