using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using _05.EF_Code_First_Movies;
using _05.EF_Code_First_Movies.Models;

namespace _06.Query_The_Database
{
    class QueryTheDatabase
    {
        static void Main()
        {
            var context = new MoviesContext();

            // 1.	Adult Movies

            var adultMovies = context.Movies
                .Where(m => m.AgeRestriction == AgeRestriction.Adult)
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Ratings.Count)
                .Select(m => new
                {
                    title = m.Title,
                    ratingsGiven = m.Ratings.Count
                });

            var json = JsonConvert.SerializeObject(adultMovies, Formatting.Indented);
            File.WriteAllText("../../adult-movies.json", json);

            //2.	Rated Movies by User

            var jmeyeryMovies =
                context.Movies.SqlQuery(
                    "SELECT m.Title [title], r.Stars [userRating], (SELECT ROUND(AVG(CAST(Stars AS FLOAT)), 2) FROM Ratings rr INNER JOIN Movies mm ON mm.Id = rr.MovieId WHERE m.Title = mm.Title) [averageRating] FROM Movies m INNER JOIN Ratings r ON r.MovieId = m.Id Where r.UserId = (SELECT Id From Users Where Username = 'jmeyery') GROUP BY r.Stars, m.Title");

            // JSON output in rated-movies-by-jmeyery.json
            

            //3.	Top 10 Favourite Movies

            var favouriteTeenMovies = context.Movies
                .Where(m => m.AgeRestriction == AgeRestriction.Teen)
                .OrderByDescending(m => m.Users.Count)
                .ThenBy(m => m.Title)
                .Select(m => new
                {
                    isbn = m.Isbn,
                    title = m.Title,
                    favouritedBy = m.Users
                                .Select(u => u.Username)
                }).Take(10);

            var json3 = JsonConvert.SerializeObject(favouriteTeenMovies, Formatting.Indented);
            File.WriteAllText("../../top-10-favourite-movies.json", json3);
        }
    }
}
