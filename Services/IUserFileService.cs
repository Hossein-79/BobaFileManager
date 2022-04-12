using BobaFileManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public interface IUserFileService
    {
        Task Add(UserFile file);
        Task<IEnumerable<UserFile>> GetUserFiles(int userId);
        Task Update(UserFile file);
    }
}