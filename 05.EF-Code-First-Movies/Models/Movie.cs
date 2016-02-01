using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _05.EF_Code_First_Movies.Models
{
    public class Movie
    {
        private ICollection<User> users;
        private ICollection<Rating> ratings; 

        public Movie()
        {
            this.users = new HashSet<User>();
            this.ratings = new HashSet<Rating>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Isbn { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public AgeRestriction AgeRestriction { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }
    }
}