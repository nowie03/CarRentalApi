using CarRentalApi.Data;
using CarRentalApi.Models;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.DAL
{
    public class BookingRepository : IBooking
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context) { _context = context; }


        public async Task<Booking?> CreateBooking(int userId, int carId, string startDate, string endDate)
        {
            try
            {
                User? user=await _context.Users.FindAsync(userId) ?? throw new Exception("Owner not found");

                Car? car=await _context.Cars.FindAsync(carId) ?? throw new Exception("Car not found");


                List<Booking> bookingAlreadyExists = _context.Bookings
                                                     .Include(booking => booking.BookedCar)
                                                     .Where(booking => booking.EndDate > DateTime.Now && booking.BookedCar.Id == carId)
                                                     .ToList();

                if (bookingAlreadyExists.Count>0)
                {
                    throw new Exception("Car is already booked");
                }

                Booking booking = new()
                {
                    BookedBy = user,
                    BookedCar = car,
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                    CreatedAt = DateTime.Now,
                };

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                return booking;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message+" "+ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Booking?> DeleteBooking(int id)
        {
            try
            {
                Booking? bookking = await _context.Bookings.Include(booking => booking.BookedBy)
                     .Include(booking => booking.BookedCar)
                     .FirstOrDefaultAsync(booking => booking.Id == id);
               if (bookking != null)
                {
                    _context.Bookings.Remove(bookking);
                    await _context.SaveChangesAsync();
                    return bookking;
                    
                }

                throw new Exception("booking not found");

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<Booking>? GetBookings()
        {
            
            return _context.Bookings.Include(booking=>booking.BookedCar)
                .Include(booking=>booking.BookedCar)
                .ToList();
        }

        public List<Booking>? GetBookings(int userId)
        {
            try
            {
                return _context.Bookings.Include(booking => booking.BookedBy)
                    .Include(booking => booking.BookedCar)
                    .Where(booking =>booking.BookedBy.ID== userId).ToList();
            }
            catch(Exception ex) { 
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
