using BobaFileManager.Models;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public interface IUserFileService
    {
        Task Add(UserFile file);
        Task Update(UserFile file);
    }
}