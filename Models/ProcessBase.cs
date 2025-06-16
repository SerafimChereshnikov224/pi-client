using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pi_client.Models
{
    public abstract class ProcessBase
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public ProcessType Type { get; protected set; }
        public Point Position { get; set; }
        public string Name { get; set; }
    }
}
