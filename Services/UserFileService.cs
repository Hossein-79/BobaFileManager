﻿using BobaFileManager.Data;
using BobaFileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly BobaContext _context;

        public UserFileService(BobaContext paperContext)
        {
            _context = paperContext;
        }

        public async Task Add(UserFile file)
        {
            await _context.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task Update(UserFile file)
        {
            _context.Update(file);
            await _context.SaveChangesAsync();
        }
    }
}
