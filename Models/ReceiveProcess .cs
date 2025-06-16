using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class ReceiveProcess : ProcessBase
    {
        public override ProcessType Type => ProcessType.Receive;
        public override string Name => "Receive";
        public string Channel { get; set; }
        public string Filter { get; set; }
    }
}
