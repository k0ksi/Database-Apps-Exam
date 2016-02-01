using System.Data.Entity;
using _05.EF_Code_First_Movies.Migrations;
using _05.EF_Code_First_Movies.Models;

namespace _05.EF_Code_First_Movies
{
    public class MoviesContext : DbContext
    {
        public MoviesContext()
            : base("name=MoviesContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MoviesContext, Configuration>());
        }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Rating> Ratings { get; set; }
    }
}