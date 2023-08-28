using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lFtpUpload
{
    internal class FtpEntity
    {
        public string Server { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public string RemoteFilePath { get; set; }
        public string LocalFilePath { get; set; }
    }
}
