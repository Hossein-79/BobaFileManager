using BobaFileManager.Models;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public interface IUserService
    {
        Task<User> GetUser(int useId);
        Task<User> GetUser(string address);
        Task TryAdd(User user);
    }
}