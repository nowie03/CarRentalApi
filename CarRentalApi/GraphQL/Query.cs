using CarRentalApi.DAL;
using CarRentalApi.Models;

namespace CarRentalApi.GraphQL
{
    public class Query
    {
        public List<User>? Users([Service] UserRepository userRepository) => userRepository.GetUsers();

        public async Task<User?> User(string email, [Service] UserRepository userRepository)=>await userRepository.GetUser(email);

        public async Task<User?> User(int id, [Service] UserRepository userRepository) => await userRepository.GetUser(id);

        public List<Car>? Cars([Service] CarRepository carRepository)=> carRepository.Cars();

        public async Task<Car?> Car(int id, [Service] CarRepository carRepository) => await carRepository.Car(id);

        public List<Booking>? Bookings([Service] BookingRepository bookingRepository)=>bookingRepository.GetBookings();

        public List<Booking>? UserBookings(int userId,[Service] BookingRepository bookingRepository) => bookingRepository.GetBookings(userId);

        public List<Comment>? Comments([Service] CommentRepository commentRepository) => commentRepository.Comments();

        public List<Comment>? CarComments(int carId,[Service] CommentRepository commentRepository) => commentRepository.Comments(carId);










    }
}
