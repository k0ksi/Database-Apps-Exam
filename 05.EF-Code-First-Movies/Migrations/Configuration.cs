using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using _05.EF_Code_First_Movies.Models;

namespace _05.EF_Code_First_Movies.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MoviesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MoviesContext context)
        {
            if (!context.Countries.Any())
            {
                var file = File.ReadAllText("../../data/countries.json");
                var countries = JsonConvert.DeserializeObject<Country[]>(file);

                foreach (var country in countries)
                {
                    Country countryObj = new Country()
                    {
                        Name = country.Name
                    };
                    context.Countries.Add(countryObj);
                }

                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                var file = File.ReadAllText("../../data/movies.json");
                var movies = JsonConvert.DeserializeObject<Movie[]>(file);

                foreach (var movie in movies)
                {
                    Movie movieObj = new Movie()
                    {
                        Isbn = movie.Isbn,
                        Title = movie.Title,
                        AgeRestriction = (AgeRestriction) movie.AgeRestriction
                    };
                    context.Movies.Add(movieObj);
                }

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var file = File.ReadAllText("../../data/users.json");
                var users = JsonConvert.DeserializeObject<UserDTO[]>(file);

                foreach (var user in users)
                {
                    Country country = null;
                    if (user.Country != null)
                    {
                        country = context.Countries
                            .FirstOrDefault(c => c.Name == user.Country);
                    }
                    User userObj = new User()
                    {
                        Username = user.Username,
                        Age = user.Age,
                        Email = user.Email,
                        Country = country
                    };
                    context.Users.Add(userObj);
                }

                context.SaveChanges();
            }

            if (!context.Ratings.Any())
            {
                var file = File.ReadAllText("../../data/movie-ratings.json");
                var movieRatings = JsonConvert.DeserializeObject<MovieRatingsDTO[]>(file);

                foreach (var rating in movieRatings)
                {
                    Movie movie = context.Movies
                        .FirstOrDefault(m => m.Isbn == rating.Movie);

                    User user = context.Users
                        .FirstOrDefault(u => u.Username == rating.User);

                    Rating ratingObj = new Rating()
                    {
                        User = user,
                        Movie = movie,
                        Stars = rating.Rating
                    };

                    context.Ratings.Add(ratingObj);
                }

                context.SaveChanges();
            }
            
            var favMoviesFile = File.ReadAllText("../../data/users-and-favourite-movies.json");
            var favMovies = JsonConvert.DeserializeObject<FavouriteMoviesDTO[]>(favMoviesFile);

            foreach (var favMovie in favMovies)
            {
                User user = context.Users
                    .FirstOrDefault(u => u.Username == favMovie.Username);
                foreach (var favouriteMovie in favMovie.FavouriteMovies)
                {
                    Movie movie = context.Movies
                   .FirstOrDefault(m => m.Isbn == favouriteMovie);
                    if (user != null)
                    {
                        if (movie != null)
                        {
                            user.Movies.Add(movie);
                        }
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
