using CarRentalApi.Data;
using CarRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.DAL
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }


        public async Task<List<Car>?> CarsOwnedBy(int userId)
        {
            try
            {
                User? user = await _context.Users.FindAsync(userId);

                if (user == null) {
                    throw new Exception("Car Owned by cannot be found");
                }

                return user.OwnedCars.ToList();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public async Task<User?> CreateUser(string email, string phoneNumber, string password, string imageUrl = "")
        {
            try
            {
                User user = new()
                {
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    ImgUrl = imageUrl,
                    CreatedAt = DateTime.Now
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User?> GetUser(int id)
        {
            try
            {
               User? user=await _context.Users.FindAsync(id) ?? throw new Exception("User Not Found");
                return user;

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User?> GetUser(string email)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(user=>user.Email.Equals(email));

                return user ?? throw new Exception("User Not Found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<User>? GetUsers()
        {
            try
            {

                var users =_context.Users.Where(user=>user.Email.Contains("@"));

                return users.ToList() ?? throw new Exception("Cannot get all users");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


    }
}
