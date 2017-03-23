using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Win32;
using System.IO;

namespace FileMoverUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsMoving
        {
            get
            {
                return !ViewModel.CanMove;
            }
        }

        public Model ViewModel { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new Model();
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFilePath.Text = openFileDialog.FileName;
            }
        }

        private void ChooseDestination_Click(object sender, RoutedEventArgs e)
        {
            
            var selectDestDialog = new SaveFileDialog();
            selectDestDialog.FileName = new FileInfo(SourceFilePath.Text).Name;
            if (selectDestDialog.ShowDialog() == true)
            {
                DestinationPath.Text = selectDestDialog.FileName;
            }
        }

        public void UpdateProgressBar(decimal progress)
        {
            if (Thread.CurrentThread == Dispatcher.Thread)
            {
                this.ProgressBar.Value = (double)progress;
            }
            else
            {
                Dispatcher.Invoke(() => UpdateProgressBar(progress));
            }
        }

        private async void Move_Click(object sender, RoutedEventArgs e)
        {

            UpdateProgressBar(0M);
            this.InfoMessage.Foreground = Brushes.Black;
            this.InfoMessage.Content = "Moving...";
            bool result = false;
            try
            {
                if (ViewModel.CanMove)
                {
                    result = await ViewModel.MoveAsync(this.SourceFilePath.Text, this.DestinationPath.Text, UpdateProgressBar);
                }
            }
            catch(Exception x)
            {
                result = false;
                this.InfoMessage.Content = x.Message;

            }
            if (result)
            {
                this.InfoMessage.Content = "Moved";
                this.InfoMessage.Foreground = Brushes.Green;
            }
            else
            {
                this.InfoMessage.Foreground = Brushes.Red;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
        }

        private async void Copy_Click(object sender, RoutedEventArgs e)
        {
            UpdateProgressBar(0M);
            this.InfoMessage.Foreground = Brushes.Black;
            this.InfoMessage.Content = "Moving...";
            bool result = false;
            try
            {
                if (ViewModel.CanMove)
                {
                    result = await ViewModel.CopyAsync(this.SourceFilePath.Text, this.DestinationPath.Text, UpdateProgressBar);
                }
            }
            catch (Exception x)
            {
                result = false;
                this.InfoMessage.Content = x.Message;

            }
            if (result)
            {
                this.InfoMessage.Content = "Moved";
                this.InfoMessage.Foreground = Brushes.Green;
            }
            else
            {
                this.InfoMessage.Foreground = Brushes.Red;
            }
        }
    }
}
