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
        public async Task<Movieobject> GetMovie()
        {
            var client = GetClient();//give api general information to receive data from api
            var response = await client.GetAsync($"/3/movie/550?api_key={_apikey}"); //use client (Httpclient) to receive data from api based of certain endpoint

            //install-package Microsoft.AspNet.WebAPI.Client
            //response has a property (content), content has a method that reads Json and plug it in specific object
            //internal serialization error if it doesnt fit
            Movieobject movie = await response.Content.ReadAsAsync<Movieobject>();
            return movie;
        }
    }
}
