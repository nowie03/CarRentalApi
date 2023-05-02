using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User BookedBy { get; set; }

        [Required]
        public Car BookedCar { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }







    }
}
