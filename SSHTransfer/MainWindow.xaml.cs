using System;
using System.IO;
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
using System.Diagnostics;
using Microsoft.Win32;

namespace SSHTransfer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void defalutconfig()
        {
            string[] configure_default = { "IP-адрес сервера", "Имя пользователя", "Путь на сервере" };
            File.WriteAllLines("config.txt", configure_default, Encoding.UTF8);
        }
        public MainWindow()
        {
            InitializeComponent();
            string[] readconfig = File.ReadAllLines("config.txt", Encoding.UTF8);
            if (readconfig[0] != "IP-адрес сервера")
            {
                ip_address.Text = readconfig[0];
                user_s.Text = readconfig[1];
                path_s.Text = readconfig[2];
            }
            else
            {
                defalutconfig();
            };
        }
        
        public string GetPath()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }
        private void find_file_Click(object sender, RoutedEventArgs e)
        {
            file_path.Text= GetPath();
        }

        private void run_tr_Click(object sender, RoutedEventArgs e)
        {
            Process proc = Process.Start(new ProcessStartInfo
            {
                FileName = "pscp",
                Arguments = $"-pw {passwd_s.Password} {file_path.Text} {user_s.Text}@{ip_address.Text}:{path_s.Text}",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
        }

        private void save_par_Click(object sender, RoutedEventArgs e)
        {
            string [] configure = {ip_address.Text, user_s.Text, path_s.Text};
            File.WriteAllLines("config.txt", configure, Encoding.UTF8);
        }

        private void clear_par_Click(object sender, RoutedEventArgs e)
        {
            defalutconfig();
            string[] readconfig = File.ReadAllLines("config.txt", Encoding.UTF8);
            ip_address.Text = readconfig[0];
            user_s.Text = readconfig[1];
            path_s.Text = readconfig[2];
        }
    }
}
