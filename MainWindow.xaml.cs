using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace PowBotLauncher
{
    public class MainWindow : Window
    {
        private TextBlock _statusBlock;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _statusBlock = this.FindControl<TextBlock>("StatusBlock");
        }

        protected override void OnOpened(EventArgs e)
        {
            SetStatus("Preparing to launch the client...");
            Task.Run(() =>
            {
                HomeFolder.Create(SetStatus);

                var jreBinary = JRE.GetOrObtainJREBinary(SetStatus);
                var client = Client.EnsureLatestClient(SetStatus);
                Shell.Execute(jreBinary, Client.GetDirectory(), true, new[] {"-jar", client});
                Environment.Exit(0);
            });
        }

        public void SetStatus(string status)
        {
            Dispatcher.UIThread.Post(() => _statusBlock.Text = status);
        }
    }
}