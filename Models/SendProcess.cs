using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public class SendProcess : ProcessBase
    {
        public SendProcess()
        {
            Type = ProcessType.Send;
            Name = "Send";
        }

        public string Channel { get; set; } = "default";
        public string Message { get; set; } = "Hello";
    }
}
