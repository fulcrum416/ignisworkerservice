using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnisWorkerService.Utilities
{
    public class MQTTDefaults
    {
        public string? TCPServer { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ClientId { get; set; }
        public int Port { get; set; }
    }
}
