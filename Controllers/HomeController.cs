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
        private readonly MovieDAL _movieDal;
        //for hiding apikey
        private readonly string _apiKey;

        public HomeController(IConfiguration configuration)
        {
            //for hiding api key
            _apiKey = configuration.GetSection("ApiKeys")["MovieAPIkey"];
            _movieDal = new MovieDAL(_apiKey);
        }

        public async Task<IActionResult> IndexAsync()
        {
            var movie = await _movieDal.GetMovie();
            return View(movie);
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
