using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.Models
{
    public enum ProcessType
    {
        Send,
        Receive,
        Parallel,
        Replication,
        NewChannel
    }
}
