using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string ImgUrl { get; set; }

        public DateTime CreatedAt { get; set; } 

        public ICollection<Car> OwnedCars { get; set; }

        public ICollection<Comment> Comments { get; set; }
        
        public ICollection<Booking> Bookings { get; set; }



    }
}
