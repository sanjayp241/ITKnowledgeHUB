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
    /// Interaction logic for FRMLoading.xaml
    /// </summary>
    public partial class FRMLoading : Window
    {
        public FRMLoading()
        {
            InitializeComponent();
        }

        private void PrgsLoading_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
