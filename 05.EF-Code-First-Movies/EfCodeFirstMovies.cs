using System;
using System.Linq;
namespace _05.EF_Code_First_Movies
{
    class EfCodeFirstMovies
    {
        static void Main()
        {
            var context = new MoviesContext();

            var moviesCount = context.Movies.Count();
            Console.WriteLine(moviesCount);
        }
    }
}
