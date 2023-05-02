using System.ComponentModel.DataAnnotations;

namespace CarRentalApi.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Car ForCar { get; set; }

        [Required]
        public User FromUser { get; set; }
        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }



    }
}
