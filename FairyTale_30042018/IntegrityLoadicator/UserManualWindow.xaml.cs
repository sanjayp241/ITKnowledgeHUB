using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for UserManualWindow.xaml
    /// </summary>
    public partial class UserManualWindow : Window
    {
        System.Windows.Controls.WebBrowser webBrowser = new System.Windows.Controls.WebBrowser();
        public UserManualWindow()
        {
            InitializeComponent();
            try
            {
                string path = System.Windows.Forms.Application.StartupPath + "\\User_Manual.Pdf";
                windowUserManual.WindowState = WindowState.Maximized;

                webBrowser.Navigate(path);
                windowUserManual.Content = webBrowser;
                windowUserManual.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("The file location could not be found.", "Location Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void windowUserManual_Closed(object sender, EventArgs e)
        {
            //base.OnClosed(e);
            webBrowser.Dispose();
            windowUserManual.Content = null;
            //windowUserManual.Close();
        
        }
    }
}
