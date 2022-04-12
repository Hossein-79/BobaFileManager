using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Models
{
    public class UserFile
    {
        public int UserFileId { get; set; }

        public int UserId { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public long Length { get; set; }

        public string LocalName { get; set; }

        public string ArweaveUrl { get; set; }

        public bool IsUploaded { get; set; }

        public DateTime UploadTime { get; set; }

        public User User { get; set; }
    }
}
