using CarRentalApi.Models;

namespace CarRentalApi.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("Booking Created")]
        public Booking? OnBookingCreate(int userId, [EventMessage]Booking booking) {

            return booking.BookedBy.ID == userId ? booking : null;
        }

        [Subscribe]
        [Topic("Booking Deleted")]
        public Booking? OnBookingDelete(int userId, [EventMessage]Booking booking)
        {
            return booking.BookedBy.ID == userId ? booking : null;

        }

        [Subscribe]
        [Topic("Car Created")]
        public Car? OnCarCreate(int userId, [EventMessage]Car car)
        {
            return car.Owner.ID == userId ? car : null;
        }

        [Subscribe]
        [Topic("Car Deleted")]
        public Car? OnCarDelete(int userId, [EventMessage]Car car) { 
            return car.Owner.ID==userId?car:null;
        }

        [Subscribe]
        [Topic("Comment Created")]
        public Comment? OnCommentCreated(int carId, [EventMessage]Comment comment)
        {
            return comment.ForCar.Id == carId ? comment : null;
        }

       

    }
}

