using CarRentalApi.Data;
using CarRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.DAL
{
    public class CarRepository : ICar
    {
        private readonly ApplicationDbContext _context;
        public CarRepository(ApplicationDbContext context) { 
            _context = context;
        }
        public List<Car>? Cars(bool isBooked = false)
        {
            if(isBooked) 
                return _context.Cars
                    .Include(car => car.Owner)
                    .Include(car => car.Bookings)
                    .Include(car => car.Comments).Where(car=>isBookingFree(car.Id)).ToList();

            return _context.Cars
                    .Include(car => car.Owner)
                    .Include(car => car.Bookings)
                    .Include(car => car.Comments).Where(car=>car.Id>0).ToList();

        }

        private bool isBookingFree(int carId)
        {
            return _context.Bookings.Include(booking=>booking.BookedCar).Any(booking=>booking.BookedCar.Id == carId && booking.EndDate< DateTime.Now) ;
        }
      


        public async Task<Car?> CreateCar(string make,string model,string regNumber, string state, string city, string district, string year, int OwnerId, string imgUrl, int rating = 0, double pricePerKm = 21)
        {
            try
            {
                User? user = await _context.Users.FindAsync(OwnerId);
                if(user == null)
                {
                    throw new Exception("Owner not found");
                }

                var car = new Car()
                {
                    RegNumber=regNumber,
                    State=state,
                    City=city,
                    District=district,
                    Year=DateTime.Parse(year),
                    Rating=rating,
                    PricePerKm=pricePerKm,
                    Make=make,
                    Model=model,
                    Owner=user,
                    ImgUrl=imgUrl
           
                };

                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();

                return car;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Car?> RemoveCar(int id)
        {
            try {
                Car? car =await _context.Cars.Include(car => car.Owner)
                    .Include(car => car.Bookings)
                    .Include(car => car.Comments)
                    .FirstAsync(car=>car.Id==id) ?? throw new Exception("car cannot be found");

                List<Booking> bookingsForCar = _context.Bookings.Include(booking => booking.BookedCar)
                    .Where(booking => booking.BookedCar.Id == id && booking.EndDate.CompareTo(DateTime.Now)<=0 ).ToList();

               // Console.WriteLine(bookingsForCar.Count + "-------");

                if (bookingsForCar.Count > 0) throw new Exception("Cannot delete a car that has a booking already");

               _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return car;

            }
            catch(Exception ex) { Console.WriteLine(ex.Message);return null; }
        }

        public async Task<Car?> Car(int id)
        {
            try
            {
                Car? car = await _context.Cars
                    .Include(car => car.Owner)
                    .Include(car => car.Bookings)
                    .Include(car => car.Comments).FirstAsync(car=>car.Id==id);

                if (car == null)
                {
                    throw new Exception("car not found");
                }

                return car;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }
    }
}
