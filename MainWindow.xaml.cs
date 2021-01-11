using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.VisualBasic;

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

                var jvmArgsPath = Path.Combine(HomeFolder.GetDirectory(), "jvmargs.txt");
                IEnumerable<string> jvmArgs = new[] {"-Xmx512M"};
                if (File.Exists(jvmArgsPath))
                {
                    jvmArgs = File.ReadAllText(jvmArgsPath).Trim().Split(" ");
                }
                else
                {
                    File.WriteAllText(jvmArgsPath, Strings.Join(jvmArgs.ToArray()));
                }

                Shell.Execute(jreBinary, Client.GetDirectory(), true, jvmArgs.Append("-jar").Append(client));
                Environment.Exit(0);
            });
        }

        public void SetStatus(string status)
        {
            Dispatcher.UIThread.Post(() => _statusBlock.Text = status);
        }
    }
}