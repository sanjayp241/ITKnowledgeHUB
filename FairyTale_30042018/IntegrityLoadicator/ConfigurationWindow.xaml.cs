 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ServiceProcess;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Xml;
using System.IO;
using Microsoft.Win32;


namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader reader;
        string sql = "";
        string connectionstring = "";
        string connectionstring1 = "";
        private XmlDocument doc;
        string pwdCheck;
        string password;
        string user;
        private string PATH = System.IO.Directory.GetCurrentDirectory() + @"\Settings\StabilityConfig.xml";
        public ConfigurationWindow()
        {
            InitializeComponent();
            instances();
        }
        public void instances()
        {
            try
            {  
                btnCreate.IsEnabled = false;
                RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
                using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
                {
                    try
                    {
                        RegistryKey rk = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                        if (rk != null)
                        {
                            String[] instances = (String[])rk.GetValue("InstalledInstances");
                            //String[] instances = (String[])rk.GetValueNames();
                            if (instances.Length > 0)
                            {
                                foreach (String element in instances)
                                {
                                    if (element == "MSSQLSERVER")
                                         MainWindow._servername= System.Environment.MachineName;
                                    else
                                     MainWindow._servername = System.Environment.MachineName + @"\" + element; //For Other System
                                       // MainWindow._servername = System.Environment.MachineName;   //For Sangita System
                                }
                            }
                            rk.Close();
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        RegistryKey key = hklm.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL");
                        if (key != null)
                        {                     
                            String[] instances = (String[])key.GetValueNames();
                         
                            if (instances.Length > 0)
                            {
                                foreach (String element in instances)
                                {
                                    // MessageBox.Show(element);
                                    if (element == "MSSQLSERVER")
                                         MainWindow._servername =System.Environment.MachineName;
                                    else
                                         MainWindow._servername =System.Environment.MachineName + @"\" + element;
                                }
                            }
                            key.Close();
                        }
                        hklm.Close();

                        XmlDocument doc = new XmlDocument();
                        XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        doc.AppendChild(declaration);
                        XmlComment comment = doc.CreateComment("This is an XML Generated File");
                        doc.AppendChild(comment);
                        XmlElement root = doc.CreateElement("Settings");
                        doc.AppendChild(root);
                        XmlElement ServerName = doc.CreateElement("ServerName");
                        ServerName.InnerText = MainWindow._servername;
                        root.AppendChild(ServerName);
                        doc.Save(PATH);
                    }
                    catch
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Backup Files(*.bak)|*.bak|All Files(*.*)|*.*";
            dlg.FilterIndex = 0;
            if (dlg.ShowDialog() == true)
            {
                textBoxPath.Text = dlg.FileName;
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
             try
              {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    if (textBoxPath.Text == "")
                    {
                        MessageBox.Show("Please Select Database Path");
                        return;
                     }           
                        connectionstring1 = "Data Source=" + MainWindow._servername + ";Initial Catalog=master;Integrated Security=SSPI";
                        conn = new SqlConnection(connectionstring1);
                        conn.Open();
                        sql = "USE Master;";
                        sql += "Alter Database FairyTale_Stability Set OFFLINE WITH ROLLBACK IMMEDIATE;";
                        sql += "Restore Database FairyTale_Stability FROM Disk = '" + textBoxPath.Text + "' WITH REPLACE;";
                        command = new SqlCommand(sql, conn);
                        command.CommandTimeout = 1000;
                        command.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        MessageBox.Show("Database Successfully Restored");
                        Mouse.OverrideCursor = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Mouse.OverrideCursor = null;
                }
        }

      

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please restart application for changes to take effect.");
            this.Close();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                if (File.Exists(PATH)) { File.Delete(PATH); }

                ServiceController sc = new ServiceController("Sql Server (SQLEXPRESS)");

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                }
                connectionstring1 = "Data Source=" + MainWindow._servername + ";Initial Catalog=master;Integrated Security=SSPI";
                conn = new SqlConnection(connectionstring1);
                conn.Open();
                sql = "USE Master;";
                sql += "IF EXISTS(SELECT name FROM sys.databases  WHERE name = 'FairyTale_Stability')";
                sql += "DROP DATABASE FairyTale_Stability";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                MessageBox.Show("Connection refresh successfully ");
                conn.Close();
                conn.Dispose();
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Mouse.OverrideCursor = null;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                connectionstring1 = "Data Source=" + MainWindow._servername + ";Initial Catalog=master;Integrated Security=SSPI";
                conn = new SqlConnection(connectionstring1);
                conn.Open();
                sql = "";
                //sql= "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + cmbDatabase.Text + "') DROP DATABASE " + cmbDatabase.Text + " RESTORE DATABASE " + cmbDatabase.Text + " FROM DISK = '" + txtRestoreFileLocation.Text + "'";
                sql = "USE Master;";
                //sql += "Alter Database Stability Set OFFLINE WITH ROLLBACK IMMEDIATE;";
                sql += "Create database FairyTale_Stability";
                command = new SqlCommand(sql, conn);
                command.CommandTimeout = 1000;
                command.ExecuteNonQuery();
                MessageBox.Show("Database Created Successfully");
                conn.Close();
                conn.Dispose();
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Mouse.OverrideCursor = null;
            }
        }

        //private void listBoxSQLInstances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        textBoxServerName.Text = listBoxSQLInstances.SelectedItem.ToString();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Please select Instance");
        //    }

        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Flags == 0)
            {
                btnConnect.IsEnabled = false;
                btnCreate.IsEnabled = true;
            }
            else
            {
                btnConnect.IsEnabled = true;
                btnCreate.IsEnabled = true; ;
            }
        }
    }
}
