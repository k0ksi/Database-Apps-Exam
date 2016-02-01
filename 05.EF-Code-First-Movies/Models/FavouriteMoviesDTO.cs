using System.Collections;

namespace _05.EF_Code_First_Movies.Models
{
    class FavouriteMoviesDTO
    {
        public string Username { get; set; }

        public string[] FavouriteMovies { get; set; }
    }
}
