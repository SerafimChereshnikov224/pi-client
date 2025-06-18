
using pi_client.Models;
using pi_client.Util;
using pi_client.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace pi_client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public ObservableCollection<ProcessBase> Processes { get; } = new ObservableCollection<ProcessBase>();
        //public ObservableCollection<ChannelModel> Channels { get; } = new ObservableCollection<ChannelModel>();
        //public ObservableCollection<ProcessConnection> Connections { get; } = new ObservableCollection<ProcessConnection>;

        //public ICommand AddSendProcessCommand { get; }
        //public ICommand AddReceiveProcessCommand { get; }

        //public MainViewModel()
        //{
        //    AddSendProcessCommand = new RelayCommand(AddSendProcess);
        //    AddReceiveProcessCommand = new RelayCommand(AddReceiveProcess);
        //}

        //private void AddSendProcess()
        //{
        //    Processes.Add(new SendProcess
        //    {
        //        Position = new Point(100, 100)
        //    });
        //}

        //private void AddReceiveProcess()
        //{
        //    Processes.Add(new ReceiveProcess
        //    {
        //        Position = new Point(300, 100)
        //    });
        //}
        //private ProcessBase _draggedItem;
        //private Point _startPoint;

        //public ICommand ProcessMouseDownCommand => new RelayCommand<MouseButtonEventArgs>(OnProcessMouseDown);
        //public ICommand ProcessMouseMoveCommand => new RelayCommand<MouseEventArgs>(OnProcessMouseMove);
        //public ICommand ProcessMouseUpCommand => new RelayCommand<MouseButtonEventArgs>(OnProcessMouseUp);

        //private void OnProcessMouseDown(MouseButtonEventArgs e)
        //{
        //    if (e.Source is FrameworkElement element && element.DataContext is ProcessBase process)
        //    {
        //        _draggedItem = process;
        //        _startPoint = e.GetPosition(null);
        //        element.CaptureMouse();
        //    }
        //}

        //private void OnProcessMouseMove(MouseEventArgs e)
        //{
        //    if (_draggedItem != null && e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        var currentPoint = e.GetPosition(null);
        //        var offset = currentPoint - _startPoint;

        //        _draggedItem.Position = new Point(
        //            _draggedItem.Position.X + offset.X,
        //            _draggedItem.Position.Y + offset.Y);

        //        _startPoint = currentPoint;
        //    }
        //}

        //private void OnProcessMouseUp(MouseButtonEventArgs e)
        //{
        //    _draggedItem = null;
        //    if (e.Source is FrameworkElement element)
        //    {
        //        element.ReleaseMouseCapture();
        //    }
        //}

        //private ProcessBase _selectedProcess;

        //public ICommand SelectProcessCommand => new RelayCommand<ProcessBase>(p => {
        //    if (_selectedProcess == null)
        //    {
        //        _selectedProcess = p;
        //    }
        //    else
        //    {
        //        Connections.Add(new ProcessConnection
        //        {
        //            Source = _selectedProcess,
        //            Target = p,
        //            ChannelName = "default"
        //        });
        //        _selectedProcess = null;
        //    }
        //});

        private int _processCount;
        private Point GetNextPosition()
        {
            int col = _processCount % 2;  
            int row = _processCount / 2;  

            var point = new Point(
                100 + col * 300,  
                50 + row * 100    
            );

            _processCount++;
            return point;
        }

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiUrl = "http://localhost:63878";

        public ObservableCollection<ProcessBase> Processes { get; } = new ObservableCollection<ProcessBase>();
        public ObservableCollection<ChannelModel> Channels { get; } = new ObservableCollection<ChannelModel>();

        public ICommand AddSendCommand { get; }
        public ICommand AddReceiveCommand { get; }
        public ICommand ExecuteCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand SelectProcessCommand { get; }
        public ICommand ResetCommand { get; }

        private ProcessBase _firstSelectedProcess;
        private ProcessBase _secondSelectedProcess;

        public MainViewModel()
        {
            AddSendCommand = new RelayCommand(AddSendProcess);
            AddReceiveCommand = new RelayCommand(AddReceiveProcess);
            ExecuteCommand = new RelayCommand(ExecuteProcesses);
            ConnectCommand = new RelayCommand(CreateChannel);
            SelectProcessCommand = new RelayCommand<ProcessBase>(SelectProcess);
            ResetCommand = new RelayCommand(Reset);
            SendJsonCommand = new RelayCommand(SendJsonToServer);
            JsonInput = "{" +
                " }";
        }

        private async void SendJsonToServer()
        {
            var jsonInputWindow = new JsonInputWindow();
            if (jsonInputWindow.ShowDialog() == true)
            {
                try
                {
                    // Проверка JSON
                    if (string.IsNullOrWhiteSpace(jsonInputWindow.JsonInput))
                    {
                        MessageBox.Show("Please enter valid JSON");
                        return;
                    }

                    // 1. Отправляем parallel запрос
                    var parallelContent = new StringContent(jsonInputWindow.JsonInput, Encoding.UTF8, "application/json");
                    var parallelResponse = await _httpClient.PostAsync($"{_apiUrl}/api/builder/parallel", parallelContent);
                    parallelResponse.EnsureSuccessStatusCode();

                    // 2. Выполняем процесс
                    var executeResponse = await _httpClient.PostAsync($"{_apiUrl}/api/builder/execute", null);
                    executeResponse.EnsureSuccessStatusCode();

                    // 3. Получаем результаты
                    var result = await executeResponse.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<ProcessResponse>(result);

                    // Показываем результаты
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        new ExecutionResultWindow(
                            diagram: response?.Diagram ?? "No diagram",
                            messages: response?.Messages ?? new List<string> { "No messages" },
                            result: response?.Result ?? "Execution completed"
                        ).ShowDialog();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void Reset()
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите сбросить все процессы и соединения?",
                "Подтверждение сброса",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Processes.Clear();
                Channels.Clear();
            }
        }

        private void AddSendProcess()
        {
            var dialog = new SendProcessDialog();
            if (dialog.ShowDialog() == true)
            {
                CreateSendProcessBackend(dialog.Channel, dialog.Message);
            }
        }

        private async void CreateSendProcessBackend(string channel, string message)
        {
            try
            {
                var request = new
                {
                    Channel = channel,
                    Message = message,
                    Continuation = (object)null
                };

                var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/api/builder/send", request);
                if (response.IsSuccessStatusCode)
                {
                    var process = new SendProcess
                    {
                        Channel = channel,
                        Message = message,
                        Position = GetNextPosition()
                    };
                    Application.Current.Dispatcher.Invoke(() => Processes.Add(process));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error creating send process: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddReceiveProcess()
        {
            var dialog = new ReceiveProcessDialog();
            if (dialog.ShowDialog() == true)
            {
                CreateReceiveProcessBackend(dialog.Channel, dialog.Filter, dialog.WaitForMessage);
            }
        }

        private async void CreateReceiveProcessBackend(string channel, string filter, bool waitForMessage)
        {
            try
            {
                var request = new
                {
                    Channel = channel,
                    Filter = filter,
                    WaitForMessage = waitForMessage
                };

                var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/api/builder/receive", request);
                if (response.IsSuccessStatusCode)
                {
                    var process = new ReceiveProcess
                    {
                        Channel = channel,
                        Filter = filter,
                        Position = GetNextPosition()
                    };
                    Application.Current.Dispatcher.Invoke(() => Processes.Add(process));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error creating receive process: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SelectProcess(ProcessBase process)
        {
            if (_firstSelectedProcess == null)
            {
                _firstSelectedProcess = process;
                process.IsActive = true;
            }
            else if (_secondSelectedProcess == null && _firstSelectedProcess != process)
            {
                _secondSelectedProcess = process;
                process.IsActive = true;
            }
            else
            {
                if (_firstSelectedProcess != null) _firstSelectedProcess.IsActive = false;
                if (_secondSelectedProcess != null) _secondSelectedProcess.IsActive = false;
                _firstSelectedProcess = process;
                _secondSelectedProcess = null;
                process.IsActive = true;
            }
        }

        private void CreateChannel()
        {
            if (_firstSelectedProcess != null && _secondSelectedProcess != null)
            {
                var channel = new ChannelModel
                {
                    Name = "channel1",
                    Source = _firstSelectedProcess,
                    Target = _secondSelectedProcess
                };
                Channels.Add(channel);

                _firstSelectedProcess.IsActive = false;
                _secondSelectedProcess.IsActive = false;
                _firstSelectedProcess = _secondSelectedProcess = null;
            }
        }

        private bool _isExecuting;
        public ObservableCollection<ChannelModel> ActiveChannels { get; } = new ObservableCollection<ChannelModel>();

        private async void ExecuteProcesses()
        {
            if (_isExecuting) return;
            try
            {
                _isExecuting = true;
                ActiveChannels.Clear();

                foreach (var channel in Channels)
                {
                    channel.IsActive = true;
                    ActiveChannels.Add(channel);
                }

                var animationTask = AnimateExecution();

                var response = await _httpClient.PostAsync($"{_apiUrl}/api/builder/execute", null);

                await animationTask;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var processResponse = JsonSerializer.Deserialize<ProcessResponse>(content);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Создаем окно напрямую с данными
                        new ExecutionResultWindow(
                            diagram: processResponse?.Diagram,
                            messages: processResponse?.Messages,
                            result: processResponse?.Result
                        ).ShowDialog();
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Execution error: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                _isExecuting = false;
                ActiveChannels.Clear();
                foreach (var channel in Channels) channel.IsActive = false;
            }
        }

        private async Task AnimateExecution()
        {
            double progress = 0;
            while (progress < 1 && _isExecuting)
            {
                progress += 0.005;
                foreach (var channel in ActiveChannels)
                {
                    channel.BallPosition = new Point(
                        channel.Source.Position.X + (channel.Target.Position.X - channel.Source.Position.X) * progress,
                        channel.Source.Position.Y + (channel.Target.Position.Y - channel.Source.Position.Y) * progress);
                }
                await Task.Delay(50);
            }
        }

        private string _jsonInput;
        public string JsonInput
        {
            get => _jsonInput;
            set
            {
                if (_jsonInput == value) return;
                _jsonInput = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendJsonCommand { get; }


        // Модель для десериализации ответа
        public class ProcessResponse
        {
            [JsonPropertyName("diagram")]
            public string Diagram { get; set; }

            [JsonPropertyName("messages")]
            public List<string> Messages { get; set; }

            [JsonPropertyName("result")]
            public string Result { get; set; }
        }

    }
}
