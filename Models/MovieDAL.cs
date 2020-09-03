using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIProject.Models
{
    public class MovieDAL
    {
        //for hiding api key
        private readonly string _apikey;

        //for hiding api key
        public MovieDAL(string apikey)
        {
            _apikey = apikey;
        }

        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(" https://api.themoviedb.org");
            client.DefaultRequestHeaders.Add("x-api-key", _apikey);
            return client;
        }
        public async Task<Result> GetMovie(int id)
        {
            var client = GetClient();//give api general information to receive data from api
            var response = await client.GetAsync($"/3/movie/{id}?api_key={_apikey}"); //use client (Httpclient) to receive data from api based of certain endpoint

            //install-package Microsoft.AspNet.WebAPI.Client
            //response has a property (content), content has a method that reads Json and plug it in specific object
            //internal serialization error if it doesnt fit
            Result movie = await response.Content.ReadAsAsync<Result>();

            return movie;
        }

        public async Task<List<Result>> GetSearch(string movieName)
        {
            var client = GetClient();//give api general information to receive data from api
            var response = await client.GetAsync($"/3/search/movie?api_key={_apikey}&language=en-US&page=1&include_adult=false&query={movieName}"); //use client (Httpclient) to receive data from api based of certain endpoint
            var JSON = await response.Content.ReadAsStringAsync();
            //install-package Microsoft.AspNet.WebAPI.Client
            //response has a property (content), content has a method that reads Json and plug it in specific object
            //internal serialization error if it doesnt fit

            Rootobject movie = await response.Content.ReadAsAsync<Rootobject>();

            List<Result> movieResult = new List<Result>();

            for (int i = 0; i < movie.results.Length; i++)
            { 
                movieResult.Add(movie.results[i]);
            }

            return movieResult;
        }
        public async Task<Video> GetVideo(int id)
        {
            var client = GetClient();//give api general information to receive data from api
            var response = await client.GetAsync($"/3/movie/{id}/videos?api_key={_apikey}"); //use client (Httpclient) to receive data from api based of certain endpoint
            var JSON = await response.Content.ReadAsStringAsync();

            Video video= await response.Content.ReadAsAsync<Video>();

            return video;
        }
    }
}
