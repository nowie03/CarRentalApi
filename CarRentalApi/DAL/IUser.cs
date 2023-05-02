using CarRentalApi.Models;

namespace CarRentalApi.DAL
{
    public interface IUser
    {
        public List<User>? GetUsers();

        public Task<User?> GetUser(int id);

        public Task<User?> GetUser(string email);

        public Task<User?> CreateUser(string email, string phoneNumber, string password, string imageUrl = "");

        public Task<List<Car>?> CarsOwnedBy(int userId);

   

    }
}
