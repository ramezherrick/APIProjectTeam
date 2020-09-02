using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Models
{
    public class FavoriteMovies
    {
        List<Result> favoritesList = new List<Result>();

        public FavoriteMovies()
        { 
            
        }
        public List<Result> AddToFavorites(Result movieInfo)
        {
            favoritesList.Add(movieInfo);

            return favoritesList;
        }
    }
}
