using CarRentalApi.DAL;
using CarRentalApi.Models;
using HotChocolate.Subscriptions;

namespace CarRentalApi.GraphQL
{
    public class Mutation
    {
        public async Task<User?> CreateUser(string email, string password, string phoneNumber, string imgUrl, [Service] UserRepository userRepository)
        {
            
            return await  userRepository.CreateUser(email, phoneNumber, password, imgUrl);
        }

        public async Task<Car?> Createcar(string make, string model, string regNumber, string state, string city, string district, string year, int OwnerId, string imgUrl,int kmsDriven,double pricePerKm,
            [Service] CarRepository carRepository, [Service]ITopicEventSender eventSender)
        {
            try
            {
               
                Car? car = await carRepository.CreateCar(make, model, regNumber, state, city, district, year, OwnerId, imgUrl, kmsDriven, pricePerKm: pricePerKm) ?? throw new Exception("user not found");
                await eventSender.SendAsync("Car Created", car);
                return car;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Car?> RemoveCar(int id, [Service]CarRepository carRepository, [Service]ITopicEventSender eventSender)
        {
           
            Car? car= await carRepository.RemoveCar(id);

            if(car != null) {
                await eventSender.SendAsync("Car Deleted", car);
            }
            return car;
        }

        public async Task<Booking?> CreateBooking(int userId, int carId, string startDate, string endDate, [Service] BookingRepository bookingRepository, [Service] ITopicEventSender eventSender)
        { 
            
             Booking? booking=await bookingRepository.CreateBooking(userId, carId, startDate, endDate);

            if(booking!=null)
            {
               await eventSender.SendAsync("Booking Created", booking);
            }
            return booking;
        }

        public async Task<Booking?> DeleteBooking(int id, [Service] BookingRepository bookingRepository, [Service] ITopicEventSender eventSender) { 

            Booking? booking=  await bookingRepository.DeleteBooking(id);

            if (booking != null)
            {
                await eventSender.SendAsync("Booking Deleted", booking);
            }
            return booking;
            

        }

        public async Task<Comment?> CreateComment(int userId, int carId, string message, [Service] CommentRepository commentRepository, [Service] ITopicEventSender eventSender)
        { 
            Comment? comment=await commentRepository.CreateComment(userId, carId, message); 

            if (comment!=null)
            {
                await eventSender.SendAsync("Comment Created", comment);
            }
            return comment;
        }



    }
}
