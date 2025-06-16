using pi_client.Models;
using pi_client.Util;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace pi_client.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<ProcessBase> Processes { get; } = new ObservableCollection<ProcessBase>();
        public ObservableCollection<ChannelModel> Channels { get; } = new ObservableCollection<ChannelModel>();

        public ICommand AddSendProcessCommand { get; }
        public ICommand AddReceiveProcessCommand { get; }

        public MainViewModel()
        {
            AddSendProcessCommand = new RelayCommand(AddSendProcess);
            AddReceiveProcessCommand = new RelayCommand(AddReceiveProcess);
        }

        private void AddSendProcess()
        {
            Processes.Add(new SendProcess
            {
                Position = new Point(100, 100)
            });
        }

        private void AddReceiveProcess()
        {
            Processes.Add(new ReceiveProcess
            {
                Position = new Point(300, 100)
            });
        }
    }
}
