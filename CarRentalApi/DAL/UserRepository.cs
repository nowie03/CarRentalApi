using CarRentalApi.Data;
using CarRentalApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Data;

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
                User? user = await _context.Users.Include(user => user.OwnedCars).FirstAsync(user => user.ID == userId);

                List<Car> ownedCars=user.OwnedCars.ToList();

                List<Car>? returnOwnedCars = new();

                foreach(var car in ownedCars)
                {
                    Car? carTemp = await _context.Cars.Include(car => car.Owner)
                    .Include(car => car.Bookings)
                    .Include(car => car.Comments).FirstAsync(c => c.Id == car.Id);

                    if (carTemp != null) 
                        returnOwnedCars.Add(carTemp);   
                }

                return user == null ? throw new Exception("Cars Owned by cannot be found") : returnOwnedCars;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);

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
                if (ex.InnerException != null)
                {
                    string result = ex.InnerException.Message;

                    int index = result.IndexOf("IX_Users_PhoneNumber");
                    if (index < 0)
                        throw new Exception("Email already exists");

                    throw new Exception("Phone number already exists");
                }
                throw new Exception("internal error");
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }

        }

        private string _generateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("your-secret-key-here-32-characters-long");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("userId", user.ID.ToString()),
                new Claim("email", user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? SignIn(string email, string password)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(user => user.Email.Equals(email)) ?? throw new Exception("user not found");

                if(user.Password.Equals(password))
                    return _generateToken(user);

                throw new Exception("password doesnt match");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GraphQLException(ex.Message).ToString();
            }
        }
    }
}
