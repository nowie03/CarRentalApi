using CarRentalApi.Data;
using CarRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.DAL
{
    public class CommentRepository:IComment
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Comment>? Comments()
        {
            try { 
                return _context.Comments.ToList();
              }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Comment>? Comments(int carId)
        {
            try
            {
                return _context.Comments.Include(comment=>comment.ForCar).Where(comment => comment.ForCar.Id == carId).ToList();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Comment?> CreateComment(int userId, int carId, string message)
        {
            try
            {
                User? user=await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    throw new Exception("User not Found");
                }

                Car? car= await _context.Cars.FindAsync(carId); 
                    
                if(car == null)
                {
                    throw new Exception("car not found");
                }

                Comment comment = new()
                {
                    FromUser = user,
                    ForCar = car,
                    Message = message,
                    CreatedAt = DateTime.Now,
                };
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to add comment");
                throw new Exception(ex.Message);
            }
        }

    }
}
