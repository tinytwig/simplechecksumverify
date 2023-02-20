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
using System.Security.AccessControl;

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #region Dictionary

        private string algorithm;
        private string path;
        private string hash;

        #endregion Dictionary

        #region Utility
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

            if (btn_sha256.IsChecked == false && btn_md5.IsChecked == false)
            {
                lbl_result.Content = "Please select an algorithm.";
                lbl_result.Foreground = Brushes.Red;
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
                txtbx_help.Text = "The provided hash matches the hash of the file you downloaded! This file should be safe to use, as long as you have verified the source, from which you have downloaded this file, is a trusted source.";
            }

            if (txtbx_user.Text != txtbx_produced.Text)
            {
                if (string.IsNullOrEmpty(txtbx_user.Text))
                {
                    lbl_result.Content = "Please paste the hash that is provided by the trusted source.";
                    lbl_result.Foreground = Brushes.Red;
                    txtbx_help.Text = "Don't forget to copy and paste the hash that has been provided by the trusted source from which you are downloading this file! This is very important, as it is the reference we will be using to verify the authenticity of the downloaded file.";
                }
                else
                {
                    lbl_result.Content = "Does not match.";
                    lbl_result.Foreground = Brushes.Red;
                    txtbx_help.Text = "The provided hash does not match the hash produced by the file you have downloaded. This could be due to 3 reasons:\n 1) The hash has been entered incorrectly\n 2) The file path is incorrect\n 3) The file that was downloaded is corrupted, the incorrect version, or is outright malicious.";
                }
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

        #endregion Utility

        #region Controls
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            var FileBrowse = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*"
            };
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
