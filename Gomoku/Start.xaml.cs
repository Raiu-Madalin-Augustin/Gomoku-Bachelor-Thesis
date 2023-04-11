using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Gomoku.GUI
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void BtnClickStartGame(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();  
        }
    }
}
