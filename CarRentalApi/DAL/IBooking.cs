using CarRentalApi.Models;

namespace CarRentalApi.DAL
{
    public interface IBooking
    {
        public Task<Booking?> CreateBooking(int userId, int carId, string startDate, string endDate);

        public Task<Booking?> DeleteBooking(int id);

        public List<Booking>? GetBookings();

        public List<Booking>? GetBookings(int userId);





    }

}
