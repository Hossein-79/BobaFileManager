using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Address { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<UserFile> UserFiles { get; set; }
    }
}
