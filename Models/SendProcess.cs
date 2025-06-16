using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class SendProcess : ProcessBase
    {
        public override ProcessType Type => ProcessType.Send;
        public override string Name => "Send";
        public string Channel { get; set; }
        public string Message { get; set; }
    }
}
