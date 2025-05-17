using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using MomoScan.Common;

namespace MomoScan.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += MainWindow_Closing;
    }
    
    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string input = InputTextBox.Text;
            NotificationHub.ScanOccured(input, EventArgs.Empty);
            InputTextBox.Text = "";
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        // Prompt the user to confirm closing
        var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            Task.Run(() =>
            {

            }).Wait();
        }
        else
        {
            e.Cancel = true;
        }
    }
}