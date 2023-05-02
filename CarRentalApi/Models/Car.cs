using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required] 
        public string Model { get; set; }

        [Required]
        public string RegNumber { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime Year { get; set; }

        public int Rating { get; set; }

        [Required]
        public double PricePerKm { get; set; }

        public string ImgUrl { get; set; }

        [Required]
        public User Owner { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public DateTime CreatedAt { get; set; }


       
    }
}
