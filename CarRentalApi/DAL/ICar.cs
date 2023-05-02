using CarRentalApi.Models;

namespace CarRentalApi.DAL
{
    public interface ICar
    {
        public Task<Car?> CreateCar(string make,string model,string regNumber, string state, string city, string district, string year, int OwnerId,string imgUrl,int rating = 0, double pricePerKm = 21.0);

        public Task<Car?> RemoveCar(int id);

        public Task<Car?> Car(int id);

        public List<Car>? Cars(bool isBooked=false);

      


       
    }
}
