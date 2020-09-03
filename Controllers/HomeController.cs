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

        //For hiding APIkey
        private readonly string _apiKey;

        public HomeController(IConfiguration configuration, FavoriteDbContext context)
        {
            //For hiding APIkey
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
            return View();
        }

        //Grabbing movies from the API and passing it to the view
        public async Task<IActionResult> SearchResults(string name)
        {
            List<Result> userMovie = await _movieDal.GetSearch(name);
            return View(userMovie);
        }

        //Gets a movie from the API and saves it to SQL
        public async Task<IActionResult> AddToFavoritesAsync (int id)
        {
            Result foundMovie = await _movieDal.GetMovie(id);

            if (foundMovie != null)
            {
                Favorite f = new Favorite();
               
                f.Title = foundMovie.title;
                f.PosterPath = foundMovie.poster_path;
                f.ReleaseDate = foundMovie.release_date;
                f.UserId = FindUserId();
                _context.Favorite.Add(f);

                _context.SaveChanges();
            }

            return RedirectToAction("DisplayFavorites");
        }

        //Deletes a movie from a user's SQL database
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
  
        //Displays a user's favorites
        public IActionResult DisplayFavorites()
        {
            string id = FindUserId();

            //Creates list of favorites for the current user
            var favList = _context.Favorite.Where(x => x.UserId == id).ToList();

            return View(favList);
        }

        //this is a method to make stephen happy
        public string FindUserId()
        {
            if (User.Identity.Name == null)
            {
                return null;
            }
            else
            {
                return _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
            }
            
        }

        //Grabs a video from Youtube to show to the user
        public async Task<IActionResult> MovieDetailsAsync(int id)
        {

           Videoobject video = await _movieDal.GetVideo(id);

            return View(video);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
