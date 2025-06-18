using System.Windows;

namespace pi_client.Views
{
    public partial class JsonInputWindow : Window
    {
        public string JsonInput { get; private set; }

        public JsonInputWindow()
        {
            InitializeComponent();
            // Пример JSON по умолчанию
            JsonTextBox.Text = @"{
    ""processes"": [
        {
            ""type"": ""send"",
            ""channel"": ""test1"",
            ""message"": ""hello""
        },
        {
            ""type"": ""receive"",
            ""channel"": ""test2"",
            ""filter"": """"
        }
    ]
}";
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            JsonInput = JsonTextBox.Text;
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
