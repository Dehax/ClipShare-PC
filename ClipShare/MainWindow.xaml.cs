using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace ClipShare
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClipShareService _clipShare;

        public MainWindow()
        {
            InitializeComponent();

            _clipShare = new ClipShareService();
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string textToCopy = text.Text;

            _clipShare.SendText(textToCopy);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _clipShare.Close();
        }

        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = _clipShare.FindAndSetRemoteIP();
            ipAddress.Text = ip.ToString();
        }
    }
}
