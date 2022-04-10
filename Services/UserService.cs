using BobaFileManager.Data;
using BobaFileManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public class UserService : IUserService
    {
        private readonly BobaContext _context;

        public UserService(BobaContext paperContext)
        {
            _context = paperContext;
        }

        public async Task TryAdd(User user)
        {
            if ((await GetUser(user.Address)) is null)
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUser(string address) =>
            await _context.Users.Where(u => u.Address == address).FirstOrDefaultAsync();

        public async Task<User> GetUser(int useId) =>
            await _context.Users.Where(u => u.UserId == useId).FirstOrDefaultAsync();
    }
}
