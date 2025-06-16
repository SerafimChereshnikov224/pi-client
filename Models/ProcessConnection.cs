using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class ProcessConnection
    {
        public ProcessBase Source { get; set; }
        public ProcessBase Target { get; set; }
        public string ChannelName { get; set; }
    }
}
