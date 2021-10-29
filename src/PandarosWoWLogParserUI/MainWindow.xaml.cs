using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Autofac;
using PandarosWoWLogParser;
using System.Diagnostics;

namespace PandarosWoWLogParserUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ParserArgs _args;
        public MainWindow()
        {
            InitializeComponent();
            _args = new ParserArgs();
            DataContext = _args;
        }

        private void SelectWoWLogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _args.LogFile = openFileDialog.FileName;
                FileLocation.Text = openFileDialog.FileName;
            }
        }

        private void SelectOutputLogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _args.Output = openFileDialog.FileName;
                OutputLocation.Text = openFileDialog.FileName;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseButton.IsEnabled = false;

            if (!File.Exists(_args.LogFile))
            {
                CompleteLabel.Content = "WoW log file not found.";
                ParseButton.IsEnabled = true;
                return;
            }

            var logger = new PandaLogger(_args.Output);
            await Task.Run(() =>
            {
                var builder = new ContainerBuilder();
                builder.PandarosParserSetup(logger, logger);

                var Container = builder.Build();

                var clp = Container.Resolve<CombatLogParser>();
                clp.PctComplete += Clp_PctComplete;
                clp.ParseToEnd(_args.LogFile);
            });
            ParseButton.IsEnabled = true;

            using (Process myProcess = new Process())
            {
                if (File.Exists(@"C:\Users\Jim\AppData\Local\Programs\Microsoft VS Code\Code.exe"))
                {
                    myProcess.StartInfo.FileName = @"C:\Users\Jim\AppData\Local\Programs\Microsoft VS Code\Code.exe"; //not the full application path
                    myProcess.StartInfo.Arguments = logger._logFile;
                    myProcess.Start();
                }
                else
                {
                    myProcess.StartInfo.FileName = @"notepad.exe"; //not the full application path
                    myProcess.StartInfo.Arguments = logger._logFile;
                    myProcess.Start();
                }
            }
        }

        private void Clp_PctComplete(object sender, double e)
        {
            CompleteLabel.Dispatcher.Invoke(() => { CompleteLabel.Content = "Complete: " + e + "%"; });
        }
    }

    public class ParserArgs
    {
        public string LogFile { get; set; } = @"C:\Program Files\Ascension Launcher\resources\client\Logs\WoWCombatLog.txt";
        public string Output { get; set; } = @"c:\temp\";
    }
}
