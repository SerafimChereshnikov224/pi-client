using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class ChannelModel
    {
        public string Name { get; set; }
        public List<string> Messages { get; } = new List<string>();
    }
}
