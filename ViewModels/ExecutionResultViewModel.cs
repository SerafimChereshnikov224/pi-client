using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pi_client.ViewModels
{
    public class ExecutionResultViewModel : INotifyPropertyChanged
    {
        public string Diagram { get; set; }
        public string Result { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
