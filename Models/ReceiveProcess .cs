using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class ReceiveProcess : ProcessBase
    {
        public ReceiveProcess()
        {
            Type = ProcessType.Receive;
            Name = "Receive";
        }

        public string Channel { get; set; } = "default";
        public string Filter { get; set; }
    }
}
