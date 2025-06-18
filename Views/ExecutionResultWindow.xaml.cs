using pi_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pi_client.Views
{
    public partial class ExecutionResultWindow : Window
    {
        public ExecutionResultWindow(string diagram, IEnumerable<string> messages, string result)
        {
            InitializeComponent();
            DiagramTextBox.Text = diagram ?? "Диаграмма не доступна";
            MessagesList.ItemsSource = new List<string> { "Сообщение успешно доставлено" };
            ResultText.Text = result ?? "Выполнение завершено";
        }
    }
}
