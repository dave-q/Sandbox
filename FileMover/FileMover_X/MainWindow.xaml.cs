using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FileMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model ViewModel { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new Model(new FileMoverModel.FileMover_PInvoke());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
            var selectFolderDialog = new FolderBrowserDialog();
            if (selectFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DestinationPath.Text = selectFolderDialog.SelectedPath;
            }
        }

        public void UpdateProgressBar(decimal progress)
        {
            this.ProgressBar.Value = (double)progress;
        }

        private async void Move_Click(object sender, RoutedEventArgs e)
        {
            this.InfoMessage.Foreground = Brushes.Black;
            this.InfoMessage.Content = "Moving...";
            var result = await ViewModel.MoveAsync(this.SourceFilePath.Text, this.DestinationPath.Text, UpdateProgressBar);
            this.InfoMessage.Content = result.Message;
            if (result.Success)
            {
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
    }
}
