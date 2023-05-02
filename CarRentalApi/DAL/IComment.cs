using CarRentalApi.Models;

namespace CarRentalApi.DAL
{
    public interface IComment
    {
        public Task<Comment?> CreateComment(int userId, int carId, string message);

        public List<Comment>? Comments();

        public List<Comment>? Comments(int carId);




    }
}
