using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pi_client.Views
{
    public partial class ReceiveProcessDialog : Window
    {
        public string Channel => ChannelTextBox.Text;
        public string Filter => FilterTextBox.Text;
        public bool WaitForMessage => WaitCheckBox.IsChecked ?? true;

        public ReceiveProcessDialog()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
