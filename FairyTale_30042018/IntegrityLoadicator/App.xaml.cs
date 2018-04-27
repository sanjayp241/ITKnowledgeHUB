using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SoftwareLocker;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        

        public App()
        {
           
            ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
        //System.Windows.Forms.Application.EnableVisualStyles();
        // System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                if (ApplicationRunningHelper.AlreadyRunning())
                {
                    System.Windows.Application.Current.Shutdown();
                    return;
                }
                SoftwareLocker.TrialMaker t = new SoftwareLocker.TrialMaker("ZebecLoadMasterGasCommerce", System.IO.Directory.GetCurrentDirectory() + "\\RegFile.reg",
                    Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\1L2O3D4I5C6A7T8O9R2.dbf",
                    "",
                    20, "745");
                byte[] MyOwnKey = { 97, 250, 1, 5, 84, 21, 7, 63,
            4, 54, 87, 56, 123, 10, 3, 62,
            7, 9, 20, 36, 37, 21, 101, 57};

                t.TripleDESKey = MyOwnKey;
                //TrialMaker.RunTypes Rt=
                TrialMaker.RunTypes RT = t.ShowDialog();
                //bool is_trial;
                if (RT != TrialMaker.RunTypes.Expired)
                {
                    //if (RT == TrialMaker.RunTypes.Full)
                    //    is_trial = false;
                    //else
                    //    is_trial = true;

                    //System.Windows.Application.Run(new MainWindow(is_trial))
                    //if (is_trial == true)
                    //{

                    System.Windows.Application.Current.StartupUri = new Uri("/ZebecLoadMaster;component/MainWindow.xaml",
                            UriKind.Relative);
                    //}
                    //else
                    //{
                    //    System.Windows.Application.Current.Shutdown();
                    //}
                    //LoginDialog dlg = new LoginDialog();
                    //if (dlg.ShowDialog() != true)
                    //    return;

                    //switch (dlg.ChoiceApp)
                    //{
                    //    case ChoiceApp.CustomerEntry:
                    //        StartupUri = new Uri("/MyApp;component/Forms/CustomerEntry.xaml",
                    //            UriKind.Relative);
                    //        break;
                    //    case ChoiceApp.VendorEntry:
                    //        StartupUri = new Uri("/MyApp;component/Forms/VendorEntry.xaml",
                    //            UriKind.Relative);
                    //        break;
                    //}
                }
                else if (RT == TrialMaker.RunTypes.Expired)
                {
                    //System.Windows.MessageBox.Show("Trial Period of this Application Has Expired.Please Contact Administrator");
                    System.Windows.Application.Current.Shutdown();

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "Error");
            }
        }
    }
}
