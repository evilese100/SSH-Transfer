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
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("profiles.txt") == true)
            {
                string[] readprofilels = File.ReadAllLines("profiles.txt", Encoding.UTF8);
                for (int i = 0; i < readprofilels.Length; i++ )
                {
                    profile_ls.Items.Add(readprofilels[i]);
                }
                File.Create("config.txt");
            }
            else
            {
                File.Create("profiles.txt");
                string[] readconfig = File.ReadAllLines("config.txt", Encoding.UTF8);
                if (readconfig.Length != 0)
                {
                    ip_address.Text = readconfig[0];
                    user_s.Text = readconfig[1];
                    path_s.Text = readconfig[2];
                };
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
            if ((profile_name.Text == "") && (profile_ls.SelectedItem == null))
            {
                MessageBox.Show("Enter a profile name or select an existing one!");
            }
            else if ((profile_name.Text == "") && (profile_ls.SelectedItem != null))
            {
                string[] editProfile = { ip_address.Text, user_s.Text, path_s.Text };
                File.WriteAllLines($"{profile_ls.SelectedItem.ToString()}.txt", editProfile, Encoding.UTF8);
            }
            else
            {
                using (System.IO.StreamWriter file = new StreamWriter("profiles.txt", true, Encoding.Default))
                {
                    file.WriteLine(profile_name.Text);
                };
                string[] configure = { ip_address.Text, user_s.Text, path_s.Text };
                File.WriteAllLines($"{profile_name.Text}.txt", configure, Encoding.UTF8);
                string[] readprofilels = File.ReadAllLines("profiles.txt", Encoding.UTF8);
                profile_ls.Items.Clear();
                for (int i = 0; i < readprofilels.Length; i++)
                {
                    profile_ls.Items.Add(readprofilels[i]);
                    profile_ls.SelectedItem = readprofilels[i];
                };
            };
        }

        private void clear_par_Click(object sender, RoutedEventArgs e)
        {
            ip_address.Text = "";
            user_s.Text = "";
            path_s.Text = "";
            var re = File.ReadAllLines("profiles.txt", Encoding.Default).Where(s => !s.Contains($"{profile_ls.SelectedItem.ToString()}"));
            File.WriteAllLines("profiles.txt", re, Encoding.Default);
            File.Delete($"{profile_ls.SelectedItem.ToString()}.txt");
            profile_ls.Items.Remove(profile_ls.SelectedItem);
        }

        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (profile_ls.SelectedItem != null)
            {
                string[] readprofile = File.ReadAllLines($"{profile_ls.SelectedItem.ToString()}.txt", Encoding.UTF8);
                ip_address.Text = readprofile[0];
                user_s.Text = readprofile[1];
                path_s.Text = readprofile[2];
            }
        }
    }
}
