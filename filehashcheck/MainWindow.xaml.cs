using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
using System.Diagnostics;
using System.Runtime;
using System.IO;
using System.Xml.Schema;
using System.Text.RegularExpressions;

namespace filehashcheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string algorithm;
        private string path;
        private string hash;

        public void Generate()
        {
            Algo();
            if ((btn_md5.IsChecked == true | btn_sha256.IsChecked == true) && !string.IsNullOrEmpty(txtbx_file.Text))
            {
                string command = $"CertUtil -hashfile {path} {algorithm}";
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                processInfo.RedirectStandardOutput = true;
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;

                using (Process process = Process.Start(processInfo))
                {
                    using (StreamReader read = process.StandardOutput)
                    {
                        string output = read.ReadToEnd();
                        string[] result = output.Split('\n');
                        hash = result[1];
                        hash = Regex.Replace(hash, @"\s", "");
                        txtbx_produced.Text = hash;
                    }
                }
            }

        }

        private void Algo()
        {
            if (btn_sha256.IsChecked == true)
            {
                algorithm = "SHA256";
            }

            if (btn_md5.IsChecked == true)
            {
                algorithm = "MD5";
            }
        }

        private void Matchcheck()
        {
            if (txtbx_user.Text == txtbx_produced.Text)
            {
                lbl_result.Content = "Matches!";
                lbl_result.Foreground = Brushes.Green;
            }

            if (txtbx_user.Text != txtbx_produced.Text)
            {
                lbl_result.Content = "Does not match.";
                lbl_result.Foreground = Brushes.Red;
            }
        }
        private void txtbx_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            Matchcheck();
        }

        private void txtbx_produced_TextChanged(object sender, TextChangedEventArgs e)
        {
            Matchcheck();
        }

        #region Controls
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            var FileBrowse = new OpenFileDialog();
            FileBrowse.Filter = "All files (*.*)|*.*";
            FileBrowse.ShowDialog();
            path = FileBrowse.FileName;
            txtbx_file.Text = path;
            if (File.Exists(path) == true)
            {
                btn_generate.IsEnabled = true;
            }
        }

        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {
            Generate();
        }
        #endregion

    }
}
