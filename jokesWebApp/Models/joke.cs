using System.ComponentModel.DataAnnotations;

namespace jokesWebApp.Models
{
    public class joke
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        public string JokeQuestion { get; set; }
        [Required]
        [MinLength(5)]
        public string JokeAnswer { get; set; }
        public joke()
        {
            
        }
    }
}
