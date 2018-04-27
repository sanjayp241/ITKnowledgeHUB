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
using System.Windows.Navigation;
using System.Windows.Shapes;    
using ZebecLoadMaster.Models;
using ZedGraph;
using System.Data;
using System.Data.Common;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using ZebecLoadMaster.Models.DAL;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Reflection;


#region CADLIB
using WW.Cad.Base;
using WW.Cad.Drawing;
using WW.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Math;
using WW.Cad.Drawing.Wpf;
using WW.Cad.Model.Objects;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Windows.Controls.Primitives;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using System.ServiceProcess;
using iTextSharp.text.pdf.draw;

#endregion
namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        BackgroundWorker bgWorker;
        int totalRow, totalRowLongitudinal;
        public System.Windows.Controls.CheckBox chk1;
        double[] x, x1, y1, y2, y3, y4, y5, y6;// xBMmax, yBMmax, xBMmin, yBMmin, xSFmax, ySFmax, xSFmin, ySFmin;
        GraphPane myGraph;
        int index;
        string header;
        int TankID;
        string reportPath;
        Double DraftsMid = 0;
        int flagDraft = 0;
        protected internal static string saveDate;
        private bool isCalculationRunning = false;
        private bool isHatchSelectionProfile = false;
        public static Dictionary<int, decimal> maxVolume;
        DataTable dtIntactCondition;
        public static bool IsTrial;
        public static int CheckNCheckOutCount, jjk = 0;
        public int _DamageStabilityCnt = 0;
        public string _damageTimeinterval_1;
        public string _damageTimeinterval_14;
        public string[,] _statusOKnNotOK = new string[41, 2];
        public static string[,] _DamageCases = new string[42, 4];
        string CmprItem = string.Empty;
        public int DamageAll = 0;
        private delegate void UpdateProgressBarDelegate(
        System.Windows.DependencyProperty dp, Object value);

        FRMLoading objLoading = new FRMLoading();
        public static string _servername;
        string connectionstring1 = "";
        private System.Data.SqlClient.SqlConnection conn;
        private System.Data.SqlClient.SqlDataReader reader;
        string sql = "";
        System.Data.SqlClient.SqlCommand command;
        public static int Flags = 0;
        public string DataBnding = "Sample";


        students sdnt = new students();

        public MainWindow()
        {
            clsSqlData.ReadValuefromXml();
            InitializeComponent();
            students sdnt = new students();
            sdnt.Myid = 10;
            sdnt.MyName = "Arnav";
            sdnt.MyPhone = 9999;
            this.DataContext = sdnt;
        
            if (_servername == null)
            { objLoading.Show(); }
            MainLoad();
            foreach (var wnd in System.Windows.Application.Current.Windows)
            {
                if (wnd is FRMLoading)
                {
                    objLoading.Close();
                }
            }
            this.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(canvasTwoD_MouseRightButtonDown);
        }
        public void MainLoad()
        {
            try
            {
                if (IsTrial == true)
                {
                    System.Windows.Application.Current.Shutdown();
                }
                else
                {
                    SortedList FSMList = new SortedList();
                    FSMList.Add("Actual", "0");
                    FSMList.Add("MAX", "1");
                    FSMType.ItemsSource = FSMList;
                    cmbDamageCases.IsEnabled = false;
                    cmbDamageCases.Text = "Select Damage Case";

                    // SFBMCondition.IsEnabled = true;
                    window.Height = window.Height - 30;
                    //clsSqlData.ReadValuefromXml();
                    if (_servername == null)
                    {
                        FRMConfiguration();
                    }
                    DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    string Err = "";
                    string cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 68";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                    Models.TableModel.CoordinateData();

                    RefreshScreen();
                    MaxVolume();
                    btnGenerateReport.IsEnabled = false;
                    btnSaveLoadingCondition.IsEnabled = false;
                    tabLongitudinal.IsEnabled = true;
                   // tabItem7.IsEnabled = true;
                    // lblLoading.IsEnabled = false;
                }
            }
            catch
            {
                Flags = 1;
                System.Windows.MessageBox.Show("Not able to Connect with Database. Please Check Configuration Settings or Contact Administrator");
            }
        }
        #region Binding
        public class students
        {
            private int ID;

            public int Myid
            {
                get { return ID; }
                set { ID = value; }
            }
            private int phone;

            public int MyPhone
            {
                get { return phone; }
                set { phone = value; }
            }
            private string Name;

            public string MyName
            {
                get { return Name; }
                set { Name = value; }
            }
            
            
            
        }

        public class AnotherClass : INotifyPropertyChanged
        {
            private string anotherfield;
            private Employee emp;
            public string AnotherField
            {
                get { return anotherfield; }
                set
                {
                    anotherfield = value;
                    OnPropertyChanged("AnotherField");
                }
            }
            public Employee EmployeeNameTest
            {
                get { return emp; }
                set
                {
                    emp = value;
                    OnPropertyChanged("EmployeeNameTest");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            //// Create the OnPropertyChanged method to raise the event
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
            public override string ToString()
            {
                return string.Format("My ToString implementation of AnotherClass");
            }
        }

        public class Employee : INotifyPropertyChanged
        {
            private string name;
            private string state;
            // Declare the event
            public event PropertyChangedEventHandler PropertyChanged;
            public Employee()
            {
            }
            public Employee(string value)
            {
                this.name = value;
            }
            public string EmployeeName
            {
                get { return name; }
                set
                {
                    name = value;
                    // Call OnPropertyChanged whenever the property is updated
                    OnPropertyChanged("EmployeeName");
                }
            }
            public string State
            {
                get { return state; }
                set
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
            //// Create the OnPropertyChanged method to raise the event
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        #endregion
        public void FRMConfiguration()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                ConfigurationWindow objInastnce = new ConfigurationWindow();
                objInastnce.instances();
                connectionstring1 = "Data Source=" + _servername + ";Initial Catalog=master;Integrated Security=SSPI";
                conn = new SqlConnection(connectionstring1);
                conn.Open();
                DataSet Dt = new DataSet();
                sql = "SELECT * FROM master.dbo.sysdatabases WHERE name ='FairyTale_Stability'";
                command = new System.Data.SqlClient.SqlCommand(sql, conn);
                command.CommandTimeout = 1200;
                reader = command.ExecuteReader();

                ArrayList ArrayLT = new ArrayList();
                while (reader.Read())
                {
                    ArrayLT.Add(reader[0].ToString());
                }

                if (ArrayLT.Count < 1)
                {
                    conn = new SqlConnection(connectionstring1);
                    conn.Open();
                    sql = "";
                    sql = "USE Master;";
                    sql += "Create database FairyTale_Stability";
                    command = new SqlCommand(sql, conn);
                    command.CommandTimeout = 1000;
                    command.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                }
                

                {
                    string path = System.Windows.Forms.Application.StartupPath;
                    string dbBakup = path + "\\Stability_Bakup.Bak";
                    conn = new SqlConnection(connectionstring1);
                    conn.Open();
                    sql = "USE Master;";
                    sql += "Alter Database FairyTale_Stability Set OFFLINE WITH ROLLBACK IMMEDIATE;";
                    sql += "Restore Database FairyTale_Stability FROM Disk = '" + dbBakup + "' WITH REPLACE;";
                    command = new SqlCommand(sql, conn);
                    command.CommandTimeout = 1000;
                    command.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    Mouse.OverrideCursor = null;
                    objLoading.PrgsLoading.Value = 70;
                }
            }
            catch (Exception ex)
            {
                Flags = 1;
                //System.Windows.MessageBox.Show("Problem while Configuring Database").ToString();
            }
        }
        #region ControlEvents
        private void Configuration_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow objConfigurationWindow = new ConfigurationWindow();
            objConfigurationWindow.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow objAboutWindow = new AboutWindow();
            objAboutWindow.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnToggle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                //speechSynthesizer.Speak("Switch to Damage!");

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                tabLongitudinal.IsEnabled = true;
                tabItem7.IsEnabled = false;
                btnToggle.Content = "Switch to Intact".ToString();
                cmbDamageCases.Text = "Select Damage Case";
                cmbDamageCases.IsEnabled = true;
                lblCalculationMethod.Content = "Damage";
                lblCalculationMethod.Background = System.Windows.Media.Brushes.Red;
                btnGenerateReport.IsEnabled = false;
                btnSaveLoadingCondition.IsEnabled = false;
                clsGlobVar.DamageCase = "";
                clsGlobVar.FlagDamageCases = true;
                dtIntactCondition = new DataTable();
                dtIntactCondition = clsGlobVar.dtSimulationAllTanks;
                cmbDamageCases.SelectedItem = false;
                RefreshScreen();
                txtLoadingConditionName.Text="--";
                Mouse.OverrideCursor = null;
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }

        }

        private void btnToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                tabLongitudinal.IsEnabled = true;

                btnGenerateReport.IsEnabled = false;
                btnSaveLoadingCondition.IsEnabled = false;
                btnToggle.Content = "Switch to Damage".ToString();
                cmbDamageCases.IsEnabled = false;
                lblCalculationMethod.Content = "Intact";
                lblCalculationMethod.Background = System.Windows.Media.Brushes.LimeGreen;
                clsGlobVar.FlagDamageCases = false;
                clsGlobVar.DamageCase = "---";
                txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                cmbDamageCases.Text = "Select Damage Case";
                //tabItem7.IsEnabled = true;//Sangita
                //tabfloodingpoint.IsEnabled = false;
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                string cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 68";
                for (int i = 0; i <= 67; i++)
                {
                    cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                }
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                Models.TableModel.SimulationModeData();
                dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                RefreshScreen();
                Mouse.OverrideCursor = null;
                CheckNCheckOutCount = 0;
                chk1.IsChecked = false;
                
            }
            catch { Mouse.OverrideCursor = null; }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        DataGridRow dgRow;
        private void dgTanks_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                header = e.Column.Header.ToString();
                if (e.Column.GetType().ToString() == "System.Windows.Controls.DataGridTextColumn")
                {
                    index = e.Row.GetIndex();
                    dgRow = e.Row;
                    TextBlock cbo = (System.Windows.Controls.TextBlock)e.Column.GetCellContent(e.Row);
                    if(index > 41 && clsGlobVar.FlagDamageCases==false)
                    {
                        dgRow.IsEnabled = false;
                    }

                }
                if (e.Column.GetType().ToString() == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    index = e.Row.GetIndex();
                    System.Windows.Controls.ComboBox cbo;
                    cbo = (System.Windows.Controls.ComboBox)e.Column.GetCellContent(e.Row);
                    cbo.SelectionChanged += new SelectionChangedEventHandler(FSMType_SelectionChanged);
                }
            }
            catch
            {

            }
        }
        private void FSMType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                int TankId;
                TankId = Convert.ToInt16((dgTanks.Items[index] as DataRowView)["Tank_ID"]);
                var comboBox = sender as System.Windows.Controls.ComboBox;
                var selectedItem = comboBox.SelectedValue;
                string FSMType = Convert.ToString(selectedItem);
                string query = "update tblFSM_max_act set [max_1_act_0]=" + FSMType.ToString() + " where Tank_ID=" + TankId;
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
            }
            catch
            {
            }

        }
        private void dgTanks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                DrawHatchDeckPlanSelection();
                DrawHatchProfileSelection();
                Mouse.OverrideCursor = null;
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }

        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (lblCalculationMethod.Content.ToString() == "Damage")
                {
                    bool checkBox;

                    if (chk1 == null) { checkBox = false; } else { checkBox = (bool)chk1.IsChecked; }
                    if (CmprItem.Trim() == "Damage Case-01" || CmprItem.Trim() == "Damage Case-02" ||
                       CmprItem.Trim() == "Damage Case-03" || CmprItem.Trim() == "Damage Case-04" ||
                       CmprItem.Trim() == "Damage Case-05" || CmprItem.Trim() == "Damage Case-06" ||
                       CmprItem.Trim() == "Damage Case-07" || CmprItem.Trim() == "Damage Case-08" ||
                       CmprItem.Trim() == "Damage Case-09" || CmprItem.Trim() == "Damage Case-10" ||
                       CmprItem.Trim() == "All Damage Cases")
                    {
                        CalculateStability();
                    }
                    else if (checkBox == true || CheckNCheckOutCount > 0)
                    {
                        CalculateStability();
                        chk1.IsChecked = false;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("At least One tank OR Compartment should damage ");
                    }

                }
                else if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    CalculateStability();

                }
                Mouse.OverrideCursor = null;
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion ControlEvents

        #region UserFunctions
        private void CalculateStability()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                isCalculationRunning = true;
                btnCalculate.IsEnabled = false;
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_Do_Work);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler
                        (bgWorker_ProgressChanged);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (bgWorker_RunWorkerCompleted);
                bgWorker.WorkerReportsProgress = true;

                bgWorker.RunWorkerAsync();
                while (isCalculationRunning)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(1);
                        if (isCalculationRunning == true)
                        {
                            Process();
                        }
                        else
                        { i = 100; }      
                    }
                }
                if (!isCalculationRunning)
                {
                    RefreshScreen();
                    if (ZebecLoadMaster.Models.DAL.clsSqlData.calculationResult == 0)
                    {
                        System.Windows.MessageBox.Show("Calculation terminated");
                        btnGenerateReport.IsEnabled = false;
                    }
                    else
                    {
                        if (_DamageStabilityCnt > 0)
                        {
                            (new System.Threading.Thread(CloseIt)).Start();
                            System.Windows.MessageBox.Show(" Damage Case " + _DamageStabilityCnt + " Stability calculation succeeded");
                          
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Stability calculation succeeded");
                        }
                        btnGenerateReport.IsEnabled = true;
                    }
                }
                pbCalculation.Value = ((DamageAll * 100) / 88);
                btnCalculate.IsEnabled = true;
                btnSaveLoadingCondition.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
            catch
            {
            }
        }

        private void Process()
        {
            pbCalculation.Minimum = 0;
            pbCalculation.Maximum = 70;
            pbCalculation.Value = 0;
            pbCalculation.Foreground = System.Windows.Media.Brushes.Green;
            double value = 0;
            UpdateProgressBarDelegate updatePbDelegate =
                new UpdateProgressBarDelegate(pbCalculation.SetValue);
            do
            {
                value += 1;
                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { System.Windows.Controls.ProgressBar.ValueProperty, value });
            }
            while (pbCalculation.Value != pbCalculation.Maximum);
        }
        public void CloseIt()
        {
            System.Threading.Thread.Sleep(4000);
            System.Windows.Forms.SendKeys.SendWait(" ");
        }
        private void Tank_PercentFill()
        {
            try
            {
                clsGlobVar.Tank1_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[0]["Percent_Full"]);
                clsGlobVar.Tank2_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[1]["Percent_Full"]);
                clsGlobVar.Tank3_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[2]["Percent_Full"]);
                clsGlobVar.Tank4_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[3]["Percent_Full"]);
                clsGlobVar.Tank5_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[4]["Percent_Full"]);
                clsGlobVar.Tank6_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[5]["Percent_Full"]);
                clsGlobVar.Tank7_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[6]["Percent_Full"]);
                clsGlobVar.Tank8_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[7]["Percent_Full"]);
                clsGlobVar.Tank9_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[8]["Percent_Full"]);
                clsGlobVar.Tank10_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[9]["Percent_Full"]);
                clsGlobVar.Tank11_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[10]["Percent_Full"]);
                clsGlobVar.Tank12_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[11]["Percent_Full"]);
                clsGlobVar.Tank13_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[12]["Percent_Full"]);
                clsGlobVar.Tank14_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[13]["Percent_Full"]);
                clsGlobVar.Tank15_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[14]["Percent_Full"]);
                clsGlobVar.Tank16_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[15]["Percent_Full"]);
                clsGlobVar.Tank17_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[16]["Percent_Full"]);
                clsGlobVar.Tank18_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[17]["Percent_Full"]);
                clsGlobVar.Tank19_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[18]["Percent_Full"]);
                clsGlobVar.Tank20_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[19]["Percent_Full"]);
                clsGlobVar.Tank21_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[20]["Percent_Full"]);
                clsGlobVar.Tank22_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[21]["Percent_Full"]);
                clsGlobVar.Tank23_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[22]["Percent_Full"]);
                clsGlobVar.Tank24_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[23]["Percent_Full"]);
                clsGlobVar.Tank25_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[24]["Percent_Full"]);
                clsGlobVar.Tank26_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[25]["Percent_Full"]);
                clsGlobVar.Tank27_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[26]["Percent_Full"]);
                clsGlobVar.Tank28_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[27]["Percent_Full"]);
                clsGlobVar.Tank29_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[28]["Percent_Full"]);
                clsGlobVar.Tank30_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[29]["Percent_Full"]);
                clsGlobVar.Tank31_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[30]["Percent_Full"]);
                clsGlobVar.Tank32_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[31]["Percent_Full"]);
                clsGlobVar.Tank33_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[32]["Percent_Full"]);
                clsGlobVar.Tank34_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[33]["Percent_Full"]);
                clsGlobVar.Tank35_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[34]["Percent_Full"]);
                clsGlobVar.Tank36_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[35]["Percent_Full"]);
                clsGlobVar.Tank37_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[36]["Percent_Full"]);
                clsGlobVar.Tank38_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[37]["Percent_Full"]);
                clsGlobVar.Tank39_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[38]["Percent_Full"]);
                clsGlobVar.Tank40_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[39]["Percent_Full"]);
                clsGlobVar.Tank41_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[40]["Percent_Full"]);
                clsGlobVar.Tank42_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[41]["Percent_Full"]);
                clsGlobVar.Tank43_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[42]["Percent_Full"]);
                clsGlobVar.Tank44_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[43]["Percent_Full"]);
                clsGlobVar.Tank45_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[44]["Percent_Full"]);
                clsGlobVar.Tank46_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[45]["Percent_Full"]);
                clsGlobVar.Tank47_PercentFill = Convert.ToDecimal(clsGlobVar.dtSimulationAllTanks.Rows[46]["Percent_Full"]);
            }
            catch
            {

            }
        }
        private void RefreshScreen()
        {
            try
            {
                tabLongitudinal.IsSelected = true;
                isHatchSelectionProfile = false;
                tabLongitudinal.Focus();
                zedGraph.Refresh();
                zedGraph.GraphPane.CurveList.Clear();
                zedGraph.GraphPane.GraphObjList.Clear();
                dt1StabilityStatus.IsEnabled = false;
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    tabfloodingpoint.IsEnabled = false;
                    SFBMCondition.IsEnabled = true;
                    tabLongitudinal.IsEnabled = true;
                    dgTanks.Columns[0].Visibility = Visibility.Hidden;
                    tabItem7.IsEnabled = true;
                    btnAldamage.IsEnabled = false;
                    lblLongitudinalStrength.IsEnabled = true;
                    lblLongitudinalStatus.Content = "OK";
                    lblLongitudinalStatus.Background = System.Windows.Media.Brushes.LimeGreen;
                    DrawLongitudinalGraph();
                }
                else
                {
                    tabfloodingpoint.IsEnabled = true;
                    SFBMCondition.IsEnabled = false;
                    tabLongitudinal.IsEnabled = true;
                    btnAldamage.IsEnabled = true;
                    tabItem7.IsEnabled = false;
                    dgTanks.Columns[0].Visibility = Visibility.Visible;
                    lblLongitudinalStrength.IsEnabled = true;
                    lblLongitudinalStatus.Content = "OK";
                    lblLongitudinalStatus.Background = System.Windows.Media.Brushes.LimeGreen;

                }

                canvasTwoD.Children.Clear();
                ViewModels.CadViewModel.Cad2dModels();
                Models.TableModel.SimulationModeData();
                Tank_PercentFill();
                Model(Models.clsGlobVar.CentrelineProfile);

                dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanks.DefaultView;
            
                dgFixedLoad.ItemsSource = clsGlobVar.dtSimulationVariableItems.DefaultView;               
                tabItem1.IsSelected = true;
                tabItem4.IsSelected = true;
                tabItem1.Focus();
                DrawGZGraph();
                StabilityCriteria();
                LebelUpdate();
                AddHatchProfile();
                AddHatchDeckPlan();
                canvasTwoD.Arrange(new Rect(0, 0, canvasTwoD.RenderSize.Width, canvasTwoD.RenderSize.Height));
                canvasTwoD.InvalidateVisual();
                CreateBitmapFromVisual(canvasTwoD, System.Windows.Forms.Application.StartupPath + "\\Images\\Img.bmp");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }

        private void DrawGZGraph()
        {
            try
            {
                myGraph = zedGraph.GraphPane;
                myGraph.XAxis.Title.Text = "Heel (degree)";
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    myGraph.Title.Text = "GZ and Lever Arm Curves";
                    myGraph.YAxis.Title.Text = "GZ and Lever Arm (m)";
                    myGraph.XAxis.Scale.Min = -30;
                    myGraph.XAxis.Scale.Max = 90;
                }
                else if (lblCalculationMethod.Content.ToString() == "Damage")
                {
                    myGraph.Title.Text = " GZ ";
                    myGraph.YAxis.Title.Text = "GZ (m)";
                    myGraph.XAxis.Scale.Min = 0;
                    myGraph.XAxis.Scale.Max = 90;
                }
                zedGraph.ZoomButtons = System.Windows.Forms.MouseButtons.None;
                zedGraph.ZoomButtons = System.Windows.Forms.MouseButtons.None;
                zedGraph.ZoomStepFraction = 0;
                zedGraph.AxisChange();
                zedGraph.Invalidate();

                DataSet dsGZData = new DataSet();
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                string cmd = "SELECT heelAng,heelGZ from GZDataSimulationMode_New where [User] = 'dbo'";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsGZData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtGZgraph = new DataTable();
                dtGZgraph = dsGZData.Tables[0];
                dgGZData.ItemsSource = dtGZgraph.DefaultView;

                myGraph.YAxis.Scale.Min = dtGZgraph.AsEnumerable().Min(row => Convert.ToInt32(row["heelGZ"])) - 0.5;
                myGraph.YAxis.Scale.Max = dtGZgraph.AsEnumerable().Max(row => Convert.ToInt32(row["heelGZ"])) + 0.5;

                foreach (DataColumn dc in dtGZgraph.Columns)
                {
                    foreach (DataRow dtrow in dtGZgraph.Rows)
                    {
                        totalRow = dtGZgraph.Rows.Count;
                        x = new double[totalRow];
                        y1 = new double[totalRow];

                        DataColumn colx1 = dtGZgraph.Columns[0];
                        DataColumn coly1 = dtGZgraph.Columns[1];

                        for (int index = 0; index < dtGZgraph.Rows.Count; index++)
                        {
                            x[index] = (float)Convert.ToDouble(dtGZgraph.Rows[index].ItemArray[0]);
                            y1[index] = (float)Convert.ToDouble(dtGZgraph.Rows[index].ItemArray[1]);

                        }

                    }
                }
                PointPairList aList = new PointPairList();
                aList.Add(x, y1);


                LineItem myCurve = myGraph.AddCurve("Smooth", aList, System.Drawing.Color.Green, SymbolType.None);
                myCurve.Label.Text = "GZ";
                myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve.Symbol.Fill = new Fill(System.Drawing.Color.Green);
                myCurve.Symbol.Size = 4;
                myCurve.Line.Width = 2;
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    DataSet dsHeelingArms = new DataSet();
                    cmd = "SELECT [lw1],[lw2] from [tblWindHeelSimulation] ";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    dsHeelingArms = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                    DataTable dtHeelingArms = new DataTable();
                    dtHeelingArms = dsHeelingArms.Tables[0];
                    double[] xArm = { -30, 90 };
                    double[] yArm = { Convert.ToDouble(dtHeelingArms.Rows[0][1]), Convert.ToDouble(dtHeelingArms.Rows[0][1]) };
                    myCurve = myGraph.AddCurve("lw2 " + Convert.ToDouble(dtHeelingArms.Rows[0][1]).ToString(), xArm, yArm, System.Drawing.Color.DarkMagenta, SymbolType.None);
                    myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                    myCurve.Line.DashOff = 2f;
                    myCurve.Line.DashOn = 2f;

                    double[] xArm2 = { -30, 90 };
                    double[] yArm2 = { Convert.ToDouble(dtHeelingArms.Rows[0][0]), Convert.ToDouble(dtHeelingArms.Rows[0][0]) };
                    myCurve = myGraph.AddCurve("lw1 " + Convert.ToDouble(dtHeelingArms.Rows[0][0]).ToString(), xArm2, yArm2, System.Drawing.Color.DarkBlue, SymbolType.None);
                    myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                    myCurve.Line.DashOff = 2f;
                    myCurve.Line.DashOn = 2f;
                }
                DataSet dsDownFlooding = new DataSet();
                cmd = "SELECT [Angle] from [tblMinimum_DF_Angle] ";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsDownFlooding = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtDownFlooding = new DataTable();
                dtDownFlooding = dsDownFlooding.Tables[0];
                double[] xDF = { Convert.ToDouble(dtDownFlooding.Rows[0][0]), Convert.ToDouble(dtDownFlooding.Rows[0][0]) };
                double[] yDF = { 0, 5 };

                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    myCurve = myGraph.AddCurve("Downflooding  " + Math.Round(Convert.ToDouble(dtDownFlooding.Rows[0][0]), 1).ToString() + (char)176, xDF, yDF, System.Drawing.Color.DarkMagenta, SymbolType.None);
                    myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                    myCurve.Line.DashOff = 2f;
                    myCurve.Line.DashOn = 2f;
                    myCurve.Line.Width = 2;
                }
                else
                {
                    //  myCurve = myGraph.AddCurve("Critical Point  " + Convert.ToDouble(dtDownFlooding.Rows[0][0]).ToString() + (char)176, xDF, yDF, System.Drawing.Color.DarkMagenta, SymbolType.None);
                }
                //myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                //myCurve.Line.DashOff = 2f;
                //myCurve.Line.DashOn = 2f;
                //myCurve.Line.Width = 2;

                DataSet dsEquillibriumAngle = new DataSet();
                cmd = "SELECT [Actual_Value] from [tblSimulationMode_Stability_Actual_Criteria_Calc] where [Stability_Summary_ID]=7 and [User]='dbo'";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsEquillibriumAngle = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtEquillibriumAngle = new DataTable();
                dtEquillibriumAngle = dsEquillibriumAngle.Tables[0];

                double xArmE = Convert.ToDouble(dtEquillibriumAngle.Rows[0][0]);
                //// Hatching Code Start///
                //PointD[] pointD = new PointD[25];
                //int maxIndex = 0;
                //for (int index = 7; index < dtGZgraph.Rows.Count; index++)
                //{
                //    if (x[index] < Convert.ToDouble(dtDownFlooding.Rows[0][0]))
                //    {
                //        pointD[index - 6] = new PointD(x[index], y1[index]);
                //        maxIndex = index - 6;
                //    }

                //}
                //pointD[maxIndex + 1] = new PointD(Convert.ToDouble(dtDownFlooding.Rows[0][0]), Convert.ToDouble(dtHeelingArms.Rows[0][1]));

                //var poly1 = new ZedGraph.PolyObj
                //{
                //    Points = pointD,
                //    Fill = new ZedGraph.Fill(System.Drawing.Color.PowderBlue),
                //    ZOrder = ZedGraph.ZOrder.E_BehindCurves
                //};
                //zedGraph.GraphPane.GraphObjList.Add(poly1);
                ///Hatching Code End
                zedGraph.AxisChange();
                zedGraph.Invalidate();
                double[] xb = { 0, 0 };
                double[] yb = { -5, 5 };

                double[] xb1 = { 0, 0 };
                double[] yb1 = { -0, 0 };
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    myCurve = myGraph.AddCurve("", xb, yb, System.Drawing.Color.Black, SymbolType.None);
                }
                else
                {
                    myCurve = myGraph.AddCurve("", xb1, yb1, System.Drawing.Color.Black, SymbolType.None);
                }


                myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve.Line.DashOff = 4f;
                myCurve.Line.DashOn = 4f;
                myCurve.YAxisIndex = 1;
                myGraph.YAxis.AxisGap = 0.2f;
                myGraph.YAxis.Title.FontSpec.Size = 15f;
                myGraph.YAxis.Scale.FontSpec.Size = 15f;
                myGraph.Y2Axis.Title.FontSpec.Size = 15f;
                myGraph.Y2Axis.Scale.FontSpec.Size = 15f;
                myGraph.XAxis.Title.FontSpec.Size = 15f;
                myGraph.XAxis.Scale.FontSpec.Size = 15f;
                myGraph.Legend.FontSpec.Size = 13f;
                myGraph.YAxis.Title.FontSpec.FontColor = System.Drawing.Color.Green;
                myGraph.YAxis.Scale.FontSpec.FontColor = System.Drawing.Color.Green;
                myGraph.Legend.Gap = 1f;
                zedGraph.ZoomStepFraction = 0;
                zedGraph.AxisChange();
                zedGraph.Invalidate();

                using (var bmp = new System.Drawing.Bitmap(zedGraph.Width, zedGraph.Height))
                {
                    zedGraph.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(System.Windows.Forms.Application.StartupPath + "\\Images\\GZ_curve.png");

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        
        private void DrawLongitudinalGraph()
        {
            try
            {  ////////////////////////longitudinal data/////////////
                DataSet dsLongitudinalData = new DataSet();
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                string cmd = "SELECT [Frame],[SF],[BM] from [tbl_SimulationMode_SFAndBM_New] where [User] = 'dbo'";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsLongitudinalData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtLongitudinal = new DataTable();

                DataSet seaOrPort = new DataSet();
                cmd = "SELECT [Sea_1_Port_0] from [tblMaster_Config_Addi]";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                seaOrPort = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable ds = new DataTable();
                ds = seaOrPort.Tables[0];
                int gg = 3;
                if (ds.Rows.Count > 0)
                {
                    gg = Convert.ToInt16(ds.Rows[0][0]);
                }
                DataSet dsLogitudinalNew = new DataSet();
                if (gg == 1)
                { cmd = "SELECT Max_BM_Sea,[Min_BM_Sea],[Max_SF_Sea], [Min_SF_Sea] from [tbl_SimulationMode_SFAndBM_Permissible_Graph] "; }
                else { cmd = "SELECT [Max_BM_Port],[Min_BM_Port],[Max_SF_Port], [Min_SF_Port] from [tbl_SimulationMode_SFAndBM_Permissible_Graph] "; }
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsLogitudinalNew = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtLogitudinalNew = dsLogitudinalNew.Tables[0];

                if (dsLongitudinalData.Tables.Count > 0)
                {
                  if (dsLongitudinalData.Tables[0].Rows.Count > 0)
                    {
                        dtLongitudinal = dsLongitudinalData.Tables[0];
                        foreach (DataRow dr in dtLongitudinal.Rows)
                        {
                            foreach (DataColumn dc in dtLongitudinal.Columns)
                            {
                                if ((dr[dc] == DBNull.Value))
                                {
                                    dr[dc] = 0;

                                }
                            }
                        }
                    }
                    else
                    {
                        zedGraphLongitudinal.GraphPane.CurveList.Clear();
                        zedGraphLongitudinal.GraphPane.GraphObjList.Clear();
                        zedGraphLongitudinal.Refresh();
                    }
                }
                else
                {
                    zedGraphLongitudinal.GraphPane.CurveList.Clear();
                    zedGraphLongitudinal.GraphPane.GraphObjList.Clear();
                    zedGraphLongitudinal.Refresh();
                }

                DataSet dsLongitudinalFrame = new DataSet();
                command = Models.DAL.clsDBUtilityMethods.GetCommand();
                Err = "";
                cmd = "SELECT [Frame],[SF],[SF_Percentage_Diff],[BM],[BM_Percentage_Diff],[Status] from [tbl_SimulationMode_SFAndBM_New] ";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsLongitudinalFrame = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtLongitudinalFrame = new DataTable();
                dtLongitudinalFrame = dsLongitudinalFrame.Tables[0];
                DataTable dtfinal = dtLongitudinalFrame.Clone();
                foreach (DataRow dr in dtLongitudinalFrame.Rows)
                {
                    dtfinal.Rows.Add(dr.ItemArray);
                }
                dtfinal.Columns.Remove("Status");
                dtfinal.Columns.Add("Status", typeof(string));
                for (int i = 0; i < dtLongitudinalFrame.Rows.Count; i++)
                {
                    if (dtLongitudinalFrame.Rows[i]["Status"].ToString() == "1")
                    {
                        dtfinal.Rows[i]["Status"] = "OK";   
                    }
                    else
                    {
                        dtfinal.Rows[i]["Status"] = "NOT OK";
                        lblLongitudinalStatus.Content = "NOT OK";
                        lblLongitudinalStatus.Background = System.Windows.Media.Brushes.Red;
                    }
                }
                dtfinal.Columns["Status"].ReadOnly = true;

                for (int i = 0; i < dtfinal.Rows.Count; i++)
                { dtfinal.Rows[i]["Frame"] = Math.Round(Convert.ToDecimal(dtfinal.Rows[i]["Frame"])); }

                dgLongitudinal.ItemsSource = dtfinal.DefaultView;



                foreach (DataColumn dcLongitudinal in dtLongitudinal.Columns)
                {
                    foreach (DataRow drLongitudinal in dtLongitudinal.Rows)
                    {
                        totalRowLongitudinal = dtLongitudinal.Rows.Count;
                        x1 = new double[totalRowLongitudinal];
                        y1 = new double[totalRowLongitudinal];
                        y2 = new double[totalRowLongitudinal];

                        DataColumn colx1 = dtLongitudinal.Columns[0];
                        DataColumn coly1 = dtLongitudinal.Columns[1];
                        DataColumn coly2 = dtLongitudinal.Columns[2];

                        for (int indexbouyancy = 0; indexbouyancy < dtLongitudinal.Rows.Count; indexbouyancy++)
                        {
                            x1[indexbouyancy] = (float)Convert.ToDouble(dtLongitudinal.Rows[indexbouyancy].ItemArray[0]);
                            y1[indexbouyancy] = (float)Convert.ToDouble(dtLongitudinal.Rows[indexbouyancy].ItemArray[1]);
                            y2[indexbouyancy] = (float)Convert.ToDouble(dtLongitudinal.Rows[indexbouyancy].ItemArray[2]);

                        }
                    }
                }

                foreach (DataColumn dcLongitudinalNew in dtLogitudinalNew.Columns)
                {
                    foreach (DataRow drLongitudinalNew in dtLogitudinalNew.Rows)
                    {
                        int totalRowLongitudinalNew = dtLongitudinal.Rows.Count;
                        y3 = new double[totalRowLongitudinalNew];
                        y4 = new double[totalRowLongitudinalNew];
                        y5 = new double[totalRowLongitudinalNew];
                        y6 = new double[totalRowLongitudinalNew];

                        DataColumn colx1 = dtLogitudinalNew.Columns[0];
                        DataColumn coly1 = dtLogitudinalNew.Columns[1];
                        DataColumn coly2 = dtLogitudinalNew.Columns[2];
                        DataColumn coly3 = dtLogitudinalNew.Columns[3];

                        for (int indexbouyancy = 0; indexbouyancy < dtLogitudinalNew.Rows.Count; indexbouyancy++)
                        {
                            y3[indexbouyancy] = (float)Convert.ToDouble(dtLogitudinalNew.Rows[indexbouyancy].ItemArray[0]);
                            y4[indexbouyancy] = (float)Convert.ToDouble(dtLogitudinalNew.Rows[indexbouyancy].ItemArray[1]);
                            y5[indexbouyancy] = (float)Convert.ToDouble(dtLogitudinalNew.Rows[indexbouyancy].ItemArray[2]);
                            y6[indexbouyancy] = (float)Convert.ToDouble(dtLogitudinalNew.Rows[indexbouyancy].ItemArray[3]);

                        }
                    }
                }

                //................changees@250116 2:41.....
                GraphPane myPane7 = zedGraphLongitudinal.GraphPane;
                myPane7.Title.Text = "Longitudinal Strength Curve";
                myPane7.XAxis.Title.Text = "Distance From Origin (m)";
                myPane7.XAxis.Scale.Min = -5;
                myPane7.XAxis.Scale.Max = 105;
                // Make up some data points based on the Sine function
                PointPairList vList = new PointPairList();
                PointPairList aList = new PointPairList();
                PointPairList bmMaxList = new PointPairList();
                PointPairList bmMinList = new PointPairList();
                PointPairList sfMaxList = new PointPairList();
                PointPairList sfMinList = new PointPairList();
                aList.Add(x1, y1);
                vList.Add(x1, y2);
                bmMaxList.Add(x1, y3);
                bmMinList.Add(x1, y4);
                sfMaxList.Add(x1, y5);
                sfMinList.Add(x1, y6);

                zedGraphLongitudinal.GraphPane.CurveList.Clear();
                zedGraphLongitudinal.GraphPane.GraphObjList.Clear();

                LineItem myCurve1 = myPane7.AddCurve("Shear Force", aList, System.Drawing.Color.Blue, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve1.Line.Width = 1.8f;
                myCurve1 = myPane7.AddCurve("Bending Moment", vList, System.Drawing.Color.Red, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve1.IsY2Axis = true;
                myCurve1.Line.Width = 1.8f;
                myCurve1 = myPane7.AddCurve("Permissible  BM", bmMaxList, System.Drawing.Color.Pink, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve1.IsY2Axis = true;
                myCurve1.Line.Width = 2.8f;
                myCurve1 = myPane7.AddCurve("Permissible  SF", sfMaxList, System.Drawing.Color.Green, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                //myCurve1.IsY2Axis = true;
                myCurve1.Line.Width = 2.5f;
                myCurve1 = myPane7.AddCurve("Permissible  BM", bmMinList, System.Drawing.Color.Pink, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve1.IsY2Axis = true;
                myCurve1.Line.Width = 2.8f;
                myCurve1 = myPane7.AddCurve("Permissible  SF", sfMinList, System.Drawing.Color.Green, SymbolType.None);
                myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                myCurve1.Line.Width = 2.5f;
            
                myPane7.XAxis.MajorGrid.IsVisible = true;
                myPane7.YAxis.Title.Text = "Shear Force (T)";
                //myPane7.Y2Axis.Title.Text = "Net Load (T/m)";
                myPane7.YAxis.AxisGap = 0.2f;
                myPane7.YAxis.Title.FontSpec.Size = 15f;
                myPane7.YAxis.Scale.FontSpec.Size = 15f;
                myPane7.Y2Axis.Title.FontSpec.Size = 15f;
                myPane7.Y2Axis.Scale.FontSpec.Size = 15f;
                myPane7.XAxis.Title.FontSpec.Size = 15f;
                myPane7.XAxis.Scale.FontSpec.Size = 15f;
                // Make the Y axis scale BLue
                myPane7.YAxis.Scale.FontSpec.FontColor = System.Drawing.Color.Blue;
                myPane7.YAxis.Title.FontSpec.FontColor = System.Drawing.Color.Blue;
                // turn off the opposite tics so the Y tics don't show up on the Y2 axis
                myPane7.YAxis.MajorTic.IsOpposite = false;
                myPane7.YAxis.MinorTic.IsOpposite = false;
                // Don't display the Y zero line
                myPane7.YAxis.MajorGrid.IsZeroLine = false;
                // Align the Y axis labels so they are flush to the axis
                myPane7.YAxis.Scale.Align = AlignP.Inside;

                // myPane7.YAxis.Scale.Max = Math.Abs(Convert.ToDouble(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_SF"])) + 100;
                myPane7.YAxis.Scale.Max = 2700;
                // myPane7.YAxis.Scale.Min = -Math.Abs(Convert.ToDouble(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_SF"]))-100;
                myPane7.YAxis.Scale.Min = -2700;

                myPane7.Y2Axis.Title.Text = "Bending Moment(T-m)";
                //myPane7.YAxis.Scale.Max = 150;
                //myPane7.YAxis.Scale.Min = -150;
                // Enable the Y2 axis display
                myPane7.Y2Axis.IsVisible = true;
                // Make the Y2 axis scale blue
                myPane7.Y2Axis.Scale.FontSpec.FontColor = System.Drawing.Color.Red;
                myPane7.Y2Axis.Title.FontSpec.FontColor = System.Drawing.Color.Red;
                // turn off the opposite tics so the Y2 tics don't show up on the Y
                //axis
                myPane7.Y2Axis.MajorTic.IsOpposite = false;
                myPane7.Y2Axis.MinorTic.IsOpposite = false;
                // Display the Y2 axis grid lines
                myPane7.Y2Axis.MajorGrid.IsVisible = true;
                // Align the Y2 axis labels so they are flush to the axis
                myPane7.Y2Axis.Scale.Align = AlignP.Inside;
                //myPane7.Y2Axis.Scale.Min = -Math.Abs(Convert.ToDouble(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_BM"]))-1000;
                //myPane7.Y2Axis.Scale.Max = Math.Abs(Convert.ToDouble(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_BM"])) + 1000;

                myPane7.Y2Axis.Scale.Max = 23000;
                myPane7.Y2Axis.Scale.Min = -23000;

                myPane7.Legend.FontSpec.Size = 15f;
                zedGraphLongitudinal.ZoomButtons = MouseButtons.None;
                zedGraphLongitudinal.ZoomButtons = MouseButtons.None;
                zedGraphLongitudinal.ZoomStepFraction = 0;

                zedGraphLongitudinal.AxisChange();
                zedGraphLongitudinal.Invalidate();

                using (var bmp = new System.Drawing.Bitmap(zedGraphLongitudinal.Width, zedGraphLongitudinal.Height))
                {
                    zedGraphLongitudinal.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(System.Windows.Forms.Application.StartupPath + "\\Images\\Longitudinal_curve.png");
                }
            }
            catch
            {
                //System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }
        private void StabilityCriteria()
        {
            try
            {
                DataTable dtfinal = new DataTable();
                //Flooding Point Start....................
                dgFloodingPoint.ItemsSource = clsGlobVar.FloodingPoint_Damage.DefaultView;

                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    dtfinal = clsGlobVar.dtSimulationStabilityCriteriaIntact.Clone();
                    foreach (DataRow dr in clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows)
                    {

                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));


                    for (int i = 0; i < clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows.Count; i++)
                    {
                        if (clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows[i]["Status"].ToString() == "True")
                        {
                            dtfinal.Rows[i]["Status"] = "Pass";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "Fail";
                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;

                }
                else
                {
                    dtfinal = clsGlobVar.dtSimulationStabilityCriteriaDamage.Clone();
                    foreach (DataRow dr in clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows)
                    {

                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));


                    for (int i = 0; i < clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows.Count; i++)
                    {
                        if (clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows[i]["Status"].ToString() == "True")
                        {
                            dtfinal.Rows[i]["Status"] = "Pass";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "Fail";

                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;
                }


                dgStabilityCriteria.ItemsSource = dtfinal.DefaultView;
            }
            catch
            {
            }
        }

        private void MaxVolume()
        {
            try
            {

              maxVolume = new Dictionary<int, decimal>();
              maxVolume.Add(1,591.511m);
              maxVolume.Add(2,591.511m);
              maxVolume.Add(3,650.975m);
              maxVolume.Add(4,650.975m);
              maxVolume.Add(5,623.884m);
              maxVolume.Add(6,623.884m);
              maxVolume.Add(7,641.29m);
              maxVolume.Add(8,641.29m);
              maxVolume.Add(9,80.966m);
              maxVolume.Add(10,80.966m);
              maxVolume.Add(11,60.526m);
              maxVolume.Add(12, 60.526m);
              maxVolume.Add(13,34.606m);
              maxVolume.Add(14,59.211m);
              maxVolume.Add(15,4.798m);
              maxVolume.Add(16,4.798m);
              maxVolume.Add(17,15.313m);
              maxVolume.Add(18,14.844m);
              maxVolume.Add(19,3.998m);
              maxVolume.Add(20,3.925m);
              maxVolume.Add(21,3.228m);
              maxVolume.Add(22,3.228m);
              maxVolume.Add(23,42.249m);
              maxVolume.Add(24,42.249m);
              maxVolume.Add(25,258.817m);
              maxVolume.Add(26,246.221m);
              maxVolume.Add(27,204.025m);
              maxVolume.Add(28,214.368m);
              maxVolume.Add(29,205.648m);
              maxVolume.Add(30,195.736m);
              maxVolume.Add(31,201.752m);
              maxVolume.Add(32,212.095m);
              maxVolume.Add(33,34.783m);
              maxVolume.Add(34,37.173m);
              maxVolume.Add(35,153.266m);
              maxVolume.Add(36,3.745m);
              maxVolume.Add(37,2.405m);
              maxVolume.Add(38,11.284m);
              maxVolume.Add(39,1.78m);
              maxVolume.Add(40,4.349m);
              maxVolume.Add(41,28.684m);
              maxVolume.Add(42,874.912m);
              maxVolume.Add(43,63.73m);
              maxVolume.Add(44,8.345m);
              maxVolume.Add(45,8.345m);
              maxVolume.Add(46,200.2m);

            }
            catch
            {
            }
        }

        private void DamageCases()
        {
            _DamageCases[0, 0] = Convert.ToString(1);
            _DamageCases[1, 0] = Convert.ToString(1);
        }
        private void LebelUpdate()
        {
            try
            {
                /////Front Values
                flagDraft++;
                DataTable dtHydrostatics = new DataTable();
                dtHydrostatics = clsGlobVar.dtSimulationHydrostatics;
                lblDisplacement.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Displacement"]), 2).ToString();
                lblDeadWeight.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Deadweight"]), 2).ToString();
                lblTrim.Content = Math.Abs(Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["TRIM"]), 2)).ToString();
                if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["TRIM"]), 2) > 0)
                {
                    lblTrimStatus.Content = "Trim (AFT)";
                }
                else
                {
                    lblTrimStatus.Content = "Trim (FWD)";
                }
                lblHeel.Content = Math.Abs(Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Heel"]), 2)).ToString();
                if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Heel"]), 2) > 0)
                {
                    lblHeelStatus.Content = "Heel (PORT)";
                }
                else
                {
                    lblHeelStatus.Content = "Heel (STBD)";
                }
                lblPropeller.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Prop_Immersion"]), 2).ToString();
                if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Prop_Immersion"]), 2) < 100)
                {
                    lblPropeller.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    lblPropeller.Background = System.Windows.Media.Brushes.Transparent;
                    lblPropeller.Foreground = System.Windows.Media.Brushes.Black;
                }
                lblGMt.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["GMT"]), 2).ToString();
                //lblRollingPeriod.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Rolling_Period"]), 2).ToString();

                if (dtHydrostatics.Rows[0]["Visibility"] == DBNull.Value)
                {
                    lblBlindZone.Content = 0;
                    lblBlindZone.Background = System.Windows.Media.Brushes.Transparent;
                    lblBlindZone.Foreground = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    lblBlindZone.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Visibility"]), 2).ToString();
                    if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Visibility"]), 2) > 216)
                    {
                        lblBlindZone.Foreground = System.Windows.Media.Brushes.Red;
                    }
                    else
                    {
                        lblBlindZone.Background = System.Windows.Media.Brushes.Transparent;
                        lblBlindZone.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
                lblDraftAP.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Draft_AP"]), 2).ToString();
                lblDraftFP.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Draft_FP"]), 2).ToString();
                DraftsMid = Math.Round(Convert.ToDouble(dtHydrostatics.Rows[0]["Draft_MID"]), 3);

             
                if ((DraftsMid > 7.235) && (flagDraft > 1) &&(lblCalculationMethod.Content.ToString() == "Intact"))
                {
                    lblDraftMid.Content = DraftsMid;
                    lblDraftMid.Foreground = System.Windows.Media.Brushes.Red;
                    System.Windows.MessageBox.Show("Draft Exceeds Design Drafts");
                }
                else
                {
                    lblDraftMid.Content = DraftsMid;
                    lblDraftMid.Foreground = System.Windows.Media.Brushes.Black;
                }

                                lblDraftAftMark.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Draft_AFT_MARK"]), 2).ToString();
                lblDraftFwdMark.Content = Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Draft_FWD_MARK"]), 2).ToString();

                //Inside TabControl
                lblHydDisplacement.Content = dtHydrostatics.Rows[0]["Displacement"].ToString();
                lblHydTrim.Content = Math.Abs(Convert.ToDecimal(dtHydrostatics.Rows[0]["TRIM"])).ToString();
                if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["TRIM"]), 2) > 0)
                {
                    lblHydroTrimStatus.Content = "Trim (AFT)";
                }
                else
                {
                    lblHydroTrimStatus.Content = "Trim (FWD)";

                }
                lblHydHeel.Content = Math.Abs(Convert.ToDecimal(dtHydrostatics.Rows[0]["Heel"])).ToString();
                if (Math.Round(Convert.ToDecimal(dtHydrostatics.Rows[0]["Heel"]), 2) > 0)
                {
                    lblHydroHeelStatus.Content = "Heel (PORT)";
                }
                else
                {
                    lblHydroHeelStatus.Content = "Heel (STBD)";
                }
                lblHydKGSolid.Content = dtHydrostatics.Rows[0]["KG(Solid)"].ToString();
                lblHydKGLiquid.Content = dtHydrostatics.Rows[0]["KG(Fluid)"].ToString();
                lblHydGMT.Content = dtHydrostatics.Rows[0]["GMT"].ToString();
                lblHydFSC.Content = dtHydrostatics.Rows[0]["FSC"].ToString();
                lblHydLCG.Content = dtHydrostatics.Rows[0]["LCG"].ToString();
                lblHydLCF.Content = dtHydrostatics.Rows[0]["LCF"].ToString();
                lblHydTPC.Content = dtHydrostatics.Rows[0]["TPC"].ToString();
                lblHydMCT.Content = dtHydrostatics.Rows[0]["MCT"].ToString();
                lblHydRollingPeriod.Content = dtHydrostatics.Rows[0]["Rolling_Period"].ToString();
                lblHydDraftAP.Content = dtHydrostatics.Rows[0]["Draft_AP"].ToString();
                lblHydDraftFP.Content = dtHydrostatics.Rows[0]["Draft_FP"].ToString();
                lblHydDraftMidship.Content = dtHydrostatics.Rows[0]["Draft_MID"].ToString();
                lblHydDraftAftMark.Content = dtHydrostatics.Rows[0]["Draft_AFT_MARK"].ToString();
                lblHydDraftForwardMark.Content = dtHydrostatics.Rows[0]["Draft_FWD_MARK"].ToString();

                //Dead Weight Details
                lblCargoOil.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[0].Rows[0][0].ToString();
                lblFuelOil.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[1].Rows[0][0].ToString();
                lblDieselOil.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[2].Rows[0][0].ToString();
                lblLubeOil.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[3].Rows[0][0].ToString();
                lblFreshWater.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[4].Rows[0][0].ToString();
                lblBallastWater.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[5].Rows[0][0].ToString();
                lblOther.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[6].Rows[0][0].ToString();
                // lblSlope.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[7].Rows[0][0].ToString();
                lblFixedLoads.Content = clsGlobVar.dsSimulationDeadWeightDetails.Tables[7].Rows[0][0].ToString();


                lblStatus.Content = Models.clsGlobVar.dtSimulationStabilitySummary.Rows[0][0].ToString();
                if (lblStatus.Content.ToString() == "OK")
                {
                    lblStatus.Background = System.Windows.Media.Brushes.LimeGreen;
                    //lblStatus.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                }
                else
                {
                    lblStatus.Background = System.Windows.Media.Brushes.Red;
                }

                if (DamageAll > 0)
                {
                    // int jjk = 0;

                    for (int jj = jjk; jj < jjk + 1; jj++)
                    {
                        _statusOKnNotOK[jj, 0] = Convert.ToString(jj + 1);
                        _statusOKnNotOK[jj, 1] = Models.clsGlobVar.dtSimulationStabilitySummary.Rows[0][0].ToString();
                    }
                    jjk++;
                    if (DamageAll >= 36) { jjk = 0; }
                }

                /////SF and BM Maximum Values
                lblBMMaxValue.Content = Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_BM"].ToString();
                lblSFMaxValue.Content = Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Max_SF"].ToString();

                lblBMMaxDistance.Content = Math.Round(Convert.ToDecimal(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Distance_BM"].ToString()));
                lblSFMaxDistance.Content = Math.Round(Convert.ToDecimal(Models.clsGlobVar.dtSimulationSFBMMax.Rows[0]["Distance_SF"].ToString()));
            }
            catch
            {
            }
        }
        #region PDFWriter
        static String ISO_Date()
        {
            return DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss");
        }
        public void printToPdf()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Document doc = new Document(iTextSharp.text.PageSize.A4, 30, 30, 30, 20);

                if (DamageAll == 0)
                {

                    reportPath = System.Windows.Forms.Application.StartupPath + "\\Reports\\" + ISO_Date() + "_Report.pdf";
                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(reportPath, FileMode.Create));
                    doc.Open();//Open Document to write
                    wri.PageEvent = new pdfFormating();
                }

                else if (DamageAll != 0)
                {
                    reportPath = System.Windows.Forms.Application.StartupPath + "\\Reports\\AllDamageCases\\" + ISO_Date() + "_Report.pdf";
                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(reportPath, FileMode.Create));
                    doc.Open();//Open Document to write
                    wri.PageEvent = new pdfFormating();
                }
                if (DamageAll == 0 || DamageAll == 1)
                {
                    iTextSharp.text.Paragraph projecttitl = new iTextSharp.text.Paragraph("FAIRYTALE :" + lblCalculationMethod.Content.ToString() + " Loading Condition ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD)); // Loading Summary Table Name
                    projecttitl.Alignment = Element.ALIGN_CENTER;
                    doc.Add(projecttitl);

                    iTextSharp.text.Paragraph _ProjectNameNVersion = new iTextSharp.text.Paragraph("Zebec LoadMatser, V-01", FontFactory.GetFont(FontFactory.TIMES, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.GRAY)); // Loading Summary Table Name
                    _ProjectNameNVersion.Alignment = Element.ALIGN_RIGHT;
                    _ProjectNameNVersion.IndentationRight = 10;

                    doc.Add(_ProjectNameNVersion);
                }

                string Damagecases = txtLoadingConditionName.ToString();
                string finalDamage = Damagecases.Split(':')[1];
                iTextSharp.text.Paragraph DamageCase = new iTextSharp.text.Paragraph(finalDamage, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.BOLD));
                DamageCase.Alignment = Element.ALIGN_CENTER;
                doc.Add(DamageCase);
                //...........StartOFLogo.........................................
                if (DamageAll == 0 || DamageAll == 1)
                {
                    iTextSharp.text.Image LogoWatermark = iTextSharp.text.Image.GetInstance(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\Watermark.jpg");
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(LogoWatermark);
                    pic.Alignment = Element.ALIGN_LEFT;
                    pic.ScaleToFit(70, 50);
                    doc.Add(pic);
                    iTextSharp.text.Image logoMdl = iTextSharp.text.Image.GetInstance(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\Client_Logo.PNG");
                    iTextSharp.text.Image pic1 = iTextSharp.text.Image.GetInstance(logoMdl);
                    pic1.Alignment = Element.ALIGN_RIGHT;
                    pic1.ScaleToFit(70, 50);
                    pic1.SetAbsolutePosition(485, 732);
                    doc.Add(pic1); 
                }

                if (DamageAll > 1)
                {
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                }

                iTextSharp.text.Image imgCad = iTextSharp.text.Image.GetInstance(System.Windows.Forms.Application.StartupPath + "\\Images\\Img.bmp");
                imgCad.Alignment = Element.ALIGN_CENTER;
                imgCad.ScaleToFit(450, 450);
                doc.Add(imgCad);

                if (DamageAll > 1)
                {
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                }

                iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph("Loading Summary", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // Loading Summary Table Name
                p3.Alignment = Element.ALIGN_CENTER;
                doc.Add(p3);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                iTextSharp.text.Font fntHeader = FontFactory.GetFont("Times New Roman", 8, iTextSharp.text.Color.BLUE);   //Header
                iTextSharp.text.Font fntBody = FontFactory.GetFont("Times New Roman", 6.5f);   // Body
                //..........StartofLoadingSummaryPart1..............................
                PdfPTable tblLoadingSummary = new PdfPTable(10);
                tblLoadingSummary.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                tblLoadingSummary.WidthPercentage = 90;
                float[] widthsLoading = new float[] { 2.5f, 1.8f, 1.8f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.5f, 1f };      //in case nishant decide to incorporate again add 2nd and 3rd column as 1f  
                tblLoadingSummary.SetWidths(widthsLoading);
                tblLoadingSummary.HorizontalAlignment = Element.ALIGN_CENTER;
                iTextSharp.text.Font fntRedColor = FontFactory.GetFont("Times New Roman", 1, iTextSharp.text.Color.ORANGE);// Body

                PdfPCell tankName = new PdfPCell(new Phrase("Tank Name", fntHeader));
                tankName.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPCell tankStatus = new PdfPCell(new Phrase("Status", fntHeader));
                tankStatus.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell percentFull = new PdfPCell(new Phrase("Percent Fill", fntHeader));
                percentFull.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell Volume = new PdfPCell(new Phrase("Volume(cu.m)", fntHeader));
                Volume.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell sg = new PdfPCell(new Phrase("SG", fntHeader));
                sg.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell weight = new PdfPCell(new Phrase("Weight(T)", fntHeader));
                weight.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell lcg = new PdfPCell(new Phrase("LCG(m)", fntHeader));
                lcg.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell tcg = new PdfPCell(new Phrase("TCG(m)", fntHeader));
                tcg.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell vcg = new PdfPCell(new Phrase("VCG(m)", fntHeader));
                vcg.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell fsm = new PdfPCell(new Phrase("FSM(T-m)", fntHeader));
                fsm.HorizontalAlignment = Element.ALIGN_CENTER;
                //Add header to table
                tblLoadingSummary.AddCell(tankName);
                tblLoadingSummary.AddCell(percentFull);
                tblLoadingSummary.AddCell(Volume);
                tblLoadingSummary.AddCell(sg);
                tblLoadingSummary.AddCell(weight);
                tblLoadingSummary.AddCell(lcg);
                tblLoadingSummary.AddCell(tcg);
                tblLoadingSummary.AddCell(vcg);
                tblLoadingSummary.AddCell(fsm);
                tblLoadingSummary.AddCell(tankStatus);

                DataTable dtLoadingSummary = clsGlobVar.dtSimulationLoadingSummary.Clone();
                foreach (DataRow dr in clsGlobVar.dtSimulationLoadingSummary.Rows)
                {
                    dtLoadingSummary.Rows.Add(dr.ItemArray);
                }

                dtLoadingSummary.Columns.Remove("IsDamaged");
                dtLoadingSummary.Columns.Add("IsDamaged", typeof(string));

                for (int i = 0; i < clsGlobVar.dtSimulationLoadingSummary.Rows.Count; i++)
                {
                    if (clsGlobVar.dtSimulationLoadingSummary.Rows[i]["IsDamaged"].ToString() == "True")
                    {
                        dtLoadingSummary.Rows[i]["IsDamaged"] = "Damaged";
                    }
                    else
                    {
                        dtLoadingSummary.Rows[i]["IsDamaged"] = "Intact";
                    }
                }
                dtLoadingSummary.Columns["IsDamaged"].ReadOnly = true;
                int columnCount = dtLoadingSummary.Columns.Count;
                int rowCount = dtLoadingSummary.Rows.Count;

                for (int rowCounter = 0; rowCounter < 41; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCount; columnCounter++)
                    {
                        string strValue = (dtLoadingSummary.Rows[rowCounter][columnCounter].ToString());
                        string temp;
                        string strDmage = (dtLoadingSummary.Rows[rowCounter][11].ToString());
                        if (columnCounter != 1 && columnCounter != 2)
                        {
                            if (rowCounter == rowCount - 1 || rowCounter == rowCount - 2 || rowCounter == rowCount - 3)
                            {
                                if (columnCounter == 0)
                                {
                                    try
                                    {
                                        PdfPCell pdf1;
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(strValue, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.BOLD)));
                                        pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdf1.Colspan = 4;
                                        tblLoadingSummary.AddCell(pdf1);
                                    }
                                    catch
                                    {
                                    }
                                }
                                else if (columnCounter == 6 || columnCounter == 7 || columnCounter == 8 || columnCounter == 9 || columnCounter == 10)
                                {
                                    decimal d = Convert.ToDecimal(strValue);
                                    temp = Convert.ToString(Math.Round(d, 3));
                                    PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                    tblLoadingSummary.AddCell(pdf);
                                }
                                else if (columnCounter == 11)
                                {
                                    PdfPCell pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                    pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    tblLoadingSummary.AddCell(pdf1);
                                }
                            }
                            else if (columnCounter == 3 || columnCounter == 4 || columnCounter == 5 || columnCounter == 6 || columnCounter == 7 || columnCounter == 8 || columnCounter == 9 || columnCounter == 10 && rowCounter != rowCount - 1 && rowCounter != rowCount - 2 && rowCounter != rowCount - 3)
                            {
                                PdfPCell pdf;
                                decimal d = Convert.ToDecimal(strValue);
                                temp = Convert.ToString(Math.Round(d, 3));
                                if (temp == Convert.ToString(0))
                                {
                                    pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    //pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    if (strDmage == "Damaged")
                                    {
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntRedColor));
                                        pdf.BackgroundColor = new iTextSharp.text.Color(214, 108, 105);
                                    }
                                    else
                                    {
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblLoadingSummary.AddCell(pdf);
                            }

                            else
                            {
                                PdfPCell pdf1;
                                temp = Convert.ToString(strValue);
                                if (temp == Convert.ToString(0))
                                {
                                    pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    // pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    if (strValue == "Damaged")
                                    {
                                        iTextSharp.text.Font fntBody1 = FontFactory.GetFont("Times New Roman", 6.5f, iTextSharp.text.Color.RED);
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody1));
                                    }
                                    else
                                    {
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                if (columnCounter == 0)
                                {
                                    pdf1.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                                else
                                {
                                    pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                tblLoadingSummary.AddCell(pdf1);
                            }
                        }
                    }
                }
                doc.Add(tblLoadingSummary);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                //..........EndofLoadingSummaryPart1..............................


                ////..........StartofLoadingSummaryPart2..............................
                doc.Add(new iTextSharp.text.Paragraph("  "));
                doc.Add(new iTextSharp.text.Paragraph("  "));

                PdfPTable tblLoadingSummary2 = new PdfPTable(10);
                tblLoadingSummary2.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                tblLoadingSummary2.WidthPercentage = 90;
                float[] widthsLoading2 = new float[] { 2.5f, 1.8f, 1.8f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.5f, 1f };      //in case nishant decide to incorporate again add 2nd and 3rd column as 1f  
                tblLoadingSummary2.SetWidths(widthsLoading2);
                tblLoadingSummary2.HorizontalAlignment = Element.ALIGN_CENTER;
                iTextSharp.text.Font fntRedColor2 = FontFactory.GetFont("Times New Roman", 1, iTextSharp.text.Color.ORANGE);// Body

                PdfPCell tankName2 = new PdfPCell(new Phrase("Tank Name", fntHeader));
                tankName2.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPCell tankStatus2 = new PdfPCell(new Phrase("Status", fntHeader));
                tankStatus2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell percentFull2 = new PdfPCell(new Phrase("Percent Fill", fntHeader));
                percentFull2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell Volume2 = new PdfPCell(new Phrase("Volume(cu.m)", fntHeader));
                Volume2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell sg2 = new PdfPCell(new Phrase("SG", fntHeader));
                sg2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell weight2 = new PdfPCell(new Phrase("Weight(T)", fntHeader));
                weight2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell lcg2 = new PdfPCell(new Phrase("LCG(m)", fntHeader));
                lcg2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell tcg2 = new PdfPCell(new Phrase("TCG(m)", fntHeader));
                tcg2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell vcg2 = new PdfPCell(new Phrase("VCG(m)", fntHeader));
                vcg2.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell fsm2 = new PdfPCell(new Phrase("FSM(T-m)", fntHeader));
                fsm2.HorizontalAlignment = Element.ALIGN_CENTER;
                //Add header to table
                tblLoadingSummary2.AddCell(tankName2);
                tblLoadingSummary2.AddCell(percentFull2);
                tblLoadingSummary2.AddCell(Volume2);
                tblLoadingSummary2.AddCell(sg2);
                tblLoadingSummary2.AddCell(weight2);
                tblLoadingSummary2.AddCell(lcg2);
                tblLoadingSummary2.AddCell(tcg2);
                tblLoadingSummary2.AddCell(vcg2);
                tblLoadingSummary2.AddCell(fsm2);
                tblLoadingSummary2.AddCell(tankStatus2);

                DataTable dtLoadingSummary2 = clsGlobVar.dtSimulationLoadingSummary.Clone();
                foreach (DataRow dr in clsGlobVar.dtSimulationLoadingSummary.Rows)
                {
                    dtLoadingSummary2.Rows.Add(dr.ItemArray);
                }

                dtLoadingSummary2.Columns.Remove("IsDamaged");
                dtLoadingSummary2.Columns.Add("IsDamaged", typeof(string));

                for (int i = 0; i < clsGlobVar.dtSimulationLoadingSummary.Rows.Count; i++)
                {
                    if (clsGlobVar.dtSimulationLoadingSummary.Rows[i]["IsDamaged"].ToString() == "True")
                    {
                        dtLoadingSummary2.Rows[i]["IsDamaged"] = "Damaged";
                    }
                    else
                    {
                        dtLoadingSummary2.Rows[i]["IsDamaged"] = "Intact";
                    }
                }
                dtLoadingSummary2.Columns["IsDamaged"].ReadOnly = true;
                int columnCount2 = dtLoadingSummary2.Columns.Count;
                int rowCount2 = dtLoadingSummary2.Rows.Count;

                for (int rowCounter = 41; rowCounter < rowCount2; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCount2; columnCounter++)
                    {
                        string strValue = (dtLoadingSummary2.Rows[rowCounter][columnCounter].ToString());
                        string temp;
                        string strDmage = (dtLoadingSummary2.Rows[rowCounter][11].ToString());
                        if (columnCounter != 1 && columnCounter != 2)
                        {
                            if (rowCounter == rowCount - 1 || rowCounter == rowCount - 2 || rowCounter == rowCount - 3)
                            {
                                if (columnCounter == 0)
                                {
                                    try
                                    {
                                        PdfPCell pdf1;
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(strValue, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.BOLD)));
                                        pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdf1.Colspan = 4;
                                        tblLoadingSummary2.AddCell(pdf1);
                                    }
                                    catch
                                    {
                                    }
                                }

                                else if (columnCounter == 6 || columnCounter == 7 || columnCounter == 8 || columnCounter == 9 || columnCounter == 10)
                                {
                                    decimal d = Convert.ToDecimal(strValue);
                                    temp = Convert.ToString(Math.Round(d, 3));
                                    PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                    tblLoadingSummary2.AddCell(pdf);
                                }
                                else if (columnCounter == 11)
                                {
                                    PdfPCell pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                    pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    tblLoadingSummary2.AddCell(pdf1);
                                }
                            }
                            else if (columnCounter == 3 || columnCounter == 4 || columnCounter == 5 || columnCounter == 6 || columnCounter == 7 || columnCounter == 8 || columnCounter == 9 || columnCounter == 10 && rowCounter != rowCount - 1 && rowCounter != rowCount - 2 && rowCounter != rowCount - 3)
                            {
                                PdfPCell pdf;
                                decimal d = Convert.ToDecimal(strValue);
                                temp = Convert.ToString(Math.Round(d, 3));
                                if (temp == Convert.ToString(0))
                                {
                                    pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    //pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    if (strDmage == "Damaged")
                                    {
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntRedColor));
                                        pdf.BackgroundColor = new iTextSharp.text.Color(214, 108, 105);
                                    }
                                    else
                                    {
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblLoadingSummary2.AddCell(pdf);
                            }

                            else
                            {
                                PdfPCell pdf1;
                                temp = Convert.ToString(strValue);
                                if (temp == Convert.ToString(0))
                                {
                                    pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    // pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    if (strValue == "Damaged")
                                    {
                                        iTextSharp.text.Font fntBody1 = FontFactory.GetFont("Times New Roman", 6.5f, iTextSharp.text.Color.RED);
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody1));
                                    }
                                    else
                                    {
                                        pdf1 = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                if (columnCounter == 0)
                                {
                                    pdf1.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                                else
                                {
                                    pdf1.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                tblLoadingSummary2.AddCell(pdf1);
                            }
                        }
                    }
                }
                doc.Add(tblLoadingSummary2);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                //..........EndofLoadingSummaryPart3..............................


                iTextSharp.text.Paragraph pHeaderDeadweight = new iTextSharp.text.Paragraph("Deadweight(T) Details", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                pHeaderDeadweight.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHeaderDeadweight);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                PdfPTable tblDeadweight = new PdfPTable(8);
                tblDeadweight.WidthPercentage = 90;
                float[] widthsDeadweight = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                tblDeadweight.SetWidths(widthsDeadweight);

                PdfPCell CargoOil = new PdfPCell(new Phrase("Cargo", fntHeader));
                PdfPCell FuelOil = new PdfPCell(new Phrase("Fuel Oil", fntHeader));
                PdfPCell DieselOil = new PdfPCell(new Phrase("Diesel Oil", fntHeader));
                PdfPCell LubeOil = new PdfPCell(new Phrase("Lube Oil", fntHeader));
                PdfPCell FreshWater = new PdfPCell(new Phrase("Fresh Water", fntHeader));
                PdfPCell BallastWater = new PdfPCell(new Phrase("Ballast Water", fntHeader));
                //PdfPCell Slope = new PdfPCell(new Phrase("Slope", fntHeader));
                PdfPCell Others = new PdfPCell(new Phrase("Others", fntHeader));
                PdfPCell FixedLoad = new PdfPCell(new Phrase("Fixed Load", fntHeader));
                CargoOil.HorizontalAlignment = Element.ALIGN_CENTER;
                FuelOil.HorizontalAlignment = Element.ALIGN_CENTER;
                DieselOil.HorizontalAlignment = Element.ALIGN_CENTER;
                LubeOil.HorizontalAlignment = Element.ALIGN_CENTER;
                FreshWater.HorizontalAlignment = Element.ALIGN_CENTER;
                BallastWater.HorizontalAlignment = Element.ALIGN_CENTER;
                //Slope.HorizontalAlignment = Element.ALIGN_CENTER;
                Others.HorizontalAlignment = Element.ALIGN_CENTER;
                FixedLoad.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDeadweight.AddCell(CargoOil);
                tblDeadweight.AddCell(FuelOil);
                tblDeadweight.AddCell(DieselOil);
                tblDeadweight.AddCell(LubeOil);
                tblDeadweight.AddCell(FreshWater);
                tblDeadweight.AddCell(BallastWater);
                //tblDeadweight.AddCell(Slope);
                tblDeadweight.AddCell(Others);
                tblDeadweight.AddCell(FixedLoad);

                for (int index = 0; index <= 7; index++)
                {
                    string strValueDead = clsGlobVar.dsSimulationDeadWeightDetails.Tables[index].Rows[0][0].ToString();
                    PdfPCell pdfDead = new PdfPCell(new iTextSharp.text.Paragraph(strValueDead, fntBody));
                    pdfDead.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblDeadweight.AddCell(pdfDead);
                }

                doc.Add(tblDeadweight);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                //Hydrostatic Equilibrium Angle
                iTextSharp.text.Paragraph pHeaderHydro = new iTextSharp.text.Paragraph("Hydrostatics", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                pHeaderHydro.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHeaderHydro);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                PdfPTable tblHydrostatic = new PdfPTable(8);
                tblHydrostatic.WidthPercentage = 90;
                float[] widthsHydro = new float[] { 1.5f, 1.3f, 1.7f, 1f, 1f, 1f, 1f, 1f };
                tblHydrostatic.SetWidths(widthsHydro);

                DataSet dsHydrostaticsData = new DataSet();
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                DataTable dtHydroDrafts = new DataTable();
                string Err = "";
                string cmd = "Select [Displacement],";
                cmd += "[TRIM],[Heel],[GMT],[KG(Solid)],[KG(Fluid)],[FSC],[LCG],[LCF]";
                cmd += ",[TPC],[MCT] From tblSimulationMode_Equilibrium_Values Where [USER] = 'dbo'";

                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsHydrostaticsData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtHydrostaticsData = new DataTable();
                dtHydrostaticsData = dsHydrostaticsData.Tables[0];

                // dtHydroDrafts = ((DataView)dtHydrostaticsData.ItemsSource).ToTable();
                int columnCountHydro2 = dtHydrostaticsData.Columns.Count;
                int rowCountHydro2 = dtHydrostaticsData.Rows.Count;
                decimal trimValue = Convert.ToDecimal(dtHydrostaticsData.Rows[0][1]);
                // decimal tvalue = trimValue.ToString("N");
                decimal heelValue = Convert.ToDecimal(dtHydrostaticsData.Rows[0][2]);


                PdfPCell Displacement = new PdfPCell(new Phrase("Displacement (T)", fntHeader));

                PdfPCell trim;
                PdfPCell list;
                if (trimValue > 0)
                {
                    trim = new PdfPCell(new Phrase("Trim(M) AFT", fntHeader));
                }
                else if (trimValue < 0)
                {
                    trim = new PdfPCell(new Phrase("Trim(M) FWD", fntHeader));
                }
                else
                {
                    trim = new PdfPCell(new Phrase("Trim(M)", fntHeader));
                }

                if (heelValue > 0)
                {
                    list = new PdfPCell(new Phrase("HEEL(DEG.) PORT", fntHeader));
                }
                else if (heelValue < 0)
                {
                    list = new PdfPCell(new Phrase("HEEL(DEG.) STBD", fntHeader));
                }
                else { list = new PdfPCell(new Phrase("HEEL(DEG.)", fntHeader)); ;}

                PdfPCell GMT = new PdfPCell(new Phrase("GMT (m)", fntHeader));
                PdfPCell KGS = new PdfPCell(new Phrase("KG-Solid (m)", fntHeader));
                PdfPCell KGF = new PdfPCell(new Phrase("KG-Fluid (m)", fntHeader));
                PdfPCell FSC = new PdfPCell(new Phrase("FSC (m)", fntHeader));
                PdfPCell LCG = new PdfPCell(new Phrase("LCG (m)", fntHeader));

                Displacement.HorizontalAlignment = Element.ALIGN_CENTER;
                trim.HorizontalAlignment = Element.ALIGN_CENTER;
                list.HorizontalAlignment = Element.ALIGN_CENTER;
                GMT.HorizontalAlignment = Element.ALIGN_CENTER;
                KGS.HorizontalAlignment = Element.ALIGN_CENTER;
                KGF.HorizontalAlignment = Element.ALIGN_CENTER;
                FSC.HorizontalAlignment = Element.ALIGN_CENTER;
                LCG.HorizontalAlignment = Element.ALIGN_CENTER;
                tblHydrostatic.AddCell(Displacement);
                tblHydrostatic.AddCell(trim);
                tblHydrostatic.AddCell(list);
                tblHydrostatic.AddCell(GMT);
                tblHydrostatic.AddCell(KGS);
                tblHydrostatic.AddCell(KGF);
                tblHydrostatic.AddCell(FSC);
                tblHydrostatic.AddCell(LCG);

                int columnCountHydro2332 = dtHydrostaticsData.Columns.Count;
                int columnCountHydro = columnCountHydro2332 - 3;
                int rowCountHydro = dtHydrostaticsData.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountHydro; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCountHydro; columnCounter++)
                    {
                        PdfPCell pdf;
                        string strValue = (dtHydrostaticsData.Rows[rowCounter][columnCounter].ToString());
                        //PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));


                        decimal value = Convert.ToDecimal(strValue);
                        string strvalueNew = "";
                        if (((columnCounter == 1) || (columnCounter == 2)) && (value < 0))
                        {
                            string dd = value.ToString("N");
                            decimal mm = Convert.ToDecimal(dd);
                            strvalueNew = Convert.ToString(Convert.ToDecimal(mm) * (-1));


                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strvalueNew, fntBody));
                        }
                        else
                        {

                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                        }

                        pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblHydrostatic.AddCell(pdf);
                    }
                }
                doc.Add(tblHydrostatic);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                //DraftAtEquilibriumAngle

                iTextSharp.text.Paragraph pHeaderDraft = new iTextSharp.text.Paragraph("Draft At Equilibrium (m)", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                pHeaderDraft.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHeaderDraft);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                PdfPTable tblDraft = new PdfPTable(5);
                tblDraft.WidthPercentage = 60;
                float[] widthsDraft = new float[] { 1f, 1f, 1f, 1f, 1f };
                tblDraft.SetWidths(widthsDraft);

                PdfPCell DraftAP = new PdfPCell(new Phrase("Draft AP", fntHeader));
                PdfPCell DraftFP = new PdfPCell(new Phrase("Draft FP", fntHeader));
                PdfPCell DraftMID = new PdfPCell(new Phrase("Draft MID", fntHeader));
                PdfPCell DraftAftMark = new PdfPCell(new Phrase("Draft Aft Mark", fntHeader));
                PdfPCell DraftFwdMark = new PdfPCell(new Phrase("Draft Fwd Mark", fntHeader));
                DraftAP.HorizontalAlignment = Element.ALIGN_CENTER;
                DraftFP.HorizontalAlignment = Element.ALIGN_CENTER;
                DraftMID.HorizontalAlignment = Element.ALIGN_CENTER;
                DraftAftMark.HorizontalAlignment = Element.ALIGN_CENTER;
                DraftFwdMark.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDraft.AddCell(DraftAP);
                tblDraft.AddCell(DraftFP);
                tblDraft.AddCell(DraftMID);
                tblDraft.AddCell(DraftAftMark);
                tblDraft.AddCell(DraftFwdMark);
                DataSet dsDraftData = new DataSet();

                command = Models.DAL.clsDBUtilityMethods.GetCommand();

                Err = "";
                cmd = "Select  [Draft_AP],[Draft_FP],[Draft_MID],[Draft_AFT_MARK],";
                cmd += "[Draft_FWD_MARK] From tblSimulationMode_Equilibrium_Values Where [USER] = 'dbo'";
                //   

                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsDraftData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtDraftData = new DataTable();
                dtDraftData = dsDraftData.Tables[0];
                int columnCountDraft = dtDraftData.Columns.Count;
                int rowCountDraft = dtDraftData.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountDraft; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCountDraft; columnCounter++)
                    {
                        string strValue = (dtDraftData.Rows[rowCounter][columnCounter].ToString());
                        PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                        pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblDraft.AddCell(pdf);
                    }
                }
                doc.Add(tblDraft);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                //Blind Zone And Propeller Immersion table  
                iTextSharp.text.Paragraph pHVisibility_PropImm = new iTextSharp.text.Paragraph("Visibility And Propeller Immersion", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                pHVisibility_PropImm.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHVisibility_PropImm);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                PdfPTable tblvisibility_PropImm = new PdfPTable(2);
                tblvisibility_PropImm.WidthPercentage = 40;
                float[] widthsVisiPropImm = new float[] { 1f, 1f };
                tblvisibility_PropImm.SetWidths(widthsVisiPropImm);
                PdfPCell PropellerImm = new PdfPCell(new Phrase("Propeller Immersion (%)", fntHeader));
                PdfPCell BlindZone = new PdfPCell(new Phrase("Blind Zone (m)", fntHeader));

                PropellerImm.HorizontalAlignment = Element.ALIGN_CENTER;
                BlindZone.HorizontalAlignment = Element.ALIGN_CENTER;
                tblvisibility_PropImm.AddCell(PropellerImm);
                tblvisibility_PropImm.AddCell(BlindZone);
                DataSet dsVisiPropImmData = new DataSet();
                command = Models.DAL.clsDBUtilityMethods.GetCommand();
                Err = "";
                cmd = "Select [Prop_Immersion],[Visibility] From tblSimulationMode_Equilibrium_Values Where [USER] = 'dbo'";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsVisiPropImmData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtVisiPropImmData = new DataTable();
                dtVisiPropImmData = dsVisiPropImmData.Tables[0];
                int columnCountVisiPropImm = dtVisiPropImmData.Columns.Count;
                int rowCountVisiPropImm = dtVisiPropImmData.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountVisiPropImm; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCountVisiPropImm; columnCounter++)
                    {
                        string strValue = (dtVisiPropImmData.Rows[rowCounter][columnCounter].ToString());
                        PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                        pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblvisibility_PropImm.AddCell(pdf);
                    }
                }
                doc.Add(tblvisibility_PropImm);
                doc.Add(new iTextSharp.text.Paragraph("  "));
             


                //IntactStability Or Damage Stability
                //Intact Stability Table Name
                PdfPTable tblIntact = new PdfPTable(4);
                tblIntact.WidthPercentage = 50;
                float[] widthsIntact = new float[] { 6f, 1.4f, 1.4f, 1f };
                tblIntact.SetWidths(widthsIntact);

                PdfPCell criterion = new PdfPCell(new Phrase("Criterion", fntHeader));
                PdfPCell criticalValue = new PdfPCell(new Phrase("Critical Value", fntHeader));
                PdfPCell actualvalue = new PdfPCell(new Phrase("Actual Value", fntHeader));
                PdfPCell status = new PdfPCell(new Phrase("Status", fntHeader));
                criterion.HorizontalAlignment = Element.ALIGN_CENTER;
                criticalValue.HorizontalAlignment = Element.ALIGN_CENTER;
                actualvalue.HorizontalAlignment = Element.ALIGN_CENTER;
                status.HorizontalAlignment = Element.ALIGN_CENTER;

                tblIntact.AddCell(criterion);
                tblIntact.AddCell(criticalValue);
                tblIntact.AddCell(actualvalue);
                tblIntact.AddCell(status);
                DataTable dtfinal = new DataTable();

                iTextSharp.text.Paragraph pHeaderIntact;
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {

                    pHeaderIntact = new iTextSharp.text.Paragraph("IMO Stability Criteria-" + lblCalculationMethod.Content.ToString(), FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD));

                }
                else
                {
                    if (rowCount2 > 59)
                    {
                        doc.NewPage();
                        doc.Add(new iTextSharp.text.Paragraph("  "));
                        pHeaderIntact = new iTextSharp.text.Paragraph("Stability Criteria-" + lblCalculationMethod.Content.ToString(), FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD));

                    }
                    else
                    {
                        pHeaderIntact = new iTextSharp.text.Paragraph("Stability Criteria-" + lblCalculationMethod.Content.ToString(), FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD));
                    }
                }
                pHeaderIntact.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHeaderIntact);


                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    dtfinal = clsGlobVar.dtSimulationStabilityCriteriaIntact.Clone();
                    foreach (DataRow dr in clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows)
                    {

                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));


                    for (int i = 0; i < clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows.Count; i++)
                    {
                        if (clsGlobVar.dtSimulationStabilityCriteriaIntact.Rows[i]["Status"].ToString() == "True")
                        {
                            dtfinal.Rows[i]["Status"] = "Pass";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "Fail";
                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;

                }
                else
                {
                   
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    dtfinal = clsGlobVar.dtSimulationStabilityCriteriaDamage.Clone();
                    foreach (DataRow dr in clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows)
                    {

                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));


                    for (int i = 0; i < clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows.Count; i++)
                    {
                        if (clsGlobVar.dtSimulationStabilityCriteriaDamage.Rows[i]["Status"].ToString() == "True")
                        {
                            dtfinal.Rows[i]["Status"] = "Pass";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "Fail";

                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;

                }

                int columnCountIntact = dtfinal.Columns.Count;
                int rowCountIntact = dtfinal.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountIntact; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCountIntact; columnCounter++)
                    {
                        string strValue = dtfinal.Rows[rowCounter][columnCounter].ToString();
                        PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                        if (columnCounter == 0)
                        {
                            pdf.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        tblIntact.AddCell(pdf);
                    }
                }
                doc.Add(tblIntact);

                //doc.Add(new iTextSharp.text.Paragraph("  "));


                if (lblCalculationMethod.Content.ToString() == "Damage")
                {
                    if (rowCount2 > 59)
                    {
                        //doc.NewPage();
                        doc.Add(new iTextSharp.text.Paragraph("  "));


                        iTextSharp.text.Paragraph pHeaderFloodingPoint = new iTextSharp.text.Paragraph("Flooding Point", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                        pHeaderFloodingPoint.Alignment = Element.ALIGN_CENTER;
                        doc.Add(pHeaderFloodingPoint);
                        doc.Add(new iTextSharp.text.Paragraph("  "));

                        PdfPTable tblFloodPoint = new PdfPTable(6);
                        tblFloodPoint.WidthPercentage = 80;
                        float[] widthsFloodPoint = new float[] { 1f, 1.5f, 1f, 1f, 1f, 1.5f };
                        tblFloodPoint.SetWidths(widthsFloodPoint);

                        PdfPCell Type = new PdfPCell(new Phrase("Type", fntHeader));
                        PdfPCell name = new PdfPCell(new Phrase("Name", fntHeader));
                        PdfPCell x = new PdfPCell(new Phrase("  X(M) ", fntHeader));
                        PdfPCell y = new PdfPCell(new Phrase(" Y(M) ", fntHeader));
                        PdfPCell z = new PdfPCell(new Phrase(" Z(M) ", fntHeader));
                        PdfPCell WlDistance = new PdfPCell(new Phrase(" Distance from WL(M) ", fntHeader));
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        name.HorizontalAlignment = Element.ALIGN_CENTER;
                        x.HorizontalAlignment = Element.ALIGN_CENTER;
                        y.HorizontalAlignment = Element.ALIGN_CENTER;
                        z.HorizontalAlignment = Element.ALIGN_CENTER;
                        WlDistance.HorizontalAlignment = Element.ALIGN_CENTER;

                        tblFloodPoint.AddCell(Type);
                        tblFloodPoint.AddCell(name);
                        tblFloodPoint.AddCell(x);
                        tblFloodPoint.AddCell(y);
                        tblFloodPoint.AddCell(z);
                        tblFloodPoint.AddCell(WlDistance);
                        //DataSet dsFllodPoint = new DataSet();
                        DataTable dtFloodPoint = new DataTable();
                        dtFloodPoint = clsGlobVar.FloodingPoint_Damage.Clone();
                        foreach (DataRow dr in clsGlobVar.FloodingPoint_Damage.Rows)
                        {

                            dtFloodPoint.Rows.Add(dr.ItemArray);
                        }
                        //dtDraftData = dsDraftData.Tables[0];
                        int columnCountFloodPoint = dtFloodPoint.Columns.Count;
                        int rowCountFloodPoint = dtFloodPoint.Rows.Count;
                        for (int rowCounter = 0; rowCounter < rowCountFloodPoint; rowCounter++)
                        {
                            for (int columnCounter = 0; columnCounter < columnCountFloodPoint; columnCounter++)
                            {

                                string strValue = (dtFloodPoint.Rows[rowCounter][columnCounter].ToString());
                                PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblFloodPoint.AddCell(pdf);
                            }
                        }
                        doc.Add(tblFloodPoint);
                        //doc.Add(new iTextSharp.text.Paragraph("  "));
                    }


                    else
                    {
                        doc.NewPage();
                        doc.Add(new iTextSharp.text.Paragraph("  "));
                        iTextSharp.text.Paragraph pHeaderFloodingPoint = new iTextSharp.text.Paragraph("Flooding Point", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                        pHeaderFloodingPoint.Alignment = Element.ALIGN_CENTER;
                        doc.Add(pHeaderFloodingPoint);
                        doc.Add(new iTextSharp.text.Paragraph("  "));

                        PdfPTable tblFloodPoint = new PdfPTable(6);
                        tblFloodPoint.WidthPercentage = 80;
                        float[] widthsFloodPoint = new float[] { 1f, 1.5f, 1f, 1f, 1f, 1.5f };
                        tblFloodPoint.SetWidths(widthsFloodPoint);

                        PdfPCell Type = new PdfPCell(new Phrase("Type", fntHeader));
                        PdfPCell name = new PdfPCell(new Phrase("Name", fntHeader));
                        PdfPCell x = new PdfPCell(new Phrase("  X(M) ", fntHeader));
                        PdfPCell y = new PdfPCell(new Phrase(" Y(M) ", fntHeader));
                        PdfPCell z = new PdfPCell(new Phrase(" Z(M) ", fntHeader));
                        PdfPCell WlDistance = new PdfPCell(new Phrase(" Distance from WL(M) ", fntHeader));
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        name.HorizontalAlignment = Element.ALIGN_CENTER;
                        x.HorizontalAlignment = Element.ALIGN_CENTER;
                        y.HorizontalAlignment = Element.ALIGN_CENTER;
                        z.HorizontalAlignment = Element.ALIGN_CENTER;
                        WlDistance.HorizontalAlignment = Element.ALIGN_CENTER;

                        tblFloodPoint.AddCell(Type);
                        tblFloodPoint.AddCell(name);
                        tblFloodPoint.AddCell(x);
                        tblFloodPoint.AddCell(y);
                        tblFloodPoint.AddCell(z);
                        tblFloodPoint.AddCell(WlDistance);
                        //DataSet dsFllodPoint = new DataSet();
                        DataTable dtFloodPoint = new DataTable();
                        dtFloodPoint = clsGlobVar.FloodingPoint_Damage.Clone();
                        foreach (DataRow dr in clsGlobVar.FloodingPoint_Damage.Rows)
                        {

                            dtFloodPoint.Rows.Add(dr.ItemArray);
                        }
                        //dtDraftData = dsDraftData.Tables[0];
                        int columnCountFloodPoint = dtFloodPoint.Columns.Count;
                        int rowCountFloodPoint = dtFloodPoint.Rows.Count;
                        for (int rowCounter = 0; rowCounter < rowCountFloodPoint; rowCounter++)
                        {
                            for (int columnCounter = 0; columnCounter < columnCountFloodPoint; columnCounter++)
                            {

                                string strValue = (dtFloodPoint.Rows[rowCounter][columnCounter].ToString());
                                PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblFloodPoint.AddCell(pdf);
                            }
                        }
                        doc.Add(tblFloodPoint);
                        //doc.Add(new iTextSharp.text.Paragraph("  "));
                    }
                }
                

// Flooding Angles ...........................................

                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    iTextSharp.text.Paragraph floodingAngle = new iTextSharp.text.Paragraph("Downflooding Points ", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                    floodingAngle.Alignment = Element.ALIGN_CENTER;
                    doc.Add(floodingAngle);
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    PdfPTable tblfloodingAngle = new PdfPTable(5);
                    tblfloodingAngle.WidthPercentage = 60;
                    float[] widthsfloodingAngle = new float[] { 2.5f, 1f, 1f, 1f, 2.5f };
                    tblfloodingAngle.SetWidths(widthsfloodingAngle);
                    PdfPCell location = new PdfPCell(new Phrase("Location Name", fntHeader));
                    PdfPCell xx = new PdfPCell(new Phrase(" X (M)", fntHeader));
                    PdfPCell yy = new PdfPCell(new Phrase(" Y (M)", fntHeader));
                    PdfPCell zz = new PdfPCell(new Phrase(" Z (M)", fntHeader));
                    PdfPCell angel = new PdfPCell(new Phrase(" Imersion Angle (DEG.)", fntHeader));

                    location.HorizontalAlignment = Element.ALIGN_CENTER;
                    xx.HorizontalAlignment = Element.ALIGN_CENTER;
                    yy.HorizontalAlignment = Element.ALIGN_CENTER;
                    zz.HorizontalAlignment = Element.ALIGN_CENTER;
                    angel.HorizontalAlignment = Element.ALIGN_CENTER;

                    tblfloodingAngle.AddCell(location);
                    tblfloodingAngle.AddCell(xx);
                    tblfloodingAngle.AddCell(yy);
                    tblfloodingAngle.AddCell(zz);
                    tblfloodingAngle.AddCell(angel);
                    DataSet dsfloodingAngle = new DataSet();
                    command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    Err = "";
                    cmd = "Select [openingName],[openingX],[openingY],[openingZ], [actualImAng] From [tblSimulationDownFloodingAngle]";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    dsfloodingAngle = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                    DataTable dtfloodingAngle = new DataTable();
                    dtfloodingAngle = dsfloodingAngle.Tables[0];
                    int columnCountfloodingAngle = dtfloodingAngle.Columns.Count;
                    int rowCountfloodingAngle = dtfloodingAngle.Rows.Count;
                    for (int rowCounter = 0; rowCounter < rowCountfloodingAngle; rowCounter++)
                    {
                        for (int columnCounter = 0; columnCounter < columnCountfloodingAngle; columnCounter++)
                        {
                            string strValue = (dtfloodingAngle.Rows[rowCounter][columnCounter].ToString());
                            PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                            pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                            tblfloodingAngle.AddCell(pdf);
                        }
                    }
                    doc.Add(tblfloodingAngle);
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                }
                doc.NewPage();
//..............................................................................................
                iTextSharp.text.Paragraph pHeaderGZ = new iTextSharp.text.Paragraph("GZ Graph", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // validation if data not available
                pHeaderGZ.Alignment = Element.ALIGN_CENTER;
                doc.Add(pHeaderGZ);
                doc.Add(new iTextSharp.text.Paragraph("  "));

                iTextSharp.text.Image imgGZ = iTextSharp.text.Image.GetInstance(System.Windows.Forms.Application.StartupPath + "\\Images\\GZ_curve.png");
                imgGZ.Alignment = Element.ALIGN_CENTER;
                imgGZ.SpacingAfter = 20f;
                imgGZ.ScaleToFit(400, 300);
                doc.Add(imgGZ);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                PdfPTable pp1 = new PdfPTable(2);
                pp1.WidthPercentage = 30;
                float[] widthsGZ5 = new float[] { 2f, 2f };
                pp1.SetWidths(widthsGZ5);

                PdfPCell heel = new PdfPCell(new Phrase("Heel(Deg)", fntHeader));
                PdfPCell GZ = new PdfPCell(new Phrase("GZ(m)", fntHeader));
                heel.HorizontalAlignment = Element.ALIGN_CENTER;
                GZ.HorizontalAlignment = Element.ALIGN_CENTER;
                pp1.AddCell(heel);
                pp1.AddCell(GZ);
                DataSet dsGZData = new DataSet();

                command = Models.DAL.clsDBUtilityMethods.GetCommand();

                Err = "";
                cmd = "SELECT heelAng,heelGZ from GZDataSimulationMode_New where [User] = 'dbo'";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;
                dsGZData = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                DataTable dtGZgraph = new DataTable();
                dtGZgraph = dsGZData.Tables[0];
                int columnCountGZ1 = dtGZgraph.Columns.Count;
                int rowCountGZ1 = dtGZgraph.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountGZ1; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < 2; columnCounter++)
                    {

                        string temp;
                        object obj = dtGZgraph.Rows[rowCounter][columnCounter];
                        string strValue1 = (obj.ToString());
                        decimal d = Convert.ToDecimal(strValue1);
                        temp = Convert.ToString(Math.Round(d, 2));
                        PdfPCell cell1 = new PdfPCell(new Phrase(temp, FontFactory.GetFont("Times New Roman", 7)));

                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        pp1.AddCell(cell1);

                    }
                }
                doc.Add(pp1);

                // Start of Longitudinal Strength Data 1 to 31 rows
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    doc.NewPage();
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    iTextSharp.text.Paragraph pHeaderNet = new iTextSharp.text.Paragraph("Longitudinal Strength Curves", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); //Intact Stability Table Name
                    pHeaderNet.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pHeaderNet);
                    iTextSharp.text.Image imgNet = iTextSharp.text.Image.GetInstance(System.Windows.Forms.Application.StartupPath + "\\Images\\Longitudinal_curve.png");
                    imgNet.Alignment = Element.ALIGN_CENTER;
                    imgNet.SpacingAfter = 20f;
                    imgNet.ScaleToFit(500, 500);
                    doc.Add(imgNet);

                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    iTextSharp.text.Paragraph pHeaderLongitudinal = new iTextSharp.text.Paragraph("Longitudinal Strength Data", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); //Intact Stability Table Name
                    pHeaderLongitudinal.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pHeaderLongitudinal);
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    PdfPTable tblLongitudinal = new PdfPTable(6);
                    tblLongitudinal.WidthPercentage = 80;
                    float[] widthsLongitudinal = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };
                    tblLongitudinal.SetWidths(widthsLongitudinal);
                    PdfPCell positionFrame = new PdfPCell(new Phrase("Distance From Origin (m)", fntHeader));
                    PdfPCell SF = new PdfPCell(new Phrase("Shear Force (T)", fntHeader));
                    PdfPCell SFPercent = new PdfPCell(new Phrase("% SF", fntHeader));
                    PdfPCell BM = new PdfPCell(new Phrase("Bending Moment (T-m)", fntHeader));
                    PdfPCell BMPercent = new PdfPCell(new Phrase("% BM", fntHeader));
                    PdfPCell Status = new PdfPCell(new Phrase("Status", fntHeader));
                    positionFrame.HorizontalAlignment = Element.ALIGN_CENTER;
                    SF.HorizontalAlignment = Element.ALIGN_CENTER;
                    SFPercent.HorizontalAlignment = Element.ALIGN_CENTER;
                    BMPercent.HorizontalAlignment = Element.ALIGN_CENTER;
                    BM.HorizontalAlignment = Element.ALIGN_CENTER;
                    Status.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblLongitudinal.AddCell(positionFrame);
                    tblLongitudinal.AddCell(SF);
                    tblLongitudinal.AddCell(SFPercent);
                    tblLongitudinal.AddCell(BM);
                    tblLongitudinal.AddCell(BMPercent);
                    tblLongitudinal.AddCell(Status);

                    DataSet dsLongitudinalFrame = new DataSet();
                    command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    Err = "";
                    cmd = "  SELECT [Frame],[SF],[SF_Percentage_Diff],[BM],[BM_Percentage_Diff],[Status] from [tbl_SimulationMode_SFAndBM_New]";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    dsLongitudinalFrame = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                    DataTable dtLongitudinalFrame = new DataTable();
                    dtLongitudinalFrame = dsLongitudinalFrame.Tables[0];
                    dtfinal = dtLongitudinalFrame.Clone();
                    foreach (DataRow dr in dtLongitudinalFrame.Rows)
                    {
                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));

                    for (int i = 0; i < dtLongitudinalFrame.Rows.Count; i++)
                    {
                        if (dtLongitudinalFrame.Rows[i]["Status"].ToString() == "1")
                        {
                            dtfinal.Rows[i]["Status"] = "OK";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "NOT OK";
                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;
                    int columnCountLongitudinal = dtfinal.Columns.Count;
                    int rowCountLongitudinal = dtfinal.Rows.Count;
                    for (int rowCounter = 0; rowCounter < 30; rowCounter++)
                    {
                        for (int columnCounter = 0; columnCounter < columnCountLongitudinal; columnCounter++)
                        {
                            {
                                PdfPCell pdf;
                                string strValue = "";
                                if (columnCounter == 0)
                                {
                                     strValue = Math.Round(Convert.ToDecimal((dtfinal.Rows[rowCounter][columnCounter]))).ToString();
                                }
                                else
                                { 
                                    strValue = (dtfinal.Rows[rowCounter][columnCounter].ToString()); 
                                }
                                
                                if (strValue == Convert.ToString(0))
                                {
                                    pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    if (columnCounter == columnCountLongitudinal - 1)
                                    {
                                        if (strValue == "NOT OK")
                                        {
                                            iTextSharp.text.Font fntBody1 = FontFactory.GetFont("Times New Roman", 6.5f, iTextSharp.text.Color.RED);
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody1));
                                        }
                                        else
                                        {
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                        }
                                    }
                                    else
                                    {
                                        decimal d = Convert.ToDecimal(strValue);
                                        string temp = Convert.ToString(Math.Round(d, 2));
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }

                                }
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblLongitudinal.AddCell(pdf);
                            }
                        }
                    }
                    doc.Add(tblLongitudinal);
                    // End of Longitudinal Strength Data 1 to 31 rows

                    // Start of Longitudinal Strength Data 32 to 100 rows
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    PdfPTable tblLongitudinal1 = new PdfPTable(6);
                    tblLongitudinal1.WidthPercentage = 80;
                    float[] widthsLongitudinal1 = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };
                    tblLongitudinal.SetWidths(widthsLongitudinal1);
                    PdfPCell positionFrame1 = new PdfPCell(new Phrase("Distance From Origin (m)", fntHeader));
                    PdfPCell SF1 = new PdfPCell(new Phrase("Shear Force (T)", fntHeader));
                    PdfPCell SFPercent1 = new PdfPCell(new Phrase("% SF", fntHeader));
                    PdfPCell BM1 = new PdfPCell(new Phrase("Bending Moment (T-m)", fntHeader));
                    PdfPCell BMPercent1 = new PdfPCell(new Phrase("% BM", fntHeader));
                    PdfPCell Status1 = new PdfPCell(new Phrase("Status", fntHeader));
                    positionFrame1.HorizontalAlignment = Element.ALIGN_CENTER;
                    SF1.HorizontalAlignment = Element.ALIGN_CENTER;
                    SFPercent1.HorizontalAlignment = Element.ALIGN_CENTER;
                    BMPercent1.HorizontalAlignment = Element.ALIGN_CENTER;
                    BM1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Status1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblLongitudinal1.AddCell(positionFrame);
                    tblLongitudinal1.AddCell(SF);
                    tblLongitudinal1.AddCell(SFPercent);
                    tblLongitudinal1.AddCell(BM);
                    tblLongitudinal1.AddCell(BMPercent);
                    tblLongitudinal1.AddCell(Status);

                    DataSet dsLongitudinalFrame1 = new DataSet();
                    command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    Err = "";
                    cmd = "  SELECT [Frame],[SF],[SF_Percentage_Diff],[BM],[BM_Percentage_Diff],[Status] from [tbl_SimulationMode_SFAndBM_New]";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    dsLongitudinalFrame = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                    DataTable dtLongitudinalFrame1 = new DataTable();
                    dtLongitudinalFrame1 = dsLongitudinalFrame.Tables[0];
                    dtfinal = dtLongitudinalFrame1.Clone();
                    foreach (DataRow dr in dtLongitudinalFrame1.Rows)
                    {
                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));

                    for (int i = 0; i < dtLongitudinalFrame1.Rows.Count; i++)
                    {
                        if (dtLongitudinalFrame1.Rows[i]["Status"].ToString() == "1")
                        {
                            dtfinal.Rows[i]["Status"] = "OK";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "NOT OK";
                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;

                    int columnCountLongitudinal1 = dtfinal.Columns.Count;
                    int rowCountLongitudinal1 = dtfinal.Rows.Count;
                    for (int rowCounter = 30; rowCounter < 94; rowCounter++)
                    {
                        for (int columnCounter = 0; columnCounter < columnCountLongitudinal1; columnCounter++)
                        {
                            {
                                PdfPCell pdf;
                                string strValue = "";
                                if (columnCounter == 0)
                                {
                                 strValue = Math.Round(Convert.ToDecimal(dtfinal.Rows[rowCounter][columnCounter])).ToString();
                                }
                                else
                                { strValue = (dtfinal.Rows[rowCounter][columnCounter].ToString()); }
                                if (strValue == Convert.ToString(0))
                                {
                                   pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                }
                                else
                                {
                                    if (columnCounter == columnCountLongitudinal1 - 1)
                                    {
                                        if (strValue == "NOT OK")
                                        {
                                            iTextSharp.text.Font fntBody1 = FontFactory.GetFont("Times New Roman", 6.5f, iTextSharp.text.Color.RED);
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody1));
                                        }
                                        else
                                        {
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                        }
                                    }
                                    else
                                    {
                                        decimal d = Convert.ToDecimal(strValue);
                                        string temp = Convert.ToString(Math.Round(d, 2));
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblLongitudinal1.AddCell(pdf);
                            }
                        }
                    }
                    doc.Add(tblLongitudinal1);// End of Longitudinal Strength Data 32 to 100 rows

                    // Start of Longitudinal Strength Data 100 to 107 rows
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    doc.Add(new iTextSharp.text.Paragraph("  "));
                    PdfPTable tblLongitudinal2 = new PdfPTable(6);
                    tblLongitudinal2.WidthPercentage = 80;
                    float[] widthsLongitudinal2 = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };
                    tblLongitudinal2.SetWidths(widthsLongitudinal2);
                    PdfPCell positionFrame2 = new PdfPCell(new Phrase("Distance From Origin (m)", fntHeader));
                    PdfPCell SF2 = new PdfPCell(new Phrase("Shear Force (T)", fntHeader));
                    PdfPCell SFPercent2 = new PdfPCell(new Phrase("% SF", fntHeader));
                    PdfPCell BM2 = new PdfPCell(new Phrase("Bending Moment (T-m)", fntHeader));
                    PdfPCell BMPercent2 = new PdfPCell(new Phrase("% BM", fntHeader));
                    PdfPCell Status2 = new PdfPCell(new Phrase("Status", fntHeader));
                    positionFrame2.HorizontalAlignment = Element.ALIGN_CENTER;
                    SF2.HorizontalAlignment = Element.ALIGN_CENTER;
                    SFPercent2.HorizontalAlignment = Element.ALIGN_CENTER;
                    BMPercent2.HorizontalAlignment = Element.ALIGN_CENTER;
                    BM2.HorizontalAlignment = Element.ALIGN_CENTER;
                    Status2.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblLongitudinal2.AddCell(positionFrame2);
                    tblLongitudinal2.AddCell(SF2);
                    tblLongitudinal2.AddCell(SFPercent2);
                    tblLongitudinal2.AddCell(BM2);
                    tblLongitudinal2.AddCell(BMPercent2);
                    tblLongitudinal2.AddCell(Status2);

                    DataSet dsLongitudinalFrame2 = new DataSet();
                    command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    Err = "";
                    cmd = "  SELECT [Frame],[SF],[SF_Percentage_Diff],[BM],[BM_Percentage_Diff],[Status] from [tbl_SimulationMode_SFAndBM_New]";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    dsLongitudinalFrame = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
                    DataTable dtLongitudinalFrame2 = new DataTable();
                    dtLongitudinalFrame2 = dsLongitudinalFrame.Tables[0];
                    dtfinal = dtLongitudinalFrame2.Clone();
                    foreach (DataRow dr in dtLongitudinalFrame2.Rows)
                    {
                        dtfinal.Rows.Add(dr.ItemArray);
                    }
                    dtfinal.Columns.Remove("Status");
                    dtfinal.Columns.Add("Status", typeof(string));

                    for (int i = 0; i < dtLongitudinalFrame2.Rows.Count; i++)
                    {
                        if (dtLongitudinalFrame2.Rows[i]["Status"].ToString() == "1")
                        {
                            dtfinal.Rows[i]["Status"] = "OK";
                        }
                        else
                        {
                            dtfinal.Rows[i]["Status"] = "NOT OK";
                        }
                    }
                    dtfinal.Columns["Status"].ReadOnly = true;

                    int columnCountLongitudinal2 = dtfinal.Columns.Count;
                    int rowCountLongitudinal2 = dtfinal.Rows.Count;
                    for (int rowCounter = 94; rowCounter < rowCountLongitudinal2; rowCounter++)
                    {
                        for (int columnCounter = 0; columnCounter < columnCountLongitudinal2; columnCounter++)
                        {
                            {
                                PdfPCell pdf;
                                string strValue = "";
                                if (columnCounter == 0)
                                {
                                    strValue = Math.Round(Convert.ToDecimal((dtfinal.Rows[rowCounter][columnCounter]))).ToString();
                                }
                                else
                                { strValue = (dtfinal.Rows[rowCounter][columnCounter].ToString()); }
                                if (strValue == Convert.ToString(0))
                                {
                                    pdf = new PdfPCell(new iTextSharp.text.Paragraph(" ", fntBody));
                                }
                                else
                                {
                                    if (columnCounter == columnCountLongitudinal2 - 1)
                                    {
                                        if (strValue == "NOT OK")
                                        {
                                            iTextSharp.text.Font fntBody1 = FontFactory.GetFont("Times New Roman", 6.5f, iTextSharp.text.Color.RED);
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody1));
                                        }
                                        else
                                        {
                                            pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                                        }
                                    }
                                    else
                                    {
                                        decimal d = Convert.ToDecimal(strValue);
                                        string temp = Convert.ToString(Math.Round(d, 2));
                                        pdf = new PdfPCell(new iTextSharp.text.Paragraph(temp, fntBody));
                                    }
                                }
                                pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblLongitudinal2.AddCell(pdf);
                            }
                        }
                    }
                    doc.Add(tblLongitudinal2);  // End of Longitudinal Strength Data 100 to 107 rows

                    ////Max_SF Max_BM Table
                    //doc.Add(new iTextSharp.text.Paragraph("  "));
                    //if (lblCalculationMethod.Content.ToString() == "Intact")
                    //{
                    //    doc.Add(new iTextSharp.text.Paragraph("  "));

                    //    PdfPTable tblSFBM = new PdfPTable(4);
                    //    tblSFBM.WidthPercentage = 65;
                    //    float[] widthsDraft2 = new float[] { 1.2f, 1f, 1.2f, 1f };
                    //    tblSFBM.SetWidths(widthsDraft2);

                    //    PdfPCell sffr = new PdfPCell(new Phrase("MAX SF at Distance (m)", fntHeader));
                    //    PdfPCell sf = new PdfPCell(new Phrase("MAX SF(T)", fntHeader));
                    //    PdfPCell mbfr = new PdfPCell(new Phrase("MAX BM at Distance (m)", fntHeader));
                    //    PdfPCell bm = new PdfPCell(new Phrase("MAX BM(T-M)", fntHeader));

                    //    sffr.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    sf.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    mbfr.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    bm.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    // DraftFwdMark.HorizontalAlignment = Element.ALIGN_CENTER;

                    //    tblSFBM.AddCell(sffr);
                    //    tblSFBM.AddCell(sf);
                    //    tblSFBM.AddCell(mbfr);
                    //    tblSFBM.AddCell(bm);
                    //    //tblDraft.AddCell(DraftFwdMark);
                    //    //DataSet dsDraftData = new DataSet();
                    //    //DataTable dtDraftDataSFBM = new DataTable();
                    //    DataTable dtDraftDataSFBM = clsGlobVar.dtSimulationSFBMMax.Clone();
                    //    foreach (DataRow dr in clsGlobVar.dtSimulationSFBMMax.Rows)
                    //    {
                    //        dtDraftDataSFBM.Rows.Add(dr.ItemArray);
                    //    }
                    //    // DataTable dtDraftDataSFBM = new DataTable();
                    //    dtDraftData = dsDraftData.Tables[0];
                    //    int columnCountDraft2 = Models.clsGlobVar.dtSimulationSFBMMax.Columns.Count;
                    //    int rowCountDraft2 = dtDraftData.Rows.Count;
                    //    for (int rowCounter = 0; rowCounter < rowCountDraft2; rowCounter++)
                    //    {
                    //        for (int columnCounter = 0; columnCounter < columnCountDraft2; columnCounter++)
                    //        {
                    //            PdfPCell pdf;
                    //            string strValue = (Models.clsGlobVar.dtSimulationSFBMMax.Rows[rowCounter][columnCounter].ToString());
                    //            string gg = Convert.ToDecimal(strValue).ToString("0");
                    //            if (columnCounter == 0 || columnCounter == 2)
                    //            {
                    //                pdf = new PdfPCell(new iTextSharp.text.Paragraph(gg, fntBody));
                    //            }
                    //            else
                    //            {
                    //                pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                    //            }
                    //            pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                    //            tblSFBM.AddCell(pdf);
                    //        }
                    //    }
                    //    doc.Add(tblSFBM);
                    //}
                    //End of Max_SF Max_BM Table
                }
                doc.Close();
                Mouse.OverrideCursor = null;

                if (DamageAll == 10)
                {

                    string st = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string rpath = st + "\\Reports\\AllDamageCases";
                    // var dir = Directory.GetFiles(rpath);
                    DirectoryInfo di = new DirectoryInfo(rpath);
                    FileInfo[] files = di.GetFiles();

                    if (files.Length > 0)
                    {

                        String[] source_files =  { Convert.ToString(rpath + "\\" + files[0]),
                                                   Convert.ToString(rpath + "\\" + files[1]), 
                                                   Convert.ToString(rpath + "\\" + files[2]),
                                                   Convert.ToString(rpath + "\\" + files[3]),
                                                   Convert.ToString(rpath + "\\" + files[4]),
                                                   Convert.ToString(rpath + "\\" + files[5]),
                                                   Convert.ToString(rpath + "\\" + files[6]),
                                                   Convert.ToString(rpath + "\\" + files[7]),
                                                   Convert.ToString(rpath + "\\" + files[8]),
                                                   Convert.ToString(rpath + "\\" + files[9]),
                                                   Convert.ToString(rpath + "\\" + files[10])};
                        reportPath = System.Windows.Forms.Application.StartupPath + "\\Reports\\" + "AllDamageCases_" + ISO_Date() + "_Report.pdf";
                        Document document = new Document();
                        //create PdfCopy object
                        PdfCopy copy = new PdfCopy(document, new FileStream(reportPath, FileMode.Create));
                        //open the document
                        document.Open();
                        //PdfReader variable
                        PdfReader reader;
                        for (int i = 0; i < source_files.Length; i++)
                        {
                            //create PdfReader object
                            reader = new PdfReader(source_files[i]);
                            //merge combine pages
                            for (int page = 1; page <= reader.NumberOfPages; page++)
                                copy.AddPage(copy.GetImportedPage(reader, page));
                        }
                        //close the document object
                        string[] fileNames = Directory.GetFiles(rpath);
                        foreach (string fileName in fileNames)
                            File.Delete(fileName);
                        document.Close();
                    }
                }
                if (DamageAll == 0)
                {
                    System.Windows.MessageBox.Show("PDF Created!");
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion PDFWriter
        #endregion UserFunctions

        #region BackgroundWorker
        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isCalculationRunning = false;
          //canvasTwoD.Children.RemoveRange(1, canvasTwoD.Children.Count - 1);


        }
        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // pbCalculation.Value = e.ProgressPercentage;

        }
        private void bgWorker_Do_Work(object sender, DoWorkEventArgs e)
        {
            try
            {
            
                string user = "dbo";
                int res;
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                command.CommandText = "spCal_SimulationMode_Stability";
                command.CommandType = CommandType.StoredProcedure;
                DbParameter param1 = Models.DAL.clsDBUtilityMethods.GetParameter();
                param1.DbType = DbType.String;
                param1.ParameterName = "@User";
                command.Parameters.Add(param1);

                DbParameter param2 = Models.DAL.clsDBUtilityMethods.GetParameter();
                param2.DbType = DbType.Int16;
                param2.Direction = ParameterDirection.Output;
                param2.ParameterName = "@Stability_Calculation_Result";
                command.Parameters.Add(param2);

                param1.Value = user;
                //param2.Value = 0;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);

                //obj.CalculateSMStability("spCal_SimulationMode_Stability", user, out res);
                ZebecLoadMaster.Models.DAL.clsSqlData.calculationResult = Convert.ToInt32(command.Parameters[1].Value);
                //bgWorker.ReportProgress(100);
                isCalculationRunning = false;
            }
            catch
            {
            }
        }
        #endregion BackgroundWorker

        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                //speechSynthesizer.Speak("Generate Report!");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                printToPdf();
                if (DamageAll == 0)
                {
                    System.Diagnostics.Process.Start(reportPath);
                }
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dgFixedLoad_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                header = e.Column.Header.ToString();
                if (e.Column.GetType().ToString() == "System.Windows.Controls.DataGridTextColumn")
                {
                    index = e.Row.GetIndex();

                    TextBlock cbo = (System.Windows.Controls.TextBlock)e.Column.GetCellContent(e.Row);


                }
            }
            catch
            {

            }
        }

        private void btnSaveLoadingCondition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                //speechSynthesizer.Speak("Save Loading Condition!");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string path = "";
                //  string path = System.IO.Directory.GetCurrentDirectory();
                if (lblCalculationMethod.Content.ToString() == "Intact")
                {
                    path = System.IO.Directory.GetCurrentDirectory() + "\\SMData\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + "_"
                        + lblCalculationMethod.Content.ToString() + "_" + txtLoadingConditionName.Text.ToString();

                }
                else
                {
                    path = System.IO.Directory.GetCurrentDirectory() + "\\SMData\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + "_" + lblCalculationMethod.Content.ToString() + "_" + clsGlobVar.DamageCase + "_" + txtLoadingConditionName.Text.ToString();
                }
                Directory.CreateDirectory(path);
                List<Tanks> liTanks = new List<Tanks>();
                Tanks objFresh = new Tanks();
                DataTable dt = new DataTable();
                dt = Models.clsGlobVar.dtSimulationAllTanks;
                //    dt = dsSMFreshWaterTank.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    int tank_ID = Convert.ToInt32(dr[0].ToString());
                    string group = dr[1].ToString();
                    string tank_Name = dr[2].ToString();
                    decimal weight = Convert.ToDecimal(dr[3].ToString());
                    decimal volume = Convert.ToDecimal(dr[4].ToString());
                    decimal percent_Full = Convert.ToDecimal(dr[5].ToString());
                    decimal sG = Convert.ToDecimal(dr[6].ToString());
                    decimal fSM = Convert.ToDecimal(dr[7].ToString());
                    bool isDamaged = Convert.ToBoolean(dr[8]);
                    int Max_1_act_0 = Convert.ToInt16(dr[9].ToString());
                    liTanks.Add(new Tanks
                    {
                        Tank_ID = tank_ID,
                        Group = group,
                        Tank_Name = tank_Name,
                        Weight = weight,
                        Volume = volume,
                        Percent_Full = percent_Full,
                        SG = sG,
                        FSM = fSM,
                        IsDamaged = isDamaged,
                        max_1_act_0 = Max_1_act_0
                    });

                }

                string fn = path + "\\Tanks.cnd";
                FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter objFormat = new BinaryFormatter();
                objFormat.Serialize(fs, liTanks);
                fs.Close();


                List<FixedItems> listFixedLoads = new List<FixedItems>();
                FixedItems objDeck1 = new FixedItems();
                dt = Models.clsGlobVar.dtSimulationVariableItems;

                foreach (DataRow dr in dt.Rows)
                {
                    listFixedLoads.Add(new FixedItems
                    {
                        Tank_ID = Convert.ToInt32(dr[0].ToString()),
                        Tank_Name = dr[1].ToString(),
                        Weight = Convert.ToDecimal(dr[2].ToString()),
                        LCG = Convert.ToDecimal(dr[3].ToString()),
                        TCG = Convert.ToDecimal(dr[4].ToString()),
                        VCG = Convert.ToDecimal(dr[5].ToString())
                    });
                }
                fn = path + "\\FixedLoads.cnd";

                fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.None);
                objFormat = new BinaryFormatter();
                objFormat.Serialize(fs, listFixedLoads);
                fs.Close();
                Mouse.OverrideCursor = null;
                System.Windows.MessageBox.Show("Loading Condition Saved Successfully");
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ReportInPDF_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", System.Windows.Forms.Application.StartupPath + @"\Reports");
        }

        private void SavedLoadingCondition_Click(object sender, RoutedEventArgs e)
        {
            LoadingCondition objLoadingCondition = new LoadingCondition("\\SMData");
            objLoadingCondition.ShowDialog();
        }

        private void StandardLoadingCondition_Click(object sender, RoutedEventArgs e)
        {
            LoadingCondition objLoadingCondition = new LoadingCondition("\\StandardData\\");
            objLoadingCondition.ShowDialog();
        }

        private void dgTanks_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Input.Key k = e.Key;
            bool controlKeyIsDown = Keyboard.IsKeyDown(Key.LeftShift);
            if (!controlKeyIsDown &&
               Key.D0 <= k && k <= Key.D9 ||
                 Key.NumPad0 <= k && k <= Key.NumPad9 ||
                 k == Key.Decimal || k == Key.OemPeriod)
            {
                //e.Handled = false;

            }
            else
            {
                e.Handled = true;

            }
        }

        private void dgTanks_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                int TankId, fsmType;
                decimal percentfill;
               
                    TankId = Convert.ToInt16((dgTanks.Items[index] as DataRowView)["Tank_ID"]);
                    
                        if (header == "Volume" || header == "% Fill" || header == "S.G." || header == "Weight")
                        {
                            decimal volume = 0, sg, weight = 0;
                            decimal minsounding = 0;
                            sg = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["SG"]);
                            percentfill = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["Percent_Full"]);

                            decimal maxsounding = maxVolume[TankId];

                            if (header == "Volume")
                            {
                                volume = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["Volume"]);
                                percentfill = Convert.ToDecimal((volume * 100) / maxsounding);
                            }
                            if (header == "% Fill")
                            {
                                volume = (percentfill * maxsounding) / 100;
                            }
                            if (header == "S.G.")
                            {
                                volume = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["Volume"]);
                                weight = volume * sg;
                            }
                            if (header == "Weight")
                            {
                                volume = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["Weight"]) / sg;
                                percentfill = Convert.ToDecimal((volume * 100) / maxsounding);
                            }
                            weight = volume * sg;
                            (dgTanks.Items[index] as DataRowView)["Volume"] = Math.Round(volume, 3);
                            (dgTanks.Items[index] as DataRowView)["Weight"] = Math.Round(weight, 3);
                            (dgTanks.Items[index] as DataRowView)["Percent_Full"] = Math.Round(percentfill, 3);
                            decimal res1 = decimal.Compare(minsounding, volume);
                            decimal res2 = decimal.Compare(volume, maxsounding);
                            int result1 = (int)res1;
                            int result2 = (int)res2;
                            if (result1 > 0 || result2 > 0)
                            {
                                string error = "Volume should be between " + minsounding + " and " + maxsounding;
                                System.Windows.MessageBox.Show(error);
                                // e.Cancel = true;
                                return;
                            }
                            else
                            {
                                string query = "update tblSimulationMode_Tank_Status set [Volume]=" + volume + " where Tank_ID=" + TankId + " "
                                    + "update tblSimulationMode_Tank_Status set SG=" + sg + " where Tank_ID=" + TankId;
                                //query += " update [tblSimulationMode_Loading_Condition] set [Weight]=" + weight + " where Tank_ID=" + TankId;
                                command.CommandText = query;
                                command.CommandType = CommandType.Text;
                                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                            }
                            index = -1;
                            TankId = 0;
                        }

                        if (header == "FSM")
                        {
                            decimal fsm;
                            fsm = Convert.ToDecimal((dgTanks.Items[index] as DataRowView)["FSM"]);
                            fsmType = Convert.ToInt16((dgTanks.Items[index] as DataRowView)["max_1_act_0"]);
                        }
                    
            }
            catch (Exception ex)
            {

            }

        }

        private void dgFixedLoad_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                string Err = "";
                int TankId;
                decimal weight, vcg, lcg, tcg;
                TankId = Convert.ToInt16((dgFixedLoad.Items[index] as DataRowView)["Tank_ID"]);
                weight = Convert.ToDecimal((dgFixedLoad.Items[index] as DataRowView)["Weight"]);
                vcg = Convert.ToDecimal((dgFixedLoad.Items[index] as DataRowView)["VCG"]);
                lcg = Convert.ToDecimal((dgFixedLoad.Items[index] as DataRowView)["LCG"]);
                tcg = Convert.ToDecimal((dgFixedLoad.Items[index] as DataRowView)["TCG"]);

                string query = "update tblSimulationMode_Tank_Status set [Weight]=" + weight + " where Tank_ID=" + TankId + " ";
                query += "update tblSimulationMode_Loading_Condition set [Weight]=" + weight + " where Tank_ID=" + TankId;
                query += "update tblSimulationMode_Loading_Condition set VCG=" + vcg + " where Tank_ID=" + TankId;
                query += " update tblSimulationMode_Loading_Condition set LCG=" + lcg + " where Tank_ID=" + TankId;
                query += " update tblSimulationMode_Loading_Condition set TCG=" + tcg + " where Tank_ID=" + TankId;
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);

            }
            catch
            {
            }
        }

        #region 2D
        private Bounds3D bounds;
        //DxfModel model;
        private WpfWireframeGraphics3DUsingDrawingVisual wpfGraphics;
        private WireframeGraphics2Cache graphicsCache;
        private GraphicsConfig graphicsConfig;
        private Vector3D translation;
        //private Vector3D translationAtMouseClick;
        private double scaling = 1d;
        //private Point2D mouseDownLocation;
        //private bool mouseDown;
        //private Point _initialPoint;
        Assembly assembly = Assembly.GetExecutingAssembly();
        //private System.Windows.Point startPt;
        //private int wid;
        //private int hei;
        //private System.Windows.Point lastLoc;
        //private double CanvasLeft, CanvasTop;
        public void Model(DxfModel model)
        {
            if (model != null)
            {
                DxfLayout paperSpaceLayout = model.ActiveLayout;
                if (model.Header.ShowModelSpace)
                {
                    paperSpaceLayout = null;
                }

                #region calculate the model's bounds to determine a proper dots per inch

                // The dots per inch value is important because it determines the eventual pen thickness.
                graphicsConfig = (GraphicsConfig)GraphicsConfig.WhiteBackgroundCorrectForBackColor.Clone();
                //GraphicsConfig.
                BoundsCalculator boundsCalculator = new BoundsCalculator();
                if (model.ActiveLayout == null || model.Header.ShowModelSpace)
                {
                    boundsCalculator.GetBounds(model);
                }
                else
                {
                    boundsCalculator.GetBounds(model, model.ActiveLayout);
                }
                bounds = boundsCalculator.Bounds;
                Vector3D delta = bounds.Delta;
                System.Windows.Size estimatedCanvasSize = new System.Windows.Size(200d, 200d);
                double estimatedScale = Math.Min(estimatedCanvasSize.Width / delta.X, estimatedCanvasSize.Height / delta.Y);
                graphicsConfig.DotsPerInch = 30d / estimatedScale;
                BoundsCalculator boundsCalculator1 = new BoundsCalculator();
                boundsCalculator1.GetBounds(model, model.Entities[20]);
                #endregion

                graphicsCache = new WireframeGraphics2Cache(false, false);
                graphicsCache.Config = graphicsConfig;
                if (model.ActiveLayout == null || model.Header.ShowModelSpace)
                {
                    graphicsCache.CreateDrawables(model, Matrix4D.Identity);

                }
                else
                {
                    graphicsCache.CreateDrawables(model, model.ActiveLayout);
                }

                wpfGraphics = new WpfWireframeGraphics3DUsingDrawingVisual();
                wpfGraphics.Config = graphicsConfig;

                canvasTwoD.Children.Add(wpfGraphics.Canvas);

                UpdateWpfGraphics();

                canvasTwoD.SizeChanged += canvas_SizeChanged;
              
            }

        }

        private static Bitmap CreateOneUnitToOnePixelBitmap(
        DxfModel model,
        Matrix4D transform,
        GraphicsConfig graphicsConfig,
        SmoothingMode smoothingMode)
        {
            
            System.Drawing.Size maxSize = new System.Drawing.Size(1000, 600);
            return ImageExporter.CreateAutoSizedBitmap(model, Matrix4D.Identity,
                                GraphicsConfig.WhiteBackgroundCorrectForBackColor,
                                SmoothingMode.HighQuality,
                                maxSize);

        }
        private void UpdateWpfGraphics()
        {
            wpfGraphics.DrawingVisuals.Clear();
            IWireframeGraphicsFactory2 graphicsFactory = wpfGraphics.CreateGraphicsFactory();
            foreach (IWireframeDrawable2 drawable in graphicsCache.Drawables)
            {
                drawable.Draw(graphicsFactory);

            }
        }
        private void UpdateRenderTransform()
        {
            double canvasWidth = canvasTwoD.ActualWidth;
            double canvasHeight = canvasTwoD.ActualHeight;
            MatrixTransform baseTransform = DxfUtil.GetScaleWMMatrixTransform(
                (Point2D)bounds.Corner1,
                (Point2D)bounds.Corner2,
                (Point2D)bounds.Center,
                new Point2D(1d, canvasHeight),
                new Point2D(canvasWidth, 1d),
                new Point2D(0.5d * (canvasWidth + 1d), 0.5d * (canvasHeight + 1d))
                );

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(baseTransform);
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = -canvasWidth / 2d,
                Y = -canvasHeight / 2d
            });
            transformGroup.Children.Add(new ScaleTransform()
            {
                ScaleX = scaling,
                ScaleY = scaling
            });
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = canvasWidth / 2d,
                Y = canvasHeight / 2d
            });
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = translation.X * canvasWidth / 2d,
                Y = -translation.Y * canvasHeight / 2d
            });

            canvasTwoD.RenderTransform = transformGroup;
        }

        public void AddHatchProfile()
        {
            try
            {
                // CARGO OIL TANKs
                DrawHatchProfile(canvasTwoD, 1, clsGlobVar.Tank1_PercentFill, clsGlobVar.ProfileCoordinate.Tank1x, clsGlobVar.ProfileCoordinate.Tank1y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 2, clsGlobVar.Tank2_PercentFill, clsGlobVar.ProfileCoordinate.Tank2x, clsGlobVar.ProfileCoordinate.Tank2y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 3, clsGlobVar.Tank3_PercentFill, clsGlobVar.ProfileCoordinate.Tank3x, clsGlobVar.ProfileCoordinate.Tank3y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 4, clsGlobVar.Tank4_PercentFill, clsGlobVar.ProfileCoordinate.Tank4x, clsGlobVar.ProfileCoordinate.Tank4y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 5, clsGlobVar.Tank5_PercentFill, clsGlobVar.ProfileCoordinate.Tank5x, clsGlobVar.ProfileCoordinate.Tank5y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 6, clsGlobVar.Tank6_PercentFill, clsGlobVar.ProfileCoordinate.Tank6x, clsGlobVar.ProfileCoordinate.Tank6y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 7, clsGlobVar.Tank7_PercentFill, clsGlobVar.ProfileCoordinate.Tank7x, clsGlobVar.ProfileCoordinate.Tank7y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 8, clsGlobVar.Tank8_PercentFill, clsGlobVar.ProfileCoordinate.Tank8x, clsGlobVar.ProfileCoordinate.Tank8y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 9, clsGlobVar.Tank9_PercentFill, clsGlobVar.ProfileCoordinate.Tank9x, clsGlobVar.ProfileCoordinate.Tank9y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchProfile(canvasTwoD, 10, clsGlobVar.Tank10_PercentFill, clsGlobVar.ProfileCoordinate.Tank10x, clsGlobVar.ProfileCoordinate.Tank10y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
               

                //FUEL OIL TANKS        
                DrawHatchProfile(canvasTwoD, 11, clsGlobVar.Tank11_PercentFill, clsGlobVar.ProfileCoordinate.Tank11x, clsGlobVar.ProfileCoordinate.Tank11y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 12, clsGlobVar.Tank12_PercentFill, clsGlobVar.ProfileCoordinate.Tank12x, clsGlobVar.ProfileCoordinate.Tank12y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 13, clsGlobVar.Tank13_PercentFill, clsGlobVar.ProfileCoordinate.Tank13x, clsGlobVar.ProfileCoordinate.Tank13y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 14, clsGlobVar.Tank14_PercentFill, clsGlobVar.ProfileCoordinate.Tank14x, clsGlobVar.ProfileCoordinate.Tank14y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 15, clsGlobVar.Tank15_PercentFill, clsGlobVar.ProfileCoordinate.Tank15x, clsGlobVar.ProfileCoordinate.Tank15y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 16, clsGlobVar.Tank16_PercentFill, clsGlobVar.ProfileCoordinate.Tank16x, clsGlobVar.ProfileCoordinate.Tank16y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 17, clsGlobVar.Tank17_PercentFill, clsGlobVar.ProfileCoordinate.Tank17x, clsGlobVar.ProfileCoordinate.Tank17y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 18, clsGlobVar.Tank18_PercentFill, clsGlobVar.ProfileCoordinate.Tank18x, clsGlobVar.ProfileCoordinate.Tank18y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));                                                                                        
                DrawHatchProfile(canvasTwoD, 19, clsGlobVar.Tank19_PercentFill, clsGlobVar.ProfileCoordinate.Tank19x, clsGlobVar.ProfileCoordinate.Tank19y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchProfile(canvasTwoD, 20, clsGlobVar.Tank20_PercentFill, clsGlobVar.ProfileCoordinate.Tank20x, clsGlobVar.ProfileCoordinate.Tank20y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));


                //LUBE OIL TANKS 
                DrawHatchProfile(canvasTwoD, 21, clsGlobVar.Tank21_PercentFill, clsGlobVar.ProfileCoordinate.Tank21x, clsGlobVar.ProfileCoordinate.Tank21y, System.Windows.Media.Color.FromArgb(180, 250, 192, 144));
                DrawHatchProfile(canvasTwoD, 22, clsGlobVar.Tank22_PercentFill, clsGlobVar.ProfileCoordinate.Tank22x, clsGlobVar.ProfileCoordinate.Tank22y, System.Windows.Media.Color.FromArgb(180, 250, 192, 144));


                //FRESH WATER TANKS
                DrawHatchFOT(canvasTwoD, 23, clsGlobVar.Tank23_PercentFill, clsGlobVar.ProfileCoordinate.Tank23x, clsGlobVar.ProfileCoordinate.Tank23y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchProfile(canvasTwoD,24, clsGlobVar.Tank24_PercentFill, clsGlobVar.ProfileCoordinate.Tank24x, clsGlobVar.ProfileCoordinate.Tank24y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));

                //BALLAST TANKS  
                DrawHatchProfile(canvasTwoD, 25, clsGlobVar.Tank25_PercentFill, clsGlobVar.ProfileCoordinate.Tank25x, clsGlobVar.ProfileCoordinate.Tank25y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 26, clsGlobVar.Tank26_PercentFill, clsGlobVar.ProfileCoordinate.Tank26x, clsGlobVar.ProfileCoordinate.Tank26y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 27, clsGlobVar.Tank27_PercentFill, clsGlobVar.ProfileCoordinate.Tank27x, clsGlobVar.ProfileCoordinate.Tank27y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 28, clsGlobVar.Tank28_PercentFill, clsGlobVar.ProfileCoordinate.Tank28x, clsGlobVar.ProfileCoordinate.Tank28y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 29, clsGlobVar.Tank29_PercentFill, clsGlobVar.ProfileCoordinate.Tank29x, clsGlobVar.ProfileCoordinate.Tank29y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 30, clsGlobVar.Tank30_PercentFill, clsGlobVar.ProfileCoordinate.Tank30x, clsGlobVar.ProfileCoordinate.Tank30y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 31, clsGlobVar.Tank31_PercentFill, clsGlobVar.ProfileCoordinate.Tank31x, clsGlobVar.ProfileCoordinate.Tank31y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 32, clsGlobVar.Tank32_PercentFill, clsGlobVar.ProfileCoordinate.Tank32x, clsGlobVar.ProfileCoordinate.Tank32y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchBT(canvasTwoD, 33, clsGlobVar.Tank33_PercentFill, clsGlobVar.ProfileCoordinate.Tank33x, clsGlobVar.ProfileCoordinate.Tank33y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchProfile(canvasTwoD, 34, clsGlobVar.Tank34_PercentFill, clsGlobVar.ProfileCoordinate.Tank34x, clsGlobVar.ProfileCoordinate.Tank34y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchBT(canvasTwoD, 35, clsGlobVar.Tank35_PercentFill, clsGlobVar.ProfileCoordinate.Tank35x, clsGlobVar.ProfileCoordinate.Tank35y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));

                //OTHER TANKS
                DrawHatchOT(canvasTwoD, 36, clsGlobVar.Tank36_PercentFill, clsGlobVar.ProfileCoordinate.Tank36x, clsGlobVar.ProfileCoordinate.Tank36y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchProfile(canvasTwoD, 37, clsGlobVar.Tank37_PercentFill, clsGlobVar.ProfileCoordinate.Tank37x, clsGlobVar.ProfileCoordinate.Tank37y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchProfile(canvasTwoD, 38, clsGlobVar.Tank38_PercentFill, clsGlobVar.ProfileCoordinate.Tank38x, clsGlobVar.ProfileCoordinate.Tank38y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchProfile(canvasTwoD, 39, clsGlobVar.Tank39_PercentFill, clsGlobVar.ProfileCoordinate.Tank39x, clsGlobVar.ProfileCoordinate.Tank39y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchOT(canvasTwoD, 40, clsGlobVar.Tank40_PercentFill, clsGlobVar.ProfileCoordinate.Tank40x, clsGlobVar.ProfileCoordinate.Tank40y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchProfile(canvasTwoD, 41, clsGlobVar.Tank41_PercentFill, clsGlobVar.ProfileCoordinate.Tank41x, clsGlobVar.ProfileCoordinate.Tank41y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchProfile(canvasTwoD, 42, clsGlobVar.Tank42_PercentFill, clsGlobVar.ProfileCoordinate.Tank42x, clsGlobVar.ProfileCoordinate.Tank42y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));

                // Compartment 
                DrawHatchProfile(canvasTwoD, 43, clsGlobVar.Tank43_PercentFill, clsGlobVar.ProfileCoordinate.Tank43x, clsGlobVar.ProfileCoordinate.Tank43y, System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                DrawHatchProfile(canvasTwoD, 44, clsGlobVar.Tank44_PercentFill, clsGlobVar.ProfileCoordinate.Tank44x, clsGlobVar.ProfileCoordinate.Tank44y, System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                DrawHatchProfile(canvasTwoD, 45, clsGlobVar.Tank45_PercentFill, clsGlobVar.ProfileCoordinate.Tank45x, clsGlobVar.ProfileCoordinate.Tank45y, System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                DrawHatchProfile(canvasTwoD, 46, clsGlobVar.Tank46_PercentFill, clsGlobVar.ProfileCoordinate.Tank46x, clsGlobVar.ProfileCoordinate.Tank46y, System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                DrawHatchProfile(canvasTwoD, 47, clsGlobVar.Tank47_PercentFill, clsGlobVar.ProfileCoordinate.Tank47x, clsGlobVar.ProfileCoordinate.Tank47y, System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                
                   
                
                DrawTrimLine();
            }
            catch
            {

            }
        }
        public void AddHatchDeckPlan()
        {
            try
            {
                //CARGO OIL TANKS
             
                DrawHatchDeckPlan(canvasTwoD, 1, clsGlobVar.Tank1_PercentFill, clsGlobVar.Tank1x, clsGlobVar.Tank1y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 2, clsGlobVar.Tank2_PercentFill, clsGlobVar.Tank2x, clsGlobVar.Tank2y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 3, clsGlobVar.Tank3_PercentFill, clsGlobVar.Tank3x, clsGlobVar.Tank3y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 4, clsGlobVar.Tank4_PercentFill, clsGlobVar.Tank4x, clsGlobVar.Tank4y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 5, clsGlobVar.Tank5_PercentFill, clsGlobVar.Tank5x, clsGlobVar.Tank5y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 6, clsGlobVar.Tank6_PercentFill, clsGlobVar.Tank6x, clsGlobVar.Tank6y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 7, clsGlobVar.Tank7_PercentFill, clsGlobVar.Tank7x, clsGlobVar.Tank7y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 8, clsGlobVar.Tank8_PercentFill, clsGlobVar.Tank8x, clsGlobVar.Tank8y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 9, clsGlobVar.Tank9_PercentFill, clsGlobVar.Tank9x, clsGlobVar.Tank9y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));
                DrawHatchDeckPlan(canvasTwoD, 10, clsGlobVar.Tank10_PercentFill, clsGlobVar.Tank10x, clsGlobVar.Tank10y, System.Windows.Media.Color.FromArgb(180, 151, 72, 7));

                //FUEL OIL TANKS 
                DrawHatchDeckPlan(canvasTwoD, 11, clsGlobVar.Tank11_PercentFill, clsGlobVar.Tank11x, clsGlobVar.Tank11y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 12, clsGlobVar.Tank12_PercentFill, clsGlobVar.Tank12x, clsGlobVar.Tank12y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 13, clsGlobVar.Tank13_PercentFill, clsGlobVar.Tank13x, clsGlobVar.Tank13y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 14, clsGlobVar.Tank14_PercentFill, clsGlobVar.Tank14x, clsGlobVar.Tank14y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 15, clsGlobVar.Tank15_PercentFill, clsGlobVar.Tank15x, clsGlobVar.Tank15y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 16, clsGlobVar.Tank16_PercentFill, clsGlobVar.Tank16x, clsGlobVar.Tank16y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 17, clsGlobVar.Tank17_PercentFill, clsGlobVar.Tank17x, clsGlobVar.Tank17y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 18, clsGlobVar.Tank18_PercentFill, clsGlobVar.Tank18x, clsGlobVar.Tank18y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 19, clsGlobVar.Tank19_PercentFill, clsGlobVar.Tank19x, clsGlobVar.Tank19y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));
                DrawHatchDeckPlan(canvasTwoD, 20, clsGlobVar.Tank20_PercentFill, clsGlobVar.Tank20x, clsGlobVar.Tank20y, System.Windows.Media.Color.FromArgb(180, 228, 109, 10));

                //LUBE OIL TANKS 
                DrawHatchDeckPlan(canvasTwoD, 21, clsGlobVar.Tank21_PercentFill, clsGlobVar.Tank21x, clsGlobVar.Tank21y, System.Windows.Media.Color.FromArgb(180, 250, 192, 144));
                DrawHatchDeckPlan(canvasTwoD, 22, clsGlobVar.Tank22_PercentFill, clsGlobVar.Tank22x, clsGlobVar.Tank22y, System.Windows.Media.Color.FromArgb(180, 250, 192, 144));


                //Fresh WATER TANKS 
                DrawHatchDeckPlan(canvasTwoD, 23, clsGlobVar.Tank23_PercentFill, clsGlobVar.Tank23x, clsGlobVar.Tank23y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 24, clsGlobVar.Tank24_PercentFill, clsGlobVar.Tank24x, clsGlobVar.Tank24y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));

                //Ballast Tank
                DrawHatchDeckPlan(canvasTwoD, 25, clsGlobVar.Tank25_PercentFill, clsGlobVar.Tank25x, clsGlobVar.Tank25y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 26, clsGlobVar.Tank26_PercentFill, clsGlobVar.Tank26x, clsGlobVar.Tank26y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 27, clsGlobVar.Tank27_PercentFill, clsGlobVar.Tank27x, clsGlobVar.Tank27y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 28, clsGlobVar.Tank28_PercentFill, clsGlobVar.Tank28x, clsGlobVar.Tank28y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 29, clsGlobVar.Tank29_PercentFill, clsGlobVar.Tank29x, clsGlobVar.Tank29y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 30, clsGlobVar.Tank30_PercentFill, clsGlobVar.Tank30x, clsGlobVar.Tank30y, System.Windows.Media.Color.FromArgb(180, 141, 180, 227));
                DrawHatchDeckPlan(canvasTwoD, 31, clsGlobVar.Tank31_PercentFill, clsGlobVar.Tank31x, clsGlobVar.Tank31y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchDeckPlan(canvasTwoD, 32, clsGlobVar.Tank32_PercentFill, clsGlobVar.Tank32x, clsGlobVar.Tank32y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchDeckPlan(canvasTwoD, 33, clsGlobVar.Tank33_PercentFill, clsGlobVar.Tank33x, clsGlobVar.Tank33y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchDeckPlan(canvasTwoD, 34, clsGlobVar.Tank34_PercentFill, clsGlobVar.Tank34x, clsGlobVar.Tank34y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));
                DrawHatchDeckPlan(canvasTwoD, 35, clsGlobVar.Tank35_PercentFill, clsGlobVar.Tank35x, clsGlobVar.Tank35y, System.Windows.Media.Color.FromArgb(180, 194, 214, 154));


                //OTHER TANK
                DrawHatchDeckPlan(canvasTwoD, 36, clsGlobVar.Tank36_PercentFill, clsGlobVar.Tank36x, clsGlobVar.Tank36y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 37, clsGlobVar.Tank37_PercentFill, clsGlobVar.Tank37x, clsGlobVar.Tank37y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 38, clsGlobVar.Tank38_PercentFill, clsGlobVar.Tank38x, clsGlobVar.Tank38y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 39, clsGlobVar.Tank39_PercentFill, clsGlobVar.Tank39x, clsGlobVar.Tank39y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 40, clsGlobVar.Tank40_PercentFill, clsGlobVar.Tank40x, clsGlobVar.Tank40y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 41, clsGlobVar.Tank41_PercentFill, clsGlobVar.Tank41x, clsGlobVar.Tank41y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));
                DrawHatchDeckPlan(canvasTwoD, 42, clsGlobVar.Tank42_PercentFill, clsGlobVar.Tank42x, clsGlobVar.Tank42y, System.Windows.Media.Color.FromArgb(180, 255, 192, 203));

                //CMP
                DrawHatchDeckPlan(canvasTwoD, 43, clsGlobVar.Tank43_PercentFill, clsGlobVar.Tank43x, clsGlobVar.Tank43y, System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                DrawHatchDeckPlan(canvasTwoD, 44, clsGlobVar.Tank44_PercentFill, clsGlobVar.Tank44x, clsGlobVar.Tank44y, System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                DrawHatchDeckPlan(canvasTwoD, 45, clsGlobVar.Tank45_PercentFill, clsGlobVar.Tank45x, clsGlobVar.Tank45y, System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                DrawHatchDeckPlan(canvasTwoD, 46, clsGlobVar.Tank46_PercentFill, clsGlobVar.Tank46x, clsGlobVar.Tank46y, System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                DrawHatchDeckPlan(canvasTwoD, 47, clsGlobVar.Tank47_PercentFill, clsGlobVar.Tank47x, clsGlobVar.Tank47y, System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
             }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void DrawTrimLine()
        {
            double xAP, yAP, xFP, yFP;
            xAP = 5839.7112;
            yAP = 30436.7495 + (Convert.ToDouble(lblDraftAP.Content)) * 1600;
            xFP = 168184.8062;
            yFP = 30436.7495 + Convert.ToDouble(lblDraftFP.Content) * 1600;
            Polygon p = new Polygon();
            p.Stroke = System.Windows.Media.Brushes.Black;

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 255);
            p.Fill = mySolidColorBrush;

            System.Windows.Point[] point = new System.Windows.Point[5];
            PointCollection pointCollection = new PointCollection();
            pointCollection.Add(new System.Windows.Point(5839.7112, yAP - 400));
            pointCollection.Add(new System.Windows.Point(168184.8062, yFP - 400));
            pointCollection.Add(new System.Windows.Point(168184.8062, yFP));
            pointCollection.Add(new System.Windows.Point(5839.7112, yAP));

            p.Points = pointCollection;
            canvasTwoD.Children.Add(p);
        }

        public void DrawHatchDeckPlan(Canvas canvasTwoD,int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                {
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;

                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                    p.Fill = mySolidColorBrush;

                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    for (int index = 1; index <= 17; index++)
                    {
                        //pointCollection.Add(new System.Windows.Point(xx[index], yy[index]));
                        pointCollection.Add(new System.Windows.Point(xx[index], yy[index]));

                    }
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);


                }
                else
                    if (percent > 0 && percent <= 100)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        for (int index = 1; index <= 17; index++)
                        {
                            //pointCollection.Add(new System.Windows.Point(xx[index], yy[index]));
                            pointCollection.Add(new System.Windows.Point(xx[index], yy[index]));

                        }
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        for (int index = 1; index <= 17; index++)
                        {
                            pointCollection.Add(new System.Windows.Point(xx[index], yy[index]));
                        }
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
            }
            catch
            {
            }
        }
        public void DrawHatchProfile(Canvas canvasTwoD, int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                {
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(180, 255, 0, 0);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[2]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);

                }

                else if (percent > 0 && percent <= 100)
                {
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = color;
                    p.Fill = mySolidColorBrush;
                    double d = yy[2] - yy[1];
                    double Fill = Convert.ToInt32(percent) * (d / 100);
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[1] + Fill));
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1] + Fill));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    percent = 0;
                }
                else if (percent == 0)
                {
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[2]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                }
            }
            catch(Exception ex)
                 
            {

            }
        }



        public void DrawHatchBT(Canvas canvasTwoD,int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Tank_ID == 33)
                {
                    xx = new double[] { 0, 7017.9913, 7069.8286, 13558.9431, 13558.9442 };
                    yy = new double[] { 0, 42232.7682, 39394.2528, 37915.374, 42163.5056 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 40)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[2] - yy[3];
                        double dx = xx[3] - xx[2];
                        double Fill = Convert.ToInt32(percent) * (d / 40);
                        double Fillx = Convert.ToInt32(percent) * (dx / 40);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[4]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[3] + Fillx, yy[3] + Fill));
                      
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 40 && percent <= 100)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[4] - yy[2];
                        //double dx = xx[3] - xx[2];
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                       // double Fillx = Convert.ToInt32(percent) * (dx / 100);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[2]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                       // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                    }
                }

                if (Tank_ID == 35)
                {
                    xx = new double[] { 0,155641.0545,	161409.2796,	166795.0117,	167219.9023,	166115.5667,	163552.7525,	163552.7525,	164801.3888,	155641.0545 };
                    yy = new double[] { 0, 30436.7495,	30830.5914,	    33797.8688,  	36353.1773,	    37502.8668,	    38836.5675,	    39708.3706,  	42389.818,	    42292.09 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 20)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[3] - yy[1];
                        double dx = xx[3] - xx[1];
                        double Fill = Convert.ToInt32(percent) * (d / 20);
                        double Fillx = Convert.ToInt32(percent) * (dx / 20);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[1] + Fillx, yy[1] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1] + Fill));
                        

                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 20 && percent <= 50)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[4] - yy[3];
                        //double dx = xx[3] - xx[2];
                        double Fill = Convert.ToInt32(percent) * (d / 50);
                        // double Fillx = Convert.ToInt32(percent) * (dx / 100);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 50 && percent <= 65)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[5] - yy[4];
                        double dx = xx[5] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 65);
                         double Fillx = Convert.ToInt32(percent) * (dx / 65);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4]+Fillx, yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[4] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 65 && percent <= 80)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[7] - yy[5];
                        double dx = xx[7] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 80);
                        double Fillx = Convert.ToInt32(percent) * (dx / 80);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[5] + Fillx, yy[5] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[5] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 80 && percent <= 100)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[8] - yy[7];
                        double dx = xx[8] - xx[7];
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                        double Fillx = Convert.ToInt32(percent) * (dx / 100);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7] + Fillx, yy[7] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[7] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                    }
                }
            }
            catch
            {

            }
        }
        public void DrawHatchFOT(Canvas canvasTwoD,int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Tank_ID == 23)
                {
                    xx = new double[] { 0, 13556.8314, 13558.9431, 15032.9677, 16562.7261, 18383.463, 18383.463, };
                    yy = new double[] { 0, 42163.5028, 37915.374, 37579.4416, 36034.7074, 36034.7074, 42120.4289 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 20)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[3] - yy[5];
                        double dx = xx[3] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 20);
                        double Fillx = Convert.ToInt32(percent) * (dx / 20);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[5] + Fillx, yy[5] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4] , yy[4] ));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 20 && percent <= 35)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[2] - yy[3];
                        double dx = xx[2] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 35);
                         double Fillx = Convert.ToInt32(percent) * (dx /35);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[5]+Fillx, yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 35 && percent <= 100)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[1] - yy[2];
                        
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                       
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                      //  pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                }
            }
            catch
            {
            }

        }
        public void DrawHatchFWT(Canvas canvasTwoD,int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Tank_ID == 229)
                {
                    xx = new double[] { 0, 99327.9544, 103519.3438, 105051.8783, 106381.3057, 107193.7336, 107673.8046, 107636.8761, 107009.091, 105975.0917, 104867.2354, 104756.4499, 104756.4499, 104941.0927, 105735.0564, 99327.9544 };
                    yy = new double[] { 0, 20713.521, 20787.3286, 21137.9143, 21820.6342, 22817.0361, 24145.572, 25566.3674, 26747.2881, 27245.489, 27448.4599, 27632.9787, 28666.2844, 29201.3891, 30234.6948, 30179.3391 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        //  pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[10], yy[10]));
                        pointCollection.Add(new System.Windows.Point(xx[11], yy[11]));
                        pointCollection.Add(new System.Windows.Point(xx[12], yy[12]));
                        pointCollection.Add(new System.Windows.Point(xx[13], yy[13]));
                        pointCollection.Add(new System.Windows.Point(xx[14], yy[14]));
                        pointCollection.Add(new System.Windows.Point(xx[15], yy[15]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 5)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[3] - yy[1];
                        double dx = xx[3] - xx[1];
                        double Fill = Convert.ToInt32(percent) * (d / 15);
                        double Fillx = Convert.ToInt32(percent) * (dx / 15);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[2] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 5 && percent <= 40)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[5] - yy[4];
                        double dx = xx[5] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 40);
                        double Fillx = Convert.ToInt32(percent) * (dx / 40);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4] + Fillx, yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[4] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 40 && percent <= 50)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[6] - yy[5];
                        double dx = xx[6] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 50);
                        double Fillx = Convert.ToInt32(percent) * (dx / 50);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[5] + Fillx, yy[5] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[5] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 50 && percent <= 60)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[7] - yy[6];
                        double dx = xx[7] - xx[6];
                        double Fill = Convert.ToInt32(percent) * (d / 60);
                        double Fillx = Convert.ToInt32(percent) * (dx / 60);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[6] + Fillx, yy[6] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[6] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 60 && percent <= 70)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[8] - yy[7];
                        double dx = xx[8] - xx[7];
                        double Fill = Convert.ToInt32(percent) * (d / 70);
                        double Fillx = Convert.ToInt32(percent) * (dx / 70);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[7] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[7] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 70 && percent <= 80)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[10] - yy[8];
                        double dx = xx[10] - xx[8];
                        double Fill = Convert.ToInt32(percent) * (d / 80);
                        double Fillx = Convert.ToInt32(percent) * (dx / 80);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[8] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[8] + Fill));

                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 80 && percent <= 90)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[12] - yy[10];
                        double dx = xx[12] - xx[10];
                        double Fill = Convert.ToInt32(percent) * (d / 90);
                        double Fillx = Convert.ToInt32(percent) * (dx / 90);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[10], yy[10]));
                        pointCollection.Add(new System.Windows.Point(xx[11], yy[10] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[10] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 90 && percent <= 100)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[15] - yy[12];
                        double dx = xx[14] - xx[12];
                        double Fill = Convert.ToInt32(percent) * (d / 110);
                        double Fillx = Convert.ToInt32(percent) * (dx / 110);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[10], yy[10]));
                        pointCollection.Add(new System.Windows.Point(xx[11], yy[11]));
                        pointCollection.Add(new System.Windows.Point(xx[12], yy[12]));
                        pointCollection.Add(new System.Windows.Point(xx[13] + Fillx, yy[13] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[14], yy[13] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[15]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                       // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                }
                if (Tank_ID == 30)
                {
                    xx = new double[] { 0, 1234.3533, 1455.3586, 4448.1011, 6016.534, 6942.7278, 9016.534, 9016.534, 8965.703, 8965.703 };
                    yy = new double[] { 0, 29528.8129, 27472.7465, 26561.7069, 26084.2503, 25488.2292, 25488.2292, 26433.2449, 27258.3545, 29539.54 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        //pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 10)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[4] - yy[5];
                        double dx = xx[6] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 10);
                        double Fillx = Convert.ToInt32(percent) * (dx / 10);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[5] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[5] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }

                    else if (percent > 10 && percent <= 20)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[4] - yy[5];
                        double dx = xx[3] - xx[5];
                        double Fill = Convert.ToInt32(percent) * (d / 20);
                        double Fillx = Convert.ToInt32(percent) * (dx / 20);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4] + Fillx, yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 20 && percent <= 60)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[2] - yy[3];
                        double dx = xx[2] - xx[3];
                        double Fill = Convert.ToInt32(percent) * (d / 60);
                        double Fillx = Convert.ToInt32(percent) * (dx / 60);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[3] + Fillx, yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[3] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[2]));

                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 60 && percent <= 100)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[1] - yy[2];
                        double dx = xx[2] - xx[3];
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                        double Fillx = Convert.ToInt32(percent) * (dx / 100);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();

                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[3] + Fillx, yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[2] + Fill));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }

                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                       // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                    }
                }
            }
            catch
            {
            }

        }
        public void DrawHatchOT(Canvas canvasTwoD, int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
                if (Tank_ID == 36)
                {
                    xx = new double[] { 0, 18383.463,  18383.463,  20894.8783, 23207.9829, 23207.9829 };
                    yy = new double[] { 0, 32100.3465, 30861.1528, 30436.7495, 30436.7495, 32100.3465 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[10], yy[10]));
                        pointCollection.Add(new System.Windows.Point(xx[11], yy[11]));
                        pointCollection.Add(new System.Windows.Point(xx[12], yy[12]));
                        pointCollection.Add(new System.Windows.Point(xx[13], yy[13]));
                        pointCollection.Add(new System.Windows.Point(xx[14], yy[14]));
                        pointCollection.Add(new System.Windows.Point(xx[15], yy[15]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 20)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[2] - yy[1];
                        double dx = xx[2] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 20);
                        double Fillx = Convert.ToInt32(percent) * (dx / 20);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4]+Fillx, yy[4]+Fill + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 20 && percent <= 100)
                    {

                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;
                        double d = yy[5] - yy[2];
                        //double dx = xx[5] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                        //double Fillx = Convert.ToInt32(percent) * (dx / 40);
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[2]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[2]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                   
                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        // pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                }
                if (Tank_ID == 40)
                {
                    xx = new double[] { 0, 14720.1129,	15667.5223,	16816.2488,	18383.463,	18383.463,	18383.463,	16562.7261,	15933.1821,	14732.5209 };
                    yy = new double[] { 0, 33577.9283,	32341.479,	31494.899,	30861.1528,	34149.5841,	36034.7074,	36034.7074,	34911.8114,	34696.1005 };

                    if (Convert.ToBoolean(Models.clsGlobVar.dtSimulationAllTanks.Rows[Tank_ID - 1]["IsDamaged"]) == true)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(220, 255, 0, 0);
                        p.Fill = mySolidColorBrush;

                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        //pointCollection.Add(new System.Windows.Point(xx[0], yy[0]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);

                    }
                    else if (percent > 0 && percent <= 20)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[2] - yy[4];
                        double dx = xx[2] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 20);
                        double Fillx = Convert.ToInt32(percent) * (dx / 20);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4]+Fillx, yy[4] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }

                    else if (percent > 20 && percent <= 40)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[1] - yy[2];
                        double dx = xx[1] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 40);
                        double Fillx = Convert.ToInt32(percent) * (dx / 40);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4] + Fillx, yy[2] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 40 && percent <= 60)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[9] - yy[1];
                       
                        double Fill = Convert.ToInt32(percent) * (d / 60);
                       
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[1]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[1] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));

                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }
                    else if (percent > 60 && percent <= 100)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = color;
                        p.Fill = mySolidColorBrush;

                        double d = yy[6] - yy[8];
                        double dx = xx[8] - xx[4];
                        double Fill = Convert.ToInt32(percent) * (d / 100);
                        double Fillx = Convert.ToInt32(percent) * (dx / 100);
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();

                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[8]+Fill));
                        pointCollection.Add(new System.Windows.Point(xx[4] + Fillx, yy[8] + Fill));
                        pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                        pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));

                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                        percent = 0;
                    }

                    else if (percent == 0)
                    {
                        Polygon p = new Polygon();
                        p.Stroke = System.Windows.Media.Brushes.Black;

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                        p.Fill = mySolidColorBrush;
                        System.Windows.Point[] point = new System.Windows.Point[25];
                        PointCollection pointCollection = new PointCollection();
                        pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                        pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                        pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                        pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                        pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                        pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                        p.Points = pointCollection;
                        canvasTwoD.Children.Add(p);
                    }
                }
            }
            catch
            {
            }

        }
        public void DrawHatchCMP(Canvas canvasTwoD, int Tank_ID, decimal percent, double[] xx, double[] yy, System.Windows.Media.Color color)
        {
            try
            {
            }
            catch
            { }
        }



        private void DrawHatchDeckPlanSelection()
        {
            try
            {
                Polygon p = new Polygon();
                p.Stroke = System.Windows.Media.Brushes.Black;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                p.Fill = mySolidColorBrush;
                p.UseLayoutRounding = true;
                p.StrokeThickness = 1;
                if (isHatchSelectionProfile == true && canvasTwoD.Children.Count > 1)
                {
                    canvasTwoD.Children.RemoveRange(canvasTwoD.Children.Count - 2, 3);
                }
                double[] Tankx = new double[18];
                double[] Tanky = new double[18];
                var currentRowIndex = dgTanks.Items.IndexOf(dgTanks.CurrentItem);
                PointCollection pointCollection = new PointCollection();

                for (int i = 1; i <= 17; i++)
                {
                    string sc = Convert.ToString("X" + i);
                    string sr = Convert.ToString("Y" + i);
                    Tankx[i] = Convert.ToDouble(Models.clsGlobVar.dsCoordinates.Tables[0].Rows[currentRowIndex][sc]);
                    Tanky[i] = Convert.ToDouble(Models.clsGlobVar.dsCoordinates.Tables[0].Rows[currentRowIndex][sr]);
                    pointCollection.Add(new System.Windows.Point(Tankx[i], Tanky[i]));
                }

                p.Points = pointCollection;
                canvasTwoD.Children.Add(p);
                // isHatchSelection = true;
            }
            catch
            {

            }
        }
        private void DrawHatchProfileSelection()
        {
            try
            {
                double[] Tankx = new double[3];
                double[] Tanky = new double[3];
                var currentRowIndex = dgTanks.Items.IndexOf(dgTanks.CurrentItem);

                if (currentRowIndex == 32)
                {
                    double[] xx = new double[] { 0, 7017.9913,	7069.8286,	13558.9431,	13558.9442 };
                    double[] yy = new double[] { 0,42232.7682,	39394.2528,	37915.374,	42163.5056};
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;

                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                    pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;

                }

                else if (currentRowIndex == 22)
                {
                    double[] xx = new double[] { 0, 13556.8314,	13558.9431,	15032.9677,	16562.7261,	18383.463,	18383.463,};
                    double[] yy = new double[] { 0, 42163.5028,	37915.374,	37579.4416,	36034.7074,	36034.7074,	42120.4289};

                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                    pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                    pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                    pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;
                }
                else if (currentRowIndex == 35)
                {
                    double[] xx = new double[] { 0, 18383.463,	18383.463,	20894.8783,	23207.9829,	23207.9829};
                    double[] yy = new double[] { 0, 32100.3465,	30861.1528,	30436.7495,	30436.7495,	32100.3465};

                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                    pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                    pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;

                }
                else if (currentRowIndex == 39)
                {
                    double[] xx = new double[] { 0,14720.1129,	15667.5223,	16816.2488,	18383.463,	18383.463,	18383.463,	16562.7261,	15933.1821,	14732.5209};
                    double[] yy = new double[] { 0,33577.9283,	32341.479,	31494.899,	30861.1528,	34149.5841,	36034.7074,	36034.7074,	34911.8114,	34696.1005};

                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                    pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                    pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                    pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                    pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                    pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                    pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;
                }
                else if (currentRowIndex == 34)
                {
                    double[] xx = new double[] { 0, 155641.0545,	161409.2796,	166795.0117,	167219.9023,	166115.5667,	163552.7525,	163552.7525,	164801.3888,	155641.0545 };
                    double[] yy = new double[] { 0, 30436.7495,	    30830.5914,	    33797.8688,	    36353.1773,	    37502.8668,	    38836.5675,	    39708.3706,	    42389.818,	    42292.09 };

                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    System.Windows.Point[] point = new System.Windows.Point[25];
                    PointCollection pointCollection = new PointCollection();
                    pointCollection.Add(new System.Windows.Point(xx[1], yy[1]));
                    pointCollection.Add(new System.Windows.Point(xx[2], yy[2]));
                    pointCollection.Add(new System.Windows.Point(xx[3], yy[3]));
                    pointCollection.Add(new System.Windows.Point(xx[4], yy[4]));
                    pointCollection.Add(new System.Windows.Point(xx[5], yy[5]));
                    pointCollection.Add(new System.Windows.Point(xx[6], yy[6]));
                    pointCollection.Add(new System.Windows.Point(xx[7], yy[7]));
                    pointCollection.Add(new System.Windows.Point(xx[8], yy[8]));
                    pointCollection.Add(new System.Windows.Point(xx[9], yy[9]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;
                }
                else
                {
                    Polygon p = new Polygon();
                    p.Stroke = System.Windows.Media.Brushes.Black;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(160, 120, 120, 120);
                    p.Fill = mySolidColorBrush;
                    p.UseLayoutRounding = true;
                    p.StrokeThickness = 1;
                    PointCollection pointCollection = new PointCollection();
                    for (int i = 1; i <= 2; i++)
                    {
                        string sc = Convert.ToString("X" + i);
                        string sr = Convert.ToString("Y" + i);
                        Tankx[i] = Convert.ToDouble(Models.clsGlobVar.dsCoordinates.Tables[1].Rows[currentRowIndex][sc]);
                        Tanky[i] = Convert.ToDouble(Models.clsGlobVar.dsCoordinates.Tables[1].Rows[currentRowIndex][sr]);
                    }
                    pointCollection.Add(new System.Windows.Point(Tankx[1], Tanky[1]));
                    pointCollection.Add(new System.Windows.Point(Tankx[2], Tanky[1]));
                    pointCollection.Add(new System.Windows.Point(Tankx[2], Tanky[2]));
                    pointCollection.Add(new System.Windows.Point(Tankx[1], Tanky[2]));
                    p.Points = pointCollection;
                    canvasTwoD.Children.Add(p);
                    isHatchSelectionProfile = true;
                }
            }
            catch
            {

            }
        }
        private void canvasTwoD_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //UpdateRenderTransform();
        }
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            UpdateRenderTransform();
        }

        public static void CreateBitmapFromVisual(Visual target, string fileName)
        {
            try
            {
                if (target == null || string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
                bounds.Width = bounds.Width;
                bounds.Height = bounds.Height;

                //Rect bounds = { -7209.99755859375, -1050.822265625, 247970.497558594, 91811.400390625 };
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width / 40, (Int32)bounds.Height / 40, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual visual = new DrawingVisual();

                using (DrawingContext context = visual.RenderOpen())
                {
                    VisualBrush visualBrush = new VisualBrush(target);
                    //visualBrush.RelativeTransform = new ScaleTransform { ScaleX = -1, ScaleY = -1 };
                    context.DrawRectangle(visualBrush, null, new Rect(0, 0, bounds.Width / 40, bounds.Height / 40));
                }

                renderTarget.Render(visual);

                PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();

                bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
                using (Stream stm = File.Create(fileName))
                {
                    bitmapEncoder.Save(stm);
                }

                // Bitmap original = (Bitmap)System.Drawing.Image.FromFile("Img.bmp");
                //Bitmap resized = new Bitmap(original, new Size(original.Width / 4, original.Height / 4));
                //resized.Save("DSC_0002_thumb.jpg");
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

        }
        #endregion 2D

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(System.Windows.Controls.DataGridCell))
            {
                // Starts the Edit on the row;
                System.Windows.Controls.DataGrid grd = (System.Windows.Controls.DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        private void dgFixedLoad_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Input.Key k = e.Key;

            bool controlKeyIsDown = Keyboard.IsKeyDown(Key.LeftShift);
            if (!controlKeyIsDown &&
                Key.D0 <= k && k <= Key.D9 ||
                Key.NumPad0 <= k && k <= Key.NumPad9 ||
                k == Key.Decimal || k == Key.OemPeriod || k == Key.OemMinus)
            {
            }

            else
            {
                e.Handled = true;
            }

        }

        private void OpenUserManual_Click(object sender, RoutedEventArgs e)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\User Manual_ZebecLoadMaster_MT ARK PIONEER.Pdf";
            System.Diagnostics.Process.Start(path);

            //UserManualWindow userManualWindow = new UserManualWindow();
            //userManualWindow.Show();
        }

        private void dgLongitudinal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDamageCases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbDamageCases.SelectedIndex >= 0)
                {
                    SFBMCondition.IsEnabled = false;
                    string cmbSelected = cmbDamageCases.SelectedItem.ToString();
                    CmprItem = Convert.ToString(cmbSelected.Split(':')[1]);
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        btnGenerateReport.IsEnabled = false;
                        btnSaveLoadingCondition.IsEnabled = false;
                        clsGlobVar.FlagDamageCases = true;
                        DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                        string Err = "", cmd = " ";
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i < 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }

                        if (CmprItem.Trim() == "Damage Case-01")
                        {
                            clsGlobVar.DamageCase = "Damage Case-01";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (34,24,30) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-02")
                        {
                            clsGlobVar.DamageCase = "Damage Case-02";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (41,31,32) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-03")
                        {
                            clsGlobVar.DamageCase = "Damage Case-03";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in ( 12,46,41,32,8)";
                        }
                        else if (CmprItem.Trim() == "Damage Case-04")
                        {
                            clsGlobVar.DamageCase = "Damage Case-04";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (29,30) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-05")
                        {
                            clsGlobVar.DamageCase = "Damage Case-05";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in ( 30,6) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-06")
                        {
                            clsGlobVar.DamageCase = "Damage Case-06";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in ( 27,28) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-07")
                        {
                            clsGlobVar.DamageCase = "Damage Case-07";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in ( 28,8) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-08")
                        {
                            clsGlobVar.DamageCase = "Damage Case-08";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (25,26) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-09")
                        {
                            clsGlobVar.DamageCase = "Damage Case-09";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (25,2,10) ";
                        }
                        else if (CmprItem.Trim() == "Damage Case-10")
                        {
                            clsGlobVar.DamageCase = "Damage Case-10";
                            txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                            cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in ( 43,35,45) ";
                        }
                        
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        clsGlobVar.FlagDamageCases = false;
                        Mouse.OverrideCursor = null;
                    }
                }
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }

        }

        private void SFBMCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int sfbmSelected = SFBMCondition.SelectedIndex;

                if (sfbmSelected == 0)
                {

                    DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    string Err = "", cmd = " ";
                    cmd = "Update [tblMaster_Config_Addi] set [Sea_1_Port_0]=1 ";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                }
                else if (sfbmSelected == 1)
                {

                    DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    string Err = "", cmd = " ";
                    cmd = "Update [tblMaster_Config_Addi] set [Sea_1_Port_0]=0 ";
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);

                }

            }
            catch
            {
                Mouse.OverrideCursor = null;
            }

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object a = e.Source;
                System.Windows.Controls.CheckBox chk = (System.Windows.Controls.CheckBox)sender;
                chk1 = chk;

                DataGridRow row = FindAncestor<DataGridRow>(chk);
                if (chk.IsChecked == true) { CheckNCheckOutCount++; } else { CheckNCheckOutCount--; }
                if (row != null)
                {
                    DataRowView rv = (DataRowView)row.Item;

                    string Err = "";
                    string query;
                    query = "update [tblSimulationMode_Tank_Status] set [IsDamaged]='" + (bool)chk.IsChecked + "' where Tank_ID=" + rv["Tank_ID"];
                    DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                }
            }

            catch
            {
            }
        }

        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            };
            return null;
        }

        public void printToPdf_DamageCases()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Document doc = new Document(iTextSharp.text.PageSize.A4, 30, 30, 30, 20);
                reportPath = System.Windows.Forms.Application.StartupPath + "\\Reports\\Damage Cases\\" + ISO_Date() + "_Report.pdf";
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(reportPath, FileMode.Create));

                doc.Open();//Open Document to write
                wri.PageEvent = new pdfFormating();

                iTextSharp.text.Paragraph projecttit = new iTextSharp.text.Paragraph("FAIRYTALE", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // Project  Name
                projecttit.Alignment = Element.ALIGN_CENTER;
                doc.Add(projecttit);
                iTextSharp.text.Paragraph projecttitl = new iTextSharp.text.Paragraph("Damage Stability Summary Report ", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); 
                projecttitl.Alignment = Element.ALIGN_CENTER;
                doc.Add(projecttitl);

                //...........StartOFLogo.........................................
                iTextSharp.text.Image LogoWatermark = iTextSharp.text.Image.GetInstance(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\Watermark.jpg");
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(LogoWatermark);
                pic.Alignment = Element.ALIGN_LEFT;
                pic.ScaleToFit(70, 40);
                doc.Add(pic);

                iTextSharp.text.Image logoMdl = iTextSharp.text.Image.GetInstance(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\Integrity Logo.PNG");
                iTextSharp.text.Image pic1 = iTextSharp.text.Image.GetInstance(logoMdl);
                pic1.Alignment = Element.ALIGN_RIGHT;
                pic1.ScaleToFit(70, 50);
                pic1.SetAbsolutePosition(485, 740);  //Sangita
                doc.Add(pic1);
                iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph("Stability Status", FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.BOLD)); // Loading Summary Table Name
                p3.Alignment = Element.ALIGN_CENTER;
                doc.Add(p3);
                doc.Add(new iTextSharp.text.Paragraph("  "));
                doc.Add(new iTextSharp.text.Paragraph("  "));
                iTextSharp.text.Font fntHeader = FontFactory.GetFont("Times New Roman", 8, iTextSharp.text.Color.BLUE);   //Header
                iTextSharp.text.Font fntBody = FontFactory.GetFont("Times New Roman", 6.5f);   // Body
                //..........StartofLoadingSummaryPart1..............................

                doc.Add(new iTextSharp.text.Paragraph("  "));
                doc.Add(new iTextSharp.text.Paragraph("  "));
                PdfPTable tbldamageSummary = new PdfPTable(2);
                tbldamageSummary.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbldamageSummary.WidthPercentage = 50;
                float[] widthsLoading = new float[] { 2.5f, 1.8f };      //in case nishant decide to incorporate again add 2nd and 3rd column as 1f  
                tbldamageSummary.SetWidths(widthsLoading);
                tbldamageSummary.HorizontalAlignment = Element.ALIGN_CENTER;
                iTextSharp.text.Font fntRedColor = FontFactory.GetFont("Times New Roman", 1, iTextSharp.text.Color.ORANGE);// Body

                PdfPCell dmgcases = new PdfPCell(new Phrase("Damage Cases", fntHeader));
                dmgcases.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell criteria = new PdfPCell(new Phrase("Status", fntHeader));
                criteria.HorizontalAlignment = Element.ALIGN_CENTER;

                //Add header to table
                tbldamageSummary.AddCell(dmgcases);
                tbldamageSummary.AddCell(criteria);

                DataTable dtDamageSummary = clsGlobVar.dtDamageCriteria.Clone();
                foreach (DataRow dr in clsGlobVar.dtDamageCriteria.Rows)
                {

                    dtDamageSummary.Rows.Add(dr.ItemArray);
                }
                int columnCountFloodPoint = dtDamageSummary.Columns.Count;
                int rowCountFloodPoint = dtDamageSummary.Rows.Count;
                for (int rowCounter = 0; rowCounter < rowCountFloodPoint; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < columnCountFloodPoint; columnCounter++)
                    {

                        string strValue = (dtDamageSummary.Rows[rowCounter][columnCounter].ToString());
                        PdfPCell pdf = new PdfPCell(new iTextSharp.text.Paragraph(strValue, fntBody));
                        pdf.HorizontalAlignment = Element.ALIGN_CENTER;
                        tbldamageSummary.AddCell(pdf);
                    }
                }
                doc.Add(tbldamageSummary);

                doc.Close();
                Mouse.OverrideCursor = null;
                System.Windows.MessageBox.Show("PDF Created!");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnAldamage_Click(object sender, RoutedEventArgs e)
        {
            _DamageStabilityCnt = 0;
          //  cmbDamageCases.Text = "Select damage Case";
            string st = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string rpath = st + "\\Reports\\AllDamageCases";
            //  string[] fileNames = Directory.GetFiles(rpath);

            DirectoryInfo di = new DirectoryInfo(rpath);
            FileInfo[] files = di.GetFiles();
            string hh = string.Empty;
            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    hh = Convert.ToString(files[i]);
                    File.Delete(rpath + "\\" + hh);
                }
            }

            var result = System.Windows.MessageBox.Show("Do You Want To start Calculation of All Damage Cases ?", " ", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                #region
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    btnGenerateReport.IsEnabled = false;
                    btnSaveLoadingCondition.IsEnabled = false;
                    clsGlobVar.FlagDamageCases = true;
                    DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                    string Err = "", cmd = " ";
                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-01";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (55,56,57,31,29) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                      
                        DamageAll = 1;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-02";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (55,56,57,29,31,3,1,44,32) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 2;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-03";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (55,56,57,29,31,2 ,1 ,43,32)";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 3;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-04";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (26,3 ,44,32,6,46,1) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 4;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-05";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in ( 25,2,43,32,5,45,1) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 5;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-06";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID] in (26,9,48,34,6,46,58) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 6;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-07";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (25,8,47,33,5,45,59) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 7;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-08";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (9,48,34,12,50) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 8;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-09";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (8,47,33,11,49) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 9;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);
                    }

                    {
                        cmd = "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=0 where [User]='dbo' and [Tank_ID] between 1 and 46 ";
                        for (int i = 0; i <= 45; i++)
                        {
                            cmd += "Update [tblSimulationMode_Tank_Status] set [Volume]=" + dtIntactCondition.Rows[i]["Volume"].ToString() + " where [User]='dbo' and [Tank_ID]=" + dtIntactCondition.Rows[i]["Tank_ID"].ToString() + " ";
                        }
                        clsGlobVar.DamageCase = "Damage Case-10";
                        txtLoadingConditionName.Text = clsGlobVar.DamageCase;
                        cmd += "Update [tblSimulationMode_Tank_Status] set [IsDamaged]=1 where [User]='dbo' and [Tank_ID]in (15, 52,34,12,60,50,36) ";
                        command.CommandText = cmd;
                        command.CommandType = CommandType.Text;
                        Models.DAL.clsDBUtilityMethods.ExecuteNonQuery(command, Err);
                        Models.TableModel.SimulationModeData();
                        dgTanks.ItemsSource = clsGlobVar.dtSimulationAllTanksForDamage.DefaultView;
                        DamageAll = 10;
                        _DamageStabilityCnt++;
                        CmprItem = "All Damage Cases";
                        this.btnCalculate_Click(sender, e);
                        this.btnGenerateReport_Click(sender, e);

                        _DamageStabilityCnt = 0;
                    }
                }
                 #endregion
                DamageAll = 0;
                pbCalculation.Value = 0;
                clsGlobVar.FlagDamageCases = false;
                CmprItem = "";

                DataTable _myDataTable = new DataTable();
                for (int i = 0; i < 2; i++)
                {
                    _myDataTable.Columns.Add();
                }
                for (int i = 0; i < 10; i++)
                {
                   DataRow row = _myDataTable.NewRow();
                    for (int j = 0; j < 2; j++)
                    {
                        row[j] = _statusOKnNotOK[i, j];
                    }
                    _myDataTable.Rows.Add(row);
                    Models.clsGlobVar.dtDamageCriteria = _myDataTable;
                    Mouse.OverrideCursor = null;
                }
                dtStabilityStatus.ItemsSource = Models.clsGlobVar.dtDamageCriteria.DefaultView;
                printToPdf_DamageCases();
                dt1StabilityStatus.IsEnabled = true;
                dt1StabilityStatus.IsSelected = true;
                dt1StabilityStatus.Focus();
            }
            
        }

        private void canvasTwoD_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        { 
            double IDSelect = 0;
            double[] ShadeX;
            double[] ShadeY;
            if (canvasTwoD.IsLoaded == true)
            {
               System.Windows.Point tp = e.GetPosition(this.canvasTwoD);
                double xx= tp.X;
                double yy = tp.Y;
                canvasTwoD.Children.RemoveRange(1, canvasTwoD.Children.Count - 1);
                AddHatchProfile();
                AddHatchDeckPlan();
                ShadeX = new double[5];
                ShadeY = new double[5];
               
                for (int i = 0; i <= 68; i++)
                    {
                        for (int j = 1; j < 2; j++)

                            if ((xx <= clsGlobVar.ProfileCoordinate.mul[i, j]) && (xx >= clsGlobVar.ProfileCoordinate.mul[i, j + 1]))
                            {
                                IDSelect = clsGlobVar.ProfileCoordinate.mul[i, j - 1];

                                for (int p = 0; p <= 68; p++)
                                {
                                    for (int n = 1; n < 2; n++)
                                    {
                                        if ((yy <= clsGlobVar.ProfileCoordinate.mul[p, n + 2]) && (yy >= clsGlobVar.ProfileCoordinate.mul[p, n + 3]) && (clsGlobVar.ProfileCoordinate.mul[p, n - 1] == IDSelect))
                                        {
                                            IDSelect = clsGlobVar.ProfileCoordinate.mul[i, j - 1];
                                            string sCmd = "Select * from [Profile_View] where Tank_ID='" + IDSelect + "'";
                                            DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                                            command.CommandText = sCmd;
                                            command.CommandType = CommandType.Text;
                                            string Err = "";
                                            DataTable dtcoordinateProfile = new DataTable();
                                            dtcoordinateProfile = Models.DAL.clsDBUtilityMethods.GetTable(command, Err);
                                            for (int o = 1; o <= 2; o++)
                                            {
                                                string sc = Convert.ToString("X" + o);
                                                string sr = Convert.ToString("Y" + o);
                                                ShadeX[o] = Convert.ToDouble(dtcoordinateProfile.Rows[0][sc]);
                                                ShadeY[o] = Convert.ToDouble(dtcoordinateProfile.Rows[0][sr]);
                                            }
                                            TankID = Convert.ToInt16(IDSelect);
                                            DrawHatchProfile(canvasTwoD, Convert.ToInt32(IDSelect), 100, ShadeX, ShadeY, System.Windows.Media.Color.FromArgb(180, 255, 255, 128));
                                          
                                            Selection();
                                            p = 69;
                                            i = 69;
                                            break;
                                        }
                                    }
                                }
                            }
                    }
               
                for (int ii = 0; ii < 18; ii++)
                {

                    for (int jj = 0; jj < 1; jj++)
                    {
                        if ((xx <= clsGlobVar.ProfileCoordinate.MxMnCurved[ii, jj + 1]) && (xx >= clsGlobVar.ProfileCoordinate.MxMnCurved[ii, jj + 2]))
                        {
                            int id = Convert.ToInt16(clsGlobVar.ProfileCoordinate.MxMnCurved[ii, 0]);

                            for (int pp = 0; pp < 18; pp++)
                            {
                                for (int np = 0; np < 1; np++)
                                {
                                    if ((yy <= clsGlobVar.ProfileCoordinate.MxMnCurved[pp, np + 3]) && (yy >= clsGlobVar.ProfileCoordinate.MxMnCurved[pp, np + 4]) && (clsGlobVar.ProfileCoordinate.MxMnCurved[pp, np] == id))
                                    {
                                        TankID = Convert.ToInt16(id);
                                        if (TankID==22)
                                        {
                                        DrawHatchFOT(canvasTwoD, id, 100, ShadeX, ShadeY, System.Windows.Media.Color.FromArgb(180, 255, 255, 128));
                                        }
                                        else if (TankID == 29 || TankID == 30)
                                        {
                                            DrawHatchFWT(canvasTwoD, id, 100, ShadeX, ShadeY, System.Windows.Media.Color.FromArgb(180, 255, 255, 128));
                                        }
                                        else if (TankID == 31)
                                        {
                                            DrawHatchBT(canvasTwoD, id, 100, ShadeX, ShadeY, System.Windows.Media.Color.FromArgb(180, 255, 255, 128));
                                        }
                                        Selection();
                                        pp = 12;
                                        ii = 12;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                //Deck plan
                ShadeX = new double[18];
                ShadeY = new double[18];

                for (int i = 0; i <= 68; i++)
                {
                    for (int j = 1; j < 2; j++)

                        if ((xx <= clsGlobVar.mulDeckPlan[i, j]) && (xx >= clsGlobVar.mulDeckPlan[i, j + 1]))
                        {
                            IDSelect = clsGlobVar.mulDeckPlan[i, j - 1];

                            for (int p = 0; p <= 68; p++)
                            {
                                for (int n = 1; n < 2; n++)
                                {
                                    if ((yy <= clsGlobVar.mulDeckPlan[p, n + 2]) && (yy >= clsGlobVar.mulDeckPlan[p, n + 3]) && (clsGlobVar.mulDeckPlan[p, n - 1] == IDSelect))
                                    {
                                        IDSelect = clsGlobVar.mulDeckPlan[i, j - 1];
                                        string sCmd = "Select * from [DeckPlan] where Tank_ID='" + IDSelect + "'";
                                        DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
                                        command.CommandText = sCmd;
                                        command.CommandType = CommandType.Text;
                                        string Err = "";
                                        DataTable dtCoordinatesDeckPlan = new DataTable();
                                        dtCoordinatesDeckPlan = Models.DAL.clsDBUtilityMethods.GetTable(command, Err);
                                        for (int o = 1; o <= 17; o++)
                                        {
                                            string sc = Convert.ToString("X" + o);
                                            string sr = Convert.ToString("Y" + o);
                                            ShadeX[o] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[0][sc]);
                                            ShadeY[o] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[0][sr]);
                                        }
                                        TankID = Convert.ToInt16(IDSelect);
                                        DrawHatchDeckPlan(canvasTwoD, Convert.ToInt32(IDSelect), 100, ShadeX, ShadeY, System.Windows.Media.Color.FromArgb(180, 255, 255, 128));
                                        Selection();
                                        p = 69;
                                        i = 69;
                                        break;
                                    }
                                }
                            }
                        }
                }
            }
    
        }

        public void Selection()
        {
            if (TankID >= 1 && TankID <= 68)
            {
                dgTanks.SelectedIndex = TankID - 1;

            }
        }
    }

    public static class ProgressBarExtensions
    {
        private static TimeSpan duration = TimeSpan.FromSeconds(2);

        public static void SetPercent(this System.Windows.Controls.ProgressBar progressBar, double percentage)
        {
            System.Windows.Media.Animation.DoubleAnimation animation = new System.Windows.Media.Animation.DoubleAnimation(percentage, duration);
            for (int i = 0; i < 50; i++)
            {
                progressBar.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, animation);
            }
        }
    }
}
    

