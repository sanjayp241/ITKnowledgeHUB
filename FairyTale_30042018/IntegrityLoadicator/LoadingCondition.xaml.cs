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
using System.Data;
using System.Data.Common;
using System.IO;
using System.Collections;
using ZebecLoadMaster.Models.DAL;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for LoadingCondition.xaml
    /// </summary>
    public partial class LoadingCondition : Window
    {
        string CalculationMethod, DamageCase;
        public static string folder = "";
        int index;
        public LoadingCondition(string folderName)
        {
            InitializeComponent();
            LoadingConditionList(folderName);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void LoadingConditionList(string folderName)
        {
            try
            {
                folder = folderName;
                lblError.Visibility = Visibility.Hidden;
                string st = System.IO.Directory.GetCurrentDirectory();
                string path = st + folderName;
               
                var dir = System.IO.Directory.GetDirectories(path).OrderBy(d => new System.IO.DirectoryInfo(d).FullName);
                if (folder == "\\SMData")
                {
                    listBoxSavedCondition.Items.SortDescriptions.Add(
                          new System.ComponentModel.SortDescription("",
                          System.ComponentModel.ListSortDirection.Descending));
                    index = st.Length + 8;
                    btnDelete.Visibility = Visibility.Visible;
                    lblConditionType.Content = "Saved Loading Condition";
                }
                else
                {
                    
                    index = st.Length + 14;
                    btnDelete.Visibility = Visibility.Hidden;
                    lblConditionType.Content = "Standard Loading Condition";
                }
                string names;
                foreach (string s in dir)
                {
                    names = s.Remove(0, index);
                    listBoxSavedCondition.Items.Add(names);
                }
                //listBoxSavedCondition.Items.SortDescriptions.Add(
                //        new System.ComponentModel.SortDescription("",
                //        System.ComponentModel.ListSortDirection.Ascending));
                if (listBoxSavedCondition.Items.Count == 0)
                {
                    lblError.Visibility = Visibility.Visible;
                }
            }
            catch
            {

            }

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                if (listBoxSavedCondition.SelectedItem != null)
                {
                    string filename = listBoxSavedCondition.SelectedItem.ToString();
                    if (folder == "\\SMData")
                    {
                        string[] file1 = filename.Split('_');
                        CalculationMethod = file1[5].ToString();
                        if (CalculationMethod == "Damage")
                        {
                            DamageCase = file1[5].ToString();
                            MainWindow.CheckNCheckOutCount = 1;
                            //if (chk1 == null) { checkBox = false; } else { checkBox = (bool)chk1.IsChecked; }
                        }
                    }
                    //Models.clsGlobVar.stdLoad = "Saved";
                    //MainWindow.saveDate = listBoxSavedCondition.SelectedItem.ToString();
                    //MainScreen obj = new MainScreen();
                   // Switcher.Switch(new MainScreen());
                    //MainWindow.refresh();
                   
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            if (folder == "\\SMData")
                            {
                                (window as MainWindow).lblCalculationMethod.Content = CalculationMethod;

                                if (CalculationMethod == "Intact")
                                {
                                    (window as MainWindow).btnToggle.IsChecked = false;
                                }
                                else
                                {
                                    (window as MainWindow).btnToggle.IsChecked = true;
                                    if (DamageCase == "Damage Case-01")
                                    {
                                        //(window as MainWindow).radioButtonCaseA.IsChecked = true;
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 1;
                                    }
                                    else if (DamageCase == "Damage Case-02")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 2;
                                    }
                                    else if (DamageCase == "Damage Case-03")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 3;
                                    }
                                    else if (DamageCase == "Damage Case-04")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 4;
                                    }
                                    else if (DamageCase == "Damage Case-05")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 5;
                                    }
                                    else if (DamageCase == "Damage Case-06")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 6;
                                    }
                                    else if (DamageCase == "Damage Case-07")
                                    {
                                        (window as MainWindow).cmbDamageCases.SelectedIndex = 7;
                                    }
                                    //else if (DamageCase == "Case B")
                                    //{
                                    //    (window as MainWindow).radioButtonCaseB.IsChecked = true;
                                    //}
                                    //else if (DamageCase == "Case C")
                                    //{
                                    //    (window as MainWindow).radioButtonCaseC.IsChecked = true;
                                    //}
                                    //else if (DamageCase == "Case D")
                                    //{
                                    //    (window as MainWindow).radioButtonCaseD.IsChecked = true;
                                    //}
                                    //else if (DamageCase == "Case E")
                                    //{
                                    //    (window as MainWindow).radioButtonCaseE.IsChecked = true;
                                    //}
                                }                           
                              
                            }
                            else
                            {
                                (window as MainWindow).lblCalculationMethod.Content = "Intact";
                                (window as MainWindow).btnToggle.IsChecked = false;
                            }
                            refresh();
                            
                            (window as MainWindow).dgTanks.ItemsSource = Models.clsGlobVar.dtSimulationAllTanks.DefaultView;
                            (window as MainWindow).dgFixedLoad.ItemsSource = Models.clsGlobVar.dtSimulationVariableItems.DefaultView;                        
                            (window as MainWindow).txtLoadingConditionName.Text = listBoxSavedCondition.SelectedItem.ToString();
                            
                        }
                    }
                   
                    this.Close();
                    MessageBox.Show(listBoxSavedCondition.SelectedItem.ToString() + " Loading Condition Loaded");
                    //Models.clsGlobVar.flagLoadingCondition = true;
                }
                else
                {
                    MessageBox.Show("Please Select a Loading Condition");
                }
                Mouse.OverrideCursor = null;
            }
            catch
            {
                this.Close();
                Mouse.OverrideCursor = null;
            }

            
        }

        private void refresh()
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory() + folder + "\\" + listBoxSavedCondition.SelectedItem.ToString();

              
                FileStream fs = new FileStream(path + "\\Tanks.cnd", FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryFormatter ob = new BinaryFormatter();


                List<Tanks> listTank = new List<Tanks>();
                listTank = (List<Tanks>)ob.Deserialize(fs);
                fs.Close();
                //dtSMBallast= liBallast.toDa
                DataTable dtTanks = CollectionHelper.ConvertTo<Tanks>(listTank);
                //dtTanks = dtSMTanks.Clone();
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                string cmd = "";

                foreach (DataRow row in dtTanks.Rows)
                {
                    cmd += " UPDATE tblSimulationMode_Tank_Status  SET Volume=" + row["Volume"].ToString() + ",SG=" + row["SG"].ToString() + ",IsDamaged=" + Convert.ToInt16(row["IsDamaged"]) + " WHERE Tank_ID=" + row["Tank_ID"].ToString() + " Update tblFSM_max_act set max_1_act_0=" + row["max_1_act_0"].ToString() + " WHERE Tank_ID=" + row["Tank_ID"].ToString();
                }
                //con.Close();
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                //for (int i = 0; i < dtTanks.Rows.Count; i++)
                //{
                //    dtTanks.ImportRow(dtSMTanks.Rows[i]);

                //}
                //dtTanks.AcceptChanges();

                fs = new FileStream(path + "\\FixedLoads.cnd", FileMode.Open, FileAccess.Read, FileShare.None);
                //  ob = new BinaryFormatter();
                cmd = "";
                List<FixedItems> liDeck1 = new List<FixedItems>();
                liDeck1 = (List<FixedItems>)ob.Deserialize(fs);
                fs.Close();
                DataTable dtFixedLoads = CollectionHelper.ConvertTo<FixedItems>(liDeck1);
                command = Models.DAL.clsDBUtilityMethods.GetCommand();
                foreach (DataRow row in dtFixedLoads.Rows)
                {
                    cmd += " UPDATE tblSimulationMode_Loading_Condition SET LCG=" + row["LCG"].ToString() + ",VCG=" + row["VCG"].ToString() + ",TCG=" + row["TCG"].ToString() + "  WHERE Tank_ID=" + row["Tank_ID"].ToString();
                    cmd += " UPDATE tblSimulationMode_Tank_Status SET Weight=" + row["Weight"].ToString() + "  WHERE Tank_ID=" + row["Tank_ID"].ToString(); ;
                }

                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                Models.clsGlobVar.dtSimulationAllTanks = dtTanks;
                Models.clsGlobVar.dtSimulationVariableItems = dtFixedLoads;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxSavedCondition.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to Delete Selected Loading Condition ?", "Delete Loading Condition", MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Directory.Delete(System.IO.Directory.GetCurrentDirectory() + folder + "\\" + listBoxSavedCondition.SelectedItem.ToString(), true);
                            listBoxSavedCondition.Items.Clear();
                            LoadingConditionList(folder);
                            break;
                        case MessageBoxResult.No:

                            break;
                        case MessageBoxResult.Cancel:

                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Loading Condition");
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
